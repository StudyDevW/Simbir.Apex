import os

import logging
import logging.config
from typing import Optional

import aiohttp
import yaml

script_dir = os.path.dirname(os.path.abspath(__file__))

try:
    SERVICE_MANAGER_URL = os.environ['SERVICE_MANAGER_URL']
except KeyError:
    logging.critical("Environment var 'SERVICE_MANAGER_URL' not set")
    SERVICE_MANAGER_URL = None

LOGGER_NAME = "service_analytics"
LOGGER_CONFIG_FILE = os.path.join(script_dir, 'logger-config.yml')

ML_BASE_INTERVAL = 60 # minutes


async def get_analyzer_url(force_refresh: bool = False) -> Optional[str]:
    """
    Fetching analyzer service URL from cache or service manager.
    """
    if not SERVICE_MANAGER_URL:
        logging.error(f"Service manager URL not set")
        return None

    global ANALYZER_URL_CACHE

    if ANALYZER_URL_CACHE and not force_refresh:
        return ANALYZER_URL_CACHE

    logging.info(f"Cache is invalidated. Checking service manager for new URL")

    try:
        async with aiohttp.ClientSession() as session:
            async with session.get(f"{SERVICE_MANAGER_URL}/api/Main/GetServiceInfo/service-analyzer") as res:
                if res.status == 200:
                    data = await res.json()
                    ep = data.get("EndPointService")
                    url = f"{ep.get("Address")}:{ep.get("Port")}"

                    if url:
                        ANALYZER_URL_CACHE = url
                        logging.info(f"Got new URL: {url}, caching it")
                        return url
                    else:
                        logging.error("Service manager sent empty URL")
                else:
                    logging.error(f"Service manager error: {res.status}")

    except aiohttp.ClientConnectorError:
        logging.error(f"Failed to connect to service manager at {SERVICE_MANAGER_URL}.")

    return None


def setup_logger():
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
