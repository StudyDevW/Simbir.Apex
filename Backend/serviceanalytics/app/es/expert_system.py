import pandas as pd

from app.core.config import get_es_config
from app.schemas.alert_schema import AlertSchema
from app.schemas.response_schema import ResponseSchema, ResponseSeverity

def get_severity(prob: float) -> ResponseSeverity:
    if prob <= 0.3:
        return ResponseSeverity.low
    elif prob <= 0.6:
        return ResponseSeverity.medium
    elif prob <= 0.85:
        return ResponseSeverity.high
    else:
        return ResponseSeverity.critical

def get_recommendations(
        data: tuple[pd.Timestamp, float, int],
        alerts: list[AlertSchema]
) -> ResponseSchema:
    """
    Get recommendations based on ML prediction and alert data
    :param data: tuple with ``pd.Timestamp`` of next timestamp, ``float`` probability and ``int`` prediction
    :param alerts: list with ``AlertSchema`` for ``LAG_INTERVAL * ML_BASE_INTERVAL`` interval
    :return: ``ResponseSchema`` with ``probability`` and ``guidelines`` in it
    """
    conf = get_es_config()
    rules = conf.get("rules", [])
    time, prob, _ = data

    alerts.sort(key=lambda x: x.createdAt)
    pred_alert = alerts[-1]
    pred_threat = pred_alert.title

    for r in rules:
        if r.get("threat_type", "") == pred_threat:
            return ResponseSchema(
                predicted_for=time.to_pydatetime(),
                probability=prob,
                threat_type=pred_threat,
                severity=get_severity(prob),
                guidelines=r.get("advice", [])
            )

    return ResponseSchema(
        probability=prob,
        predicted_for=time.to_pydatetime(),
        threat_type=pred_threat,
        severity=get_severity(prob),
        guidelines=["Нет рекоммендаций для данной угрозы"]
    )
