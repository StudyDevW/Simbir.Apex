FROM python:3.13-slim

WORKDIR /trainer

COPY requirements.txt .

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        build-essential && \
    rm -rf /var/lib/apt/lists/*

RUN pip install -r requirements.txt

COPY . .

ENV PYTHONPATH="${PYTHONPATH}:."

