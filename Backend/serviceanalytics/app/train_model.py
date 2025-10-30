"""
Script for training model
"""

import asyncio
import tempfile

from catboost import CatBoostClassifier
from sklearn.model_selection import train_test_split

from app.core.script_config import get_minio_client, MINIO_BUCKET_NAME, MODEL_NAME
from app.data.analyzer_data import fetch_alerts_all_time
from app.ml.features_config import create_alerts_dataframe, create_events_dataframe, aggregate_features_train
from app.ml.model_training import prepare_data_and_features, train_model, evaluate_model, RND_SEED

def save_model(model: CatBoostClassifier):
    client = get_minio_client()

    with tempfile.NamedTemporaryFile(suffix='.cbm', delete=True) as tmp_file:
        try:
            model.save_model(tmp_file.name, format='cbm')
            print(f"Model saved to tempfile {tmp_file.name}")
        except Exception as e:
            print(f"Error while saving model: {e}")
            return

        try:
            if not client.bucket_exists(MINIO_BUCKET_NAME):
                client.make_bucket(MINIO_BUCKET_NAME)

            client.fput_object(
                MINIO_BUCKET_NAME,
                MODEL_NAME,
                tmp_file.name,
                content_type='application/octet-stream'
            )
            print(f"Model successfully saved in MinIO: {MINIO_BUCKET_NAME}/{MODEL_NAME}")
        except Exception as e:
            print(f"Error while saving in MinIO: {e}")

async def main():
    all_alerts = await fetch_alerts_all_time()

    df_alerts = create_alerts_dataframe(all_alerts)
    df_events = create_events_dataframe(all_alerts)

    df_aggregated = aggregate_features_train(
        df_events=df_events,
        df_alerts=df_alerts,
    )

    X, Y, cat_features = prepare_data_and_features(df_aggregated)

    while True:
        X_train_val, X_test, Y_train_val, Y_test = train_test_split(
            X, Y,
            test_size=0.2,
            random_state=RND_SEED,
            stratify=Y
        )

        X_train, X_val, Y_train, Y_val = train_test_split(
            X_train_val, Y_train_val,
            test_size=0.25,
            random_state=RND_SEED,
            stratify=Y_train_val
        )

        model = train_model(
            X_train=X_train,
            Y_train=Y_train,
            categorical_features=cat_features,
            X_val=X_val,
            Y_val=Y_val
        )

        evaluate_model(model=model, X_test=X_test, Y_test=Y_test)
        print("Save model? y/n")
        if input() == 'y':
            save_model(model)
            return
        else:
            print("Retraining...\n")
            continue

if __name__ == '__main__':
    asyncio.run(main())
