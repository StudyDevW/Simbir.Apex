import os

import logging
import logging.config
import yaml

script_dir = os.path.dirname(os.path.abspath(__file__))

LOGGER_NAME = "serviceanalytics"
LOGGER_CONFIG_FILE = os.path.join(script_dir, 'logger-config.yml')

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