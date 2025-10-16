# 1. Стейдж сборки
FROM maven:4.0.0-rc-4-eclipse-temurin-21 AS build
WORKDIR /app

COPY pom.xml .
RUN mvn dependency:go-offline

COPY src ./src
RUN mvn clean package -DskipTests

# 2. Минимальный контейнер для запуска
FROM eclipse-temurin:21-jre-alpine
WORKDIR /app

ARG JAR_FILE=target/collector-0.0.1-SNAPSHOT.jar
COPY --from=build /app/${JAR_FILE} app.jar

EXPOSE 8080
ENTRYPOINT ["java","-jar","/app.jar"]
