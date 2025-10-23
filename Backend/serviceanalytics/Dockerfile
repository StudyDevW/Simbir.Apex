FROM python:3.13-slim

WORKDIR /service

COPY requirements.txt .

RUN pip install -r requirements.txt

COPY . .

ENV PYTHONPATH="${PYTHONPATH}:."

EXPOSE 8000

CMD ["python", "app/main.py"]


