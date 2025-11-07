import logging

from fastapi import HTTPException

from app.core.config import LOGGER_NAME
from app.data.analyzer_data import fetch_alerts
from app.es.expert_system import get_recommendations
from app.ml.predictor import load_model, predict_alerts
from app.schemas.response_schema import ResponseSchema

logger = logging.getLogger(LOGGER_NAME)

async def predict_next_hour() -> ResponseSchema:
    alerts = await fetch_alerts()

    if not alerts:
        raise HTTPException(status_code=400, detail="Events or alerts not found")

    try:
        model = load_model()

        res = predict_alerts(alert_data=alerts, model=model)

        return get_recommendations(res, alerts)
    except Exception as ex:
        logger.error(f"Failed to predict alerts: {ex}")
        raise HTTPException(status_code=500, detail="Failed to predict alerts")
