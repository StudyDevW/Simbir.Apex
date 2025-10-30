from fastapi import HTTPException

from app.data.analyzer_data import fetch_events, fetch_alerts
from app.es.expert_system import get_recommendations
from app.ml.predictor import load_model, predict_alerts
from app.schemas.response_schema import ResponseSchema


async def predict_next_hour() -> ResponseSchema:
    events = await fetch_events()
    alerts = await fetch_alerts()

    if not events or not alerts:
        raise HTTPException(status_code=400, detail="Events or alerts not found")

    try:
        model = load_model()

        res = predict_alerts(alerts, events, model)

        return get_recommendations(res, alerts)
    except Exception:
        raise HTTPException(status_code=500, detail="Failed to predict alerts")
