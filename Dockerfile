#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG MYSQL_USER
ARG MYSQL_PASSWORD
ARG DB_IP

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Thesis_backend.csproj", "."]
RUN dotnet restore "./Thesis_backend.csproj"
COPY . .
RUN sed -ir "s/Server=[^;]*;/Server=$DB_IP;/g" ./appsettings.json
RUN sed -ir "s/User=[^;]*;/User=$MYSQL_USER;/g" ./appsettings.json
RUN sed -ir "s/Password=[^;]*;/Password=$MYSQL_PASSWORD;/g" ./appsettings.json


WORKDIR "/src/."
RUN dotnet build "./Thesis_backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Thesis_backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Thesis_backend.dll"]