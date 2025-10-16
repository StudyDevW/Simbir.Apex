import config
import logging
import logging.config
import yaml

logger = logging.getLogger(config.LOGGER_NAME)

def main():
    with open(config.LOGGER_CONFIG_FILE, "rt") as f:
        try:
            conf = yaml.safe_load(f.read())
            logging.config.dictConfig(conf)
        except Exception as e:
            print(f"Error while set up config from file {config.LOGGER_CONFIG_FILE}: {e}")
            logging.basicConfig(level=logging.INFO)


    logger.info("Starting service-analytics")

if __name__ == "__main__":
    main()