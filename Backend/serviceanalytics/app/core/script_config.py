import os

from minio import Minio

MODEL_NAME = "cbm-model.cbm"

MINIO_ENDPOINT = os.environ.get("MINIO_ENDPOINT", None)
MINIO_ACCESS_KEY = os.environ.get("MINIO_ACCESS_KEY", None)
MINIO_SECRET_KEY = os.environ.get("MINIO_SECRET_KEY", None)
MINIO_BUCKET_NAME = "ml-models"
MINIO_SECURE = False

def get_minio_client() -> Minio | None:
    """
    Creates MinIO client
    """
    if not MINIO_ENDPOINT or not MINIO_ACCESS_KEY or not MINIO_SECRET_KEY:
        print("MinIO credentials not set, cannot create MinIO client")
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
        print(f"Error while initializing MinIO client: {e}")
        raise