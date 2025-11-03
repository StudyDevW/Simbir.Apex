# Сервис аналитики

Данный сервис на основе событий и угроз (получает по API от сервиса-анализатора) строит прогнозы для вывода в виде 
текстовой аналитике  рекомендаций по устранению угроз и диаграмм. Продумать настройку рекомендательной системы в виде 
файла от эксперта, который подкладывается «под ноги» сервиса.

> [!NOTE]
> Модель для экспертной системы использует поле из AlertDto title, с помощью которого она
> ищет в `expert.yml` подходящие рекомендации
> ```python
> # pred_threat -- будет иметь такое же значение что и title у AlertDto
> # threat_type -- из expert.yml
> if r.get("threat_type", "") in pred_threat:
>   ...
> ```

## Запуск

Сервис использует MinIO для хранения обученной модели, поэтому необходимо добавить MinIO в `docker-compose.yml`
### Запуск в Docker Compose
В `docker-compose.yml` файле используйте сервис так
```yaml
services:
  # ...
  minio:
    container_name: minio # любое имя
    image: minio/minio:latest
    ports:
      - "9000:9000"
      - "9001:9001"
    environment:
      MINIO_ROOT_USER: minio_root_user
      MINIO_ROOT_PASSWORD: minio_root_password
    command: server /data --console-address ":9001"
    volumes:
      - minio_data:/data
    healthcheck:
      test: [ "CMD", "curl", "-f", "http://localhost:9000/minio/health/live" ] # порт из ports
      interval: 30s
      timeout: 20s
      retries: 3
    service-analytics:
    build:
      context: .
      dockerfile: Dockerfile
      environment:
        SERVICE_MANAGER_URL: http://192.168.3.2:8080 # адрес сервиса управленца

        MINIO_ENDPOINT: minio:9000
        MINIO_ACCESS_KEY: minio_root_user
        MINIO_SECRET_KEY: minio_root_password
      ports:
        - "8000:8000" # порт (стандартный у сервиса 8000)
      depends_on:
        - minio # только после запуска MinIO
  # ...
```

Затем запустите docker-compose

```bash
docker-compose up --build
```

И обучите модель. Для начала перейдите в интерактивную оболочку внутри сервиса

```bash
docker-compose exec service-analytics bash
```
Внутри интерактивной оболочки сервиса
```bash
python app/train_model.py 
```

Затем программа начнет обучать модель, по окончанию выведет статистику и возможность переобучить модель. 

```
...
Stopped by overfitting detector  (100 iterations wait)

bestTest = 0.7333333333
bestIteration = 77

Shrink model to first 78 iterations.
Model training done
Evaluating model performance
AUC-ROC on test data: 0.7576

Classification Report (threshold 0.5):
              precision    recall  f1-score   support

         0.0       0.58      0.78      0.67         9
         1.0       0.75      0.55      0.63        11

    accuracy                           0.65        20
   macro avg       0.67      0.66      0.65        20
weighted avg       0.68      0.65      0.65        20

Save model? y/n
```

При выборе `y` модель сохраняется в MinIO.

Сервис имеет OpenAPI документацию по адресу ```http://localhost:8000/docs```