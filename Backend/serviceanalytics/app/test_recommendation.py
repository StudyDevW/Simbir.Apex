# run_analysis.py
import os
from ml.threat_predictor import ThreatPredictor
from services.expert_recommender import ExpertRecommender

def main():
    # Пути (подправь под свою структуру)
    base_dir = os.path.dirname(os.path.abspath(__file__))
    data_path = os.path.join(base_dir, "data", "training_data.json")
    yaml_path = os.path.join(base_dir, "..", "config", "expert.yml")
    yaml_path = os.path.normpath(yaml_path)

    predictor = ThreatPredictor()
    recommender = ExpertRecommender()

    # Пример входных событий (raw_data -> events)
    events = ["LOGIN_FAIL", "LOGIN_FAIL", "LOGIN_SUCCESS", "FILE_DELETE"]

    # Если модель и векторизатор есть — используем реальное предсказание
   
    print("Using trained model for prediction...")
    ml_result = predictor.predict(events)
  
    # Получаем рекомендации
    rec = recommender.recommend(ml_result)

    # Печать результата
    print("\n=== ML result ===")
    print(ml_result)
    print("\n=== Recommendations ===")
    print(f"Main threat: {rec['main_threat']}")
    print(f"Related threats: {rec['related_threats']}")
    print("Advice:")
    for a in rec["recommendations"]:
        print(f" - {a}")

if __name__ == "__main__":
    main()