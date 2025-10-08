# Сервис-сборщик

Данный сервис занимается сбором событий от сервиса-агента из брокера Kafka, дополняет синхронизирует, структурирует 
данные и отправляет обработанные данные в RabbitMQ другим сервисам.

## Зависимости

Для обеспечения работы необходимо настроить Kafka и RabbitMQ, используя `docker-compose` или изменив `src/main/resources/application.yml`

## Запуск

Сервис полностью готов к запуску с использованием Docker контейнера. Для использования Dockerfile контейнера в `docker-compose.yml` необходимо
предварительно настроить файл.

```yaml
services:
  # другие сервисы
  # ...
  app:
    build: . 
    container_name: app
    ports:
      - "8080:8080"
    environment:
      # параметры RabbitMQ
      SPRING_RABBITMQ_HOST: rabbitmq # название контейнера с RabbitMQ
      SPRING_RABBITMQ_PORT: 5672
      SPRING_RABBITMQ_USERNAME: user
      SPRING_RABBITMQ_PASSWORD: password

      # параметры кафка сервера
      BOOTSTRAP_SERVERS: kafka:29092 # название контейнера с Kafka + порт для INTERNAL целей
    depends_on:
      # по хорошему использовать это, так как сервис должен запускаться при удачном запуске RabbitMQ и Kafka
      kafka:
        condition: service_healthy # необходимо в kafka service добавить healthcheck, так как кафка может с первого
                                   # запуска не работать (пример есть в docker-compose.yml для тестирования данного сервиса)
      rabbitmq:
        condition: service_started # здесь проще
```

Далее, в терминале можно будет видеть логи приложения.
