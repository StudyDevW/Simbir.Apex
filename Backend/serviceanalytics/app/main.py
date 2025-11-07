import asyncio
import logging

import uvicorn
from fastapi import FastAPI

import app.api.controller as main_controller
from app.core import config
from app.core.config import setup_logger, setup_es_config

app = FastAPI()
app.include_router(main_controller.router)

if __name__ == "__main__":
    setup_logger()
    setup_es_config()

    logger = logging.getLogger(config.LOGGER_NAME)

    asyncio.run(config.register_in_manager())

    logger.info("Starting analytics service...")

    uvicorn.run("main:app", host="0.0.0.0", port=80, reload=True, log_config=config.LOGGER_CONFIG_FILE)