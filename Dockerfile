# Stage 1: Build the application
FROM bellsoft/liberica-openjdk-debian:21 AS build_stage

# Add the necessary extra packages
RUN apt-get update \
    && apt-get install -y maven \
    && rm -rf /var/lib/apt/lists/*

# Copy the source code and Maven resources
COPY ./MainBackendServer /build/MainBackendServer
COPY ./.m2 /build/.m2
RUN mkdir -p /build/.mvn && echo "-Dmaven.repo.local=/build/.m2/repository" > /build/.mvn/maven.config

# Build the application
RUN ls -al /build && cd /build/MainBackendServer && mvn -f pom.xml -Dmaven.repo.local=/build/.m2/repository clean package -DskipTests

# Stage 2: Create the runtime image
FROM bellsoft/liberica-openjdk-debian:21

WORKDIR /app

# Copy the JAR from builder stage
COPY --from=build_stage /build/MainBackendServer/target/MainBackendServer-1.0-SNAPSHOT.jar app.jar

# Expose the port
EXPOSE 7891

# Run the application
ENTRYPOINT ["java", "-jar", "/app/app.jar"]
