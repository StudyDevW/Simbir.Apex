from fastapi import HTTPException

from app.data.analyzer_data import fetch_events, fetch_alerts
from app.ml.predictor import load_model, predict_alerts
from app.schemas.response_schema import ResponseSchema


async def predict_next_hour() -> ResponseSchema:
    events = await fetch_events()
    alerts = await fetch_alerts()

    if not events:
        raise HTTPException(status_code=404)

    try:
        model = load_model()

        res = predict_alerts(alerts, events, model)

        # TODO: pass to ES service
        # second value in res = probability
        # third value = class (0 or 1)
    except Exception:
        raise HTTPException(status_code=500)
