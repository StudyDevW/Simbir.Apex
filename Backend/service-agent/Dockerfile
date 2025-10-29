FROM maven:3.9-eclipse-temurin-23 AS builder

WORKDIR /app

COPY pom.xml .

RUN mvn -B dependency:go-offline

COPY src ./src

RUN mvn -B clean package -DskipTests



FROM eclipse-temurin:23-jre AS runtime

WORKDIR /app

COPY --from=builder /app/target/*-jar-with-dependencies.jar app.jar

ENTRYPOINT ["java", "-jar", "app.jar"]
