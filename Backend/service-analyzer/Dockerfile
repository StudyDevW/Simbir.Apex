FROM maven:3.9.6-eclipse-temurin-21 AS build

WORKDIR /app

# Копируем только pom.xml и тянем зависимости заранее (кэшируется)
COPY pom.xml .
RUN mvn dependency:go-offline

# Копируем исходники и собираем jar
COPY src ./src
RUN mvn clean package -DskipTests

# Финальный образ для запуска
FROM eclipse-temurin:21-jre-alpine

WORKDIR /app
COPY --from=build /app/target/*.jar app.jar

EXPOSE 8080
ENTRYPOINT ["java", "-jar", "/app/app.jar"]
