FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ServiceManagerRestAPI/ServiceManagerRestAPI.csproj", "ServiceManagerRestAPI/"]
RUN dotnet restore "./ServiceManagerRestAPI/ServiceManagerRestAPI.csproj"
COPY . .
WORKDIR "/src/ServiceManagerRestAPI"
RUN dotnet build "./ServiceManagerRestAPI.csproj" -c Debug -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
RUN dotnet publish "./ServiceManagerRestAPI.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceManagerRestAPI.dll"]