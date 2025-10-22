import logging

from serviceanalytics.app.core import config
from serviceanalytics.app.core.config import setup_logger

logger = logging.getLogger(config.LOGGER_NAME)

def main():
    setup_logger()

    logger.info("Starting analytics service...")

if __name__ == "__main__":
    main()