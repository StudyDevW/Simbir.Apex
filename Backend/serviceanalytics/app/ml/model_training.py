import logging
import os

import pandas as pd
from catboost import CatBoostClassifier
from sklearn.metrics import roc_auc_score, classification_report

from serviceanalytics.app.core.config import LOGGER_NAME, MODEL_EXPORT_FILE

logger = logging.getLogger(LOGGER_NAME)
RND_SEED = 42


def prepare_data_and_features(df_final: pd.DataFrame) -> tuple[pd.DataFrame, pd.Series, list[str]]:
    """
    Splits DataFrame to labels, data and determines categorial features
    :param df_final: final DataFrame ready for training
    :return: tuple of (data: ``pd.DataFrame``, labels: ``pd.Series``, categorial features: ``list[str]``)
    """
    Y = df_final['is_alert']
    X = df_final.drop(columns=['is_alert'])
    categorical_features = [col for col in X.columns if X[col].dtype == object]

    return X, Y, categorical_features


def train_model(
        X_train: pd.DataFrame,
        Y_train: pd.Series,
        categorical_features: list[str],
        X_val: pd.DataFrame,
        Y_val: pd.Series
) -> CatBoostClassifier:
    """
    Trains CatBoost model based on data
    :param X_train: data for training
    :param Y_train: labels for training
    :param categorical_features: explicitly passed to model categorial features for training
    :param X_val: validation data
    :param Y_val: validation labels
    :return: trained model
    """
    logger.info(f"Start training CatBoost model...")

    model = CatBoostClassifier(
        iterations=500,
        learning_rate=0.05,
        loss_function='Logloss',
        eval_metric='F1',
        random_seed=RND_SEED,
        verbose=True,
        cat_features=categorical_features,
        early_stopping_rounds=50
    )

    model.fit(
        X_train, Y_train,
        eval_set=(X_val, Y_val),
        use_best_model=True
    )
    logger.info("Model training done")
    return model


def evaluate_model(model: CatBoostClassifier,
                   X_test: pd.DataFrame,
                   Y_test: pd.Series) -> float:
    """
    Evaluates model performance on test data
    :param model: trained model
    :param X_test: test data
    :param Y_test: test labels
    :return: AUC-ROC score
    """
    logger.info("Evaluating model performance")

    Y_proba = model.predict_proba(X_test)[:, 1]

    Y_pred = model.predict(X_test)

    roc_auc = roc_auc_score(Y_test, Y_proba)
    print(f"AUC-ROC on test data: {roc_auc:.4f}")

    print("\nClassification Report (threshold 0.5):")
    print(classification_report(Y_test, Y_pred, zero_division=0))

    return roc_auc


def export_model(model: CatBoostClassifier):
    """
    Exports trained model to a file
    :param model: trained model
    """
    os.makedirs(MODEL_EXPORT_FILE, exist_ok=True)

    try:
        model.save_model(MODEL_EXPORT_FILE)
        logger.info(f"Model successfully saved in {MODEL_EXPORT_FILE}")
    except Exception as e:
        logger.error(f"Error while saving model: {e}")
