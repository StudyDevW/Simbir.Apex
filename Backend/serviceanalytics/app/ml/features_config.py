import pandas as pd
import numpy as np

from app.core.config import ML_BASE_INTERVAL
from app.schemas.alert_schema import AlertSchema
from app.schemas.event_schema import EventSchema

LAG_INTERVAL = 4
FREQ = f"{ML_BASE_INTERVAL}T"


def create_alerts_dataframe(alerts: list[AlertSchema]) -> pd.DataFrame:
    """
    Creating dataframe from alert list
    :param alerts: list of alerts
    :return: alerts dataframe
    """
    if not alerts:
        return pd.DataFrame()

    alerts_data = [alert.model_dump(exclude={'rawData'}) for alert in alerts]

    df_alerts = pd.DataFrame(alerts_data)

    df_alerts['createdAt'] = pd.to_datetime(df_alerts['createdAt'])
    df_alerts = df_alerts.set_index('createdAt').sort_index()

    return df_alerts


def create_events_dataframe(alerts: list[AlertSchema]) -> pd.DataFrame:
    """
    Creating event dataframe from alert list
    :param alerts: list of alerts
    :return: events dataframe
    """
    if not alerts:
        return pd.DataFrame()

    all_events_data = []
    for alert in alerts:
        events = [event.model_dump() for event in alert.rawData]
        all_events_data.extend(events)

    if not all_events_data:
        return pd.DataFrame()

    df_events = pd.DataFrame(all_events_data)

    df_events['timestamp'] = pd.to_datetime(df_events['timestamp'])
    df_events = df_events.set_index('timestamp').sort_index()

    return df_events

def append_events_to_exists_df(df: pd.DataFrame, events: list[EventSchema]) -> pd.DataFrame:
    """
    Appends events to existing DataFrame with events
    :param df: existing DataFrame with events
    :param events: list of events
    :return: appended events dataframe
    """
    if not events:
        return df

    df_events = pd.concat([df, pd.DataFrame(events)])

    df_events['timestamp'] = pd.to_datetime(df_events['timestamp'])
    df_events = df_events.set_index('timestamp').sort_index()

    df_events.drop(inplace=True)

    return df_events


def aggregate_event_features(df_events: pd.DataFrame) -> pd.DataFrame:
    """
    Aggregates EventSchema features
    :param df_events: events DataFrame indexed by time
    :return: aggregated with features DataFrame
    """

    def mode_value(x):
        return x.mode()[0] if not x.empty else 'none'

    def ratio_count(x, target_value):
        if x.empty or x.shape[0] == 0:
            return 0.0
        return np.sum((x == target_value)) / x.shape[0]

    df_features = df_events.resample(FREQ).agg(
        event_count=('eventId', 'count'),
        unique_ips=('ip', pd.Series.nunique),
        lan_count=('isLan', 'sum'),
        wan_count=('isLan', lambda x: np.sum((x == False))),

        event_high_count=('severity', lambda x: np.sum((x == 'high'))),
        event_medium_ratio=('severity', lambda x: ratio_count(x, 'medium')),
        mode_event_severity=('severity', mode_value),

        auth_ratio=('category', lambda x: ratio_count(x, 'auth')),
        network_ratio=('category', lambda x: ratio_count(x, 'network')),
        mode_event_category=('category', mode_value),

        unique_devices=('device', pd.Series.nunique)
    )

    df_features = df_features.fillna({
        col: 0.0 if df_features[col].dtype in [np.float64, np.int64] else 'none'
        for col in df_features.columns
    })

    return df_features


def aggregate_alert_features(df_alerts: pd.DataFrame) -> pd.DataFrame:
    """
    Aggregates EventSchema features
    :param df_alerts: alerts DataFrame indexed by time
    :return: aggregated with features DataFrame
    """

    def mode_value(x):
        return x.mode()[0] if not x.empty else 'none'

    df_alerts_agg = df_alerts.resample(FREQ).agg(
        alert_count=('id', 'count'),
        false_positive_count=('status', lambda x: np.sum((x == 'false_positive'))),

        mode_alert_status=('status', mode_value),
        mode_alert_severity=('severity', mode_value),
        mode_hostname=('hostname', mode_value)
    )

    df_alerts_agg['is_alert'] = (df_alerts_agg['alert_count'] > 0).astype(int)

    df_alerts_agg = df_alerts_agg.fillna({
        'alert_count': 0,
        'false_positive_count': 0,
        'mode_alert_status': 'none',
        'mode_alert_severity': 'none',
        'mode_hostname': 'none'
    })

    return df_alerts_agg


def aggregate_features_train(df_events: pd.DataFrame, df_alerts: pd.DataFrame) -> pd.DataFrame:
    """
    Aggregates events and alerts for training, appends lags and time context

    :param df_events: event DataFrame indexed by time.
    :param df_alerts: alert DataFrame indexed by tim.
    :return: final DataFrame ready for training
    """
    df_features = aggregate_event_features(df_events)
    df_alerts_agg = aggregate_alert_features(df_alerts)

    df_final = df_features.merge(df_alerts_agg, left_index=True, right_index=True, how='outer')
    df_final['is_alert_future'] = df_final['is_alert'].shift(-1)
    df_final = df_final.drop(columns=['is_alert'])

    df_final = df_final.rename(columns={'is_alert_future': 'is_alert'})

    df_final = df_final.fillna({
        col: 0.0 if df_final[col].dtype in [np.float64, np.int64] else 'none_missing'
        for col in df_final.columns
    })

    df_final['hour_of_day'] = df_final.index.hour
    df_final['day_of_week'] = df_final.index.dayofweek
    df_final['is_weekend'] = df_final['dayofweek'].apply(lambda x: 1 if x >= 5 else 0)

    lag_features = [col for col in df_final.columns if
                    col not in ['hour_of_day', 'day_of_week', 'is_weekend', 'is_alert']]

    lag_features.append('is_alert')

    for feature in lag_features:
        for lag in range(1, LAG_INTERVAL + 1):
            new_col_name = f'{feature}_lag_{lag}'
            df_final[new_col_name] = df_final[feature].shift(lag)

            if df_final[feature].dtype == object:
                df_final[new_col_name].fillna('missing_lag', inplace=True)

    df_final = df_final.iloc[LAG_INTERVAL:].dropna(subset=['is_alert'])

    return df_final


def aggregate_features_pred(df_events: pd.DataFrame, df_alerts: pd.DataFrame) -> pd.DataFrame:
    """
    Aggregates events and alerts for predictions, appends lags and time context

    :param df_events: event DataFrame indexed by time.
    :param df_alerts: alert DataFrame indexed by tim.
    :return: final DataFrame ready for training
    """
    df_features = aggregate_event_features(df_events)
    df_alerts_agg = aggregate_alert_features(df_alerts)

    df_final = df_features.merge(df_alerts_agg, left_index=True, right_index=True, how='outer')

    df_final = df_final.fillna({
        col: 0.0 if df_final[col].dtype in [np.float64, np.int64] else 'none_missing'
        for col in df_final.columns
    })

    df_final['hour_of_day'] = df_final.index.hour
    df_final['day_of_week'] = df_final.index.dayofweek
    df_final['is_weekend'] = df_final['dayofweek'].apply(lambda x: 1 if x >= 5 else 0)

    lag_features = [col for col in df_final.columns if
                    col not in ['hour_of_day', 'day_of_week', 'is_weekend', 'is_alert']]

    lag_features.append('is_alert')

    for feature in lag_features:
        for lag in range(1, LAG_INTERVAL + 1):
            new_col_name = f'{feature}_lag_{lag}'
            df_final[new_col_name] = df_final[feature].shift(lag)

            if df_final[feature].dtype == object:
                df_final[new_col_name].fillna('missing_lag', inplace=True)

    df_final = df_final.iloc[LAG_INTERVAL:]

    X_inference = df_final.iloc[[-1]]

    X_inference = X_inference.drop(columns=['is_alert'])

    return X_inference
