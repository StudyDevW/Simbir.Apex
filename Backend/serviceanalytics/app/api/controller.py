import logging
import random

from fastapi import APIRouter

from app.core.config import LOGGER_NAME
from app.schemas.response_schema import ResponseSchema

router = APIRouter(tags=["Main controller"])
logger = logging.getLogger(LOGGER_NAME)

@router.get("/health")
async def health_check():
    """
    Health check endpoint. Returns ``{"status": "ok"}`` if service is healthy.
    """
    return {"status": "ok"}

@router.post("/train")
async def retrain_model():
    """
    Retraining model with newer data from service analyzer.
    """
    # TODO: add authentication
    ...

@router.get("/predict", response_model=ResponseSchema)
async def predict_next_hour():
    """
    Predicts next hour alerts probability. Returns ``ResponseSchema`` with ``probability`` and ``guidelines`` in it.
    """
    # TODO: add authentication
    # TODO: implement prediction
    # TODO: return real data

    # result = predict_next_hour()
    # will return some data in ResponseSchema

    return ResponseSchema(
        probability=random.uniform(0, 1),
        guidelines=["Guideline 1", "Guideline 2", "Guideline 3"]
    )