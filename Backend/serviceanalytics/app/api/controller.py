import logging

from fastapi import APIRouter

from app.core.config import LOGGER_NAME
from app.schemas.response_schema import ResponseSchema
from app.services.service import predict_next_hour

router = APIRouter(tags=["Main controller"])
logger = logging.getLogger(LOGGER_NAME)

@router.get("/health")
async def health_check():
    """
    Health check endpoint. Returns ``{"status": "ok"}`` if service is healthy.
    """
    return {"status": "ok"}

@router.get("/predict", response_model=ResponseSchema)
async def get_predictions():
    """
    Predicts next hour alerts probability. Returns ``ResponseSchema`` with ``probability`` and ``guidelines`` in it.
    """

    return await predict_next_hour()