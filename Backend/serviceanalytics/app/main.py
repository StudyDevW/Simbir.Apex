import logging
from contextlib import asynccontextmanager

import uvicorn
from fastapi import FastAPI

import app.api.controller as main_controller
from app.core import config
from app.core.config import setup_logger

setup_logger()
logger = logging.getLogger(config.LOGGER_NAME)

@asynccontextmanager
async def lifespan(_: FastAPI):
    await config.register_in_manager()
    yield

app = FastAPI(lifespan=lifespan)

app.include_router(main_controller.router)

if __name__ == "__main__":
    logger.info("Starting analytics service...")

    uvicorn.run("main:app", host="0.0.0.0", port=config.PORT, reload=True, log_config=config.LOGGER_CONFIG_FILE)