import os

script_dir = os.path.dirname(os.path.abspath(__file__))

LOGGER_NAME = "service-analytics"
LOGGER_CONFIG_FILE = os.path.join(script_dir, 'logger-config.yml')