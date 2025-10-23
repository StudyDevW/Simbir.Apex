import aiohttp
import logging

import app.core.config as config
from app.schemas.alert_schema import AlertSchema
from app.schemas.event_schema import EventSchema

logger = logging.getLogger(config.LOGGER_NAME)

async def fetch_alerts(time_period: int = config.ML_BASE_INTERVAL) -> list[AlertSchema] | None:
    """
    Fetch alerts from service analyzer for last ``time_period`` minutes
    :param time_period: period in minutes, defaults to 60 minutes
    :return: alert list
    """
    if time_period < 1:
        return None
    logger.info(f"Fetching alerts for last {time_period} minutes")

    url = await config.get_analyzer_url()
    if not url:
        logger.error("Failed to get analyzer URL")
        return None

    async with aiohttp.ClientSession() as session:
        async with session.get(f"{url}/api/alerts/period/{time_period}") as res:
            data = await res.json()
            logger.info(f"Got {len(data)} alerts")

            return [AlertSchema.model_validate(item) for item in data]


async def fetch_events(time_period: int = config.ML_BASE_INTERVAL) -> list[EventSchema] | None:
    """
    Fetch events from service analyzer for last ``time_period`` minutes
    :param time_period: period in minutes, defaults to 60 minutes
    :return: alert list
    """
    if time_period < 1:
        return None
    logger.info(f"Fetching events for last {time_period} minutes")

    url = await config.get_analyzer_url()
    if not url:
        logger.error("Failed to get analyzer URL")
        return None

    async with aiohttp.ClientSession() as session:
        async with session.get(f"{url}/api/events/period/{time_period}") as res:
            data = await res.json()
            logger.info(f"Got {len(data)} events")

            return [EventSchema.model_validate(item) for item in data]


async def fetch_alerts_all_time() -> list[AlertSchema] | None:
    """
    Fetch alerts from service analyzer for all time
    :return: alert list
    """
    logger.info(f"Fetching alerts for all time")
    url = await config.get_analyzer_url(True)
    if not url:
        logger.error("Failed to get analyzer URL")
        return None

    async with aiohttp.ClientSession() as session:
        async with session.get(f"{url}/api/alerts") as res:
            data = await res.json()
            logger.info(f"Got {len(data)} alerts")

            return [AlertSchema.model_validate(item) for item in data]
