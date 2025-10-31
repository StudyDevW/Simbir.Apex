# ===== Сборка Maven =====
FROM maven:3.9.6-eclipse-temurin-23 AS build

WORKDIR /app
COPY pom.xml .
RUN mvn dependency:go-offline

COPY src ./src
RUN mvn clean package -DskipTests

# ===== Финальный образ =====
FROM eclipse-temurin:21-jre-alpine

WORKDIR /app
COPY --from=build /app/target/*.jar app.jar

EXPOSE 8080

# Параметры подключения к БД (можно переопределить через compose)
ENV DB_HOST=postgres_db
ENV DB_PORT=5432
ENV DB_NAME=simbirapex
ENV DB_USER=practice_user
ENV DB_PASS=root

ENTRYPOINT ["java", "-jar", "app.jar"]
