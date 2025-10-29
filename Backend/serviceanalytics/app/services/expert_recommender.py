import yaml
import os

class ExpertRecommender:
    def __init__(self):
        # Берём путь к текущему файлу класса
        current_dir = os.path.dirname(os.path.abspath(__file__))
        # Поднимаемся на уровень проекта, если config лежит в serviceanalytics/config
        self.yaml_path = os.path.normpath(os.path.join(current_dir, "..","..", "config", "expert.yml"))

        # Загружаем конфиг
        with open(self.yaml_path, "r", encoding="utf-8") as f:
            self.rules = yaml.safe_load(f)

    def recommend(self, ml_result):

        main_threat_prob = max(ml_result["probabilities"].values())
        threshold = 0.7 * main_threat_prob  # порог для "соседних угроз"

        recommendations = []
        related_threats = []

        for rule in self.rules.get("rules", []):
            print(rule)
            threat = rule["threat_type"]
            prob = ml_result["probabilities"].get(threat)
            print(threshold)
            if threat == "normal_activity":
                continue  # игнорируем нормальные активности и маловероятные угрозы
            
            if prob >= threshold:
                
                related_threats.append(threat)
                recommendations.extend(rule.get("advice", []))

        return {
            "main_threat": ml_result["predicted_threat"],
            "related_threats": related_threats,
            "recommendations": recommendations
        }
