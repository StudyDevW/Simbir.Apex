from collections import defaultdict
from datetime import datetime, timedelta

from serviceanalytics.app.core.config import ML_BASE_INTERVAL
from serviceanalytics.app.schemas.alert_schema import AlertSchema
from serviceanalytics.app.schemas.event_schema import EventSchema


def get_all_sorted_events(alerts: list[AlertSchema]) -> list[EventSchema]:
    all_events: list[EventSchema] = []

    for alert in alerts:
        alert.rawData.sort(key=lambda x: x.timestamp)
        all_events.extend(alert.rawData)

    all_events.sort(key=lambda x: x.timestamp)
    return all_events


def split_events_to_intervals(
        alerts: list[AlertSchema]
) -> dict[datetime, list[EventSchema]]:
    all_events = get_all_sorted_events(alerts)
    if not all_events:
        return {}

    t_start_raw = all_events[0].timestamp
    t_start = t_start_raw.replace(minute=0, second=0, microsecond=0)

    if t_start > t_start_raw:
        t_start = t_start - timedelta(hours=1)

    interval_duration = timedelta(minutes=ML_BASE_INTERVAL)

    events_by_interval: dict[datetime, list[EventSchema]] = defaultdict(list)

    current_interval_start = t_start

    for event in all_events:
        while event.timestamp >= (current_interval_start + interval_duration):
            current_interval_start += interval_duration

        events_by_interval[current_interval_start].append(event)

    return events_by_interval


def get_alerts_by_interval(
        alerts: list[AlertSchema],
        interval_map: dict[datetime, list[EventSchema]]
) -> dict[datetime, list[AlertSchema]]:
    alerts_by_interval: dict[datetime, list[AlertSchema]] = defaultdict(list)
    interval_duration = timedelta(minutes=ML_BASE_INTERVAL)

    sorted_intervals = sorted(interval_map.keys())

    for alert in alerts:
        for start_time in sorted_intervals:
            if start_time <= alert.createdAt < (start_time + interval_duration):
                alerts_by_interval[start_time].append(alert)
                break

    return alerts_by_interval


def extract_features_predict(events: list[EventSchema]):
    # TODO: implement
    ...


def extract_features_train(alerts: list[AlertSchema]):
    # TODO: implement
    ...