import json
import os

import logging
import logging.config
import socket
from typing import Optional

import aiohttp
import yaml
from minio import Minio

from dotenv import load_dotenv

load_dotenv()

PORT = 8000
ML_BASE_INTERVAL = 60  # minutes

MODEL_NAME = "cbm-model.cbm"

LOGGER_NAME = "service_analytics"

LOGGER_CONFIG_FILE = "config/logger-config.yml"
EXPERT_SYSTEM_CONFIG = "config/expert.yml"

SERVICE_MANAGER_URL = os.environ.get('SERVICE_MANAGER_URL', None)

MINIO_ENDPOINT = os.environ.get("MINIO_ENDPOINT", None)
MINIO_ACCESS_KEY = os.environ.get("MINIO_ACCESS_KEY", None)
MINIO_SECRET_KEY = os.environ.get("MINIO_SECRET_KEY", None)
MINIO_BUCKET_NAME = "ml-models"
MINIO_SECURE = False

_ANALYZER_URL_CACHE = None
_ES_CONFIG = None

logger = logging.getLogger(LOGGER_NAME)


def get_minio_client() -> Minio | None:
    """
    Creates MinIO client
    """
    if not MINIO_ENDPOINT or not MINIO_ACCESS_KEY or not MINIO_SECRET_KEY:
        logger.error("MinIO credentials not set, cannot create MinIO client")
        return None
    try:
        client = Minio(
            MINIO_ENDPOINT,
            access_key=MINIO_ACCESS_KEY,
            secret_key=MINIO_SECRET_KEY,
            secure=MINIO_SECURE
        )
        return client
    except Exception as e:
        logger.error(f"Error while initializing MinIO client: {e}")
        raise


async def register_in_manager():
    """
    Registering service in service manager.
    """
    if not SERVICE_MANAGER_URL:
        logger.error(f"Service manager URL not set, skipping registration")
        return None

    service_data = {
        "ServiceName": "ServiceAnalytics",
        "EndPointService": {
            "Address": socket.gethostbyname(socket.getfqdn()),
            "Port": PORT
        },
        "Id": 0
    }

    try:
        async with aiohttp.ClientSession() as session:

            first_json = json.dumps(service_data)
            second_json = json.dumps(first_json)  # двойная сериализация

            async with session.post(
                f"http://{SERVICE_MANAGER_URL}/api/Main/InsertService",
                data=second_json,
                headers={"Content-Type": "application/json"}
            ) as res:
                if res.status == 200:
                    logger.info("Service registered successfully")
                else:
                    error_text = await res.text()
                    logger.error(f"Service manager error: {error_text}")
    except aiohttp.ClientConnectorError:
        logger.error(f"Failed to connect to service manager at {SERVICE_MANAGER_URL}.")


async def get_analyzer_url(force_refresh: bool = False) -> Optional[str]:
    """
    Fetching analyzer service URL from cache or service manager.
    """
    if not SERVICE_MANAGER_URL:
        logging.error(f"Service manager URL not set")
        return None

    global _ANALYZER_URL_CACHE

    if _ANALYZER_URL_CACHE and not force_refresh:
        return _ANALYZER_URL_CACHE

    logger.info(f"Cache is invalidated. Checking service manager for new URL")

    try:
        async with aiohttp.ClientSession() as session:
            async with session.get(f"{SERVICE_MANAGER_URL}/api/main/getserviceinfo/service-analyzer") as res:
                if res.status == 200:
                    data = await res.json()
                    ep = data.get("EndPointService")
                    url = f"{ep.get("Address")}:{ep.get("Port")}"

                    if url:
                        _ANALYZER_URL_CACHE = url
                        logger.info(f"Got new URL: {url}, caching it")
                        return url
                    else:
                        logger.error("Service manager sent empty URL")
                else:
                    logger.error(f"Service manager error: {res.status}")

    except aiohttp.ClientConnectorError:
        logger.error(f"Failed to connect to service manager at {SERVICE_MANAGER_URL}.")

    return None


def setup_logger():
    """
    Setting up logger for application
    """
    if os.path.exists(LOGGER_CONFIG_FILE):
        with open(LOGGER_CONFIG_FILE, "rt") as f:
            try:
                conf = yaml.safe_load(f.read())
                logging.config.dictConfig(conf)
                print(f"INFO: Logging configured successfully from: {LOGGER_CONFIG_FILE}")
            except Exception as e:
                print(f"Error while set up config from file {LOGGER_CONFIG_FILE}: {e}")
                logging.basicConfig(level=logging.INFO)
    else:
        print(f"WARNING: Logging config file not found at {LOGGER_CONFIG_FILE}. Using basic INFO config.")
        logging.basicConfig(level=logging.INFO)


def setup_es_config():
    """
    Setting up expert system config
    """
    if os.path.exists(EXPERT_SYSTEM_CONFIG):
        with open(EXPERT_SYSTEM_CONFIG, "rt") as f:
            try:
                global _ES_CONFIG
                _ES_CONFIG = yaml.safe_load(f.read())
                logger.info(f"Expert system config loaded from: {EXPERT_SYSTEM_CONFIG}")
            except Exception as e:
                logger.error(f"Error while set up config from file {EXPERT_SYSTEM_CONFIG}: {e}")
    else:
        logger.critical(f"Expert system config file not found at {EXPERT_SYSTEM_CONFIG}")


def get_es_config() -> dict | None:
    """
    Returns expert system config
    """
    if not _ES_CONFIG:
        logger.info("Expert system config not loaded, reloading...")
        setup_es_config()
    return _ES_CONFIG
