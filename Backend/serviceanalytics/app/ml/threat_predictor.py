import json
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.ensemble import RandomForestClassifier
import joblib
import os


class ThreatPredictor:
    def __init__(
        self,
        model_path=None,
        vectorizer_path=None
    ):
        # Берём путь к папке с этим файлом
        current_dir = os.path.dirname(os.path.abspath(__file__))

        self.model_path = model_path or os.path.join(current_dir, "threat_model.pkl")
        self.vectorizer_path = vectorizer_path or os.path.join(current_dir, "vectorizer.pkl")

        self.model = None
        self.vectorizer = None

    def train(self, training_data_path):
        with open(training_data_path, "r", encoding="utf-8") as f:
            data = json.load(f)

        texts = [" ".join(item["events"]) for item in data]
        labels = [item["threat_type"] for item in data]

        self.vectorizer = TfidfVectorizer()
        X = self.vectorizer.fit_transform(texts)

        self.model = RandomForestClassifier(
            n_estimators=200,
            random_state=42,
            min_samples_leaf=2,
            max_features="sqrt",
        )

        self.model.fit(X, labels)

        os.makedirs(os.path.dirname(self.model_path), exist_ok=True)
        joblib.dump(self.model, self.model_path)
        joblib.dump(self.vectorizer, self.vectorizer_path)
        print("✅ Model trained and saved")

    def predict(self, events):
        if self.model is None or self.vectorizer is None:
            self.model = joblib.load(self.model_path)
            self.vectorizer = joblib.load(self.vectorizer_path)

        text = " ".join(events)
        X = self.vectorizer.transform([text])
        proba = self.model.predict_proba(X)[0]
        classes = self.model.classes_

        probs_dict = {cls: float(p) for cls, p in zip(classes, proba)}
        pred = max(probs_dict, key=probs_dict.get)

        return {
            "predicted_threat": pred,
            "probabilities": probs_dict
        }
