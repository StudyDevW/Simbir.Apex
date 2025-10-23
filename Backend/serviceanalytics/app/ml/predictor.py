import os

import logging
from datetime import timedelta

import pandas as pd
from catboost import CatBoostClassifier

from app.core.config import MODEL_EXPORT_FILE, LOGGER_NAME, ML_BASE_INTERVAL
from app.ml.features_config import LAG_INTERVAL, create_alerts_dataframe, aggregate_features_pred, \
    create_events_dataframe, append_events_to_exists_df
from app.schemas.alert_schema import AlertSchema

from app.schemas.event_schema import EventSchema

logger = logging.getLogger(LOGGER_NAME)


def load_model() -> CatBoostClassifier:
    """
    Loading pretrained CatBoost model from file defined in ``serviceanalytics/app/core/config.py``

    :return: ``CatBoostClassifier`` model.
    """
    if not os.path.exists(MODEL_EXPORT_FILE):
        logger.error(f"Model export file {MODEL_EXPORT_FILE} not found.")
        raise FileNotFoundError(f"Model export file {MODEL_EXPORT_FILE} not found.")

    logger.info(f"Loading model from file {MODEL_EXPORT_FILE}...")
    try:
        model = CatBoostClassifier().load_model(MODEL_EXPORT_FILE, format='cbm')
        logger.info("Model successfully loaded")

        return model
    except Exception as e:
        logger.error(f"Error while loading model: {e}")
        raise


def predict_alerts(
        alert_data: list[AlertSchema],
        events_data: list[EventSchema],
        model: CatBoostClassifier
) -> tuple[pd.Timestamp, float, int]:
    """
    Predicting alerts in next ``ML_BASE_INTERVAL``
    :param alert_data: list with ``AlertSchema`` for ``LAG_INTERVAL * ML_BASE_INTERVAL`` interval
    :param events_data: list with ``EventSchema`` for ``LAG_INTERVAL * ML_BASE_INTERVAL`` interval
    :param model: pretrained ``CatBoostClassifier`` model
    :return: tuple with ``pd.Timestamp`` of next timestamp, ``float`` probability and ``int`` prediction
    """
    df_alerts = create_alerts_dataframe(alert_data)
    df_events_from_alerts = create_events_dataframe(alert_data)

    df_events = append_events_to_exists_df(df_events_from_alerts, events_data)

    if df_events.empty and df_alerts.empty:
        logger.info("No new data in this period")
        return None, 0.0, 0

    X_inference = aggregate_features_pred(
        df_events=df_events,
        df_alerts=df_alerts,
    )

    if X_inference.empty:
        logger.info(f"Not enough data for calculation lag W={LAG_INTERVAL}.")
        return None, 0.0, 0

    Y_proba = model.predict_proba(X_inference)[:, 1][0]
    Y_pred = model.predict(X_inference)[0]

    next_timestamp = X_inference.index[0] + timedelta(minutes=ML_BASE_INTERVAL)

    logger.info(f"Alert prediction for {next_timestamp}: Probability {Y_proba:.4f}, Class {Y_pred}")

    return next_timestamp, Y_proba, Y_pred
