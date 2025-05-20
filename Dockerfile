# Dockerfile.dev

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY API/appsettings*.json API/
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN dotnet restore "API/API.csproj"

COPY . .

EXPOSE 5001

WORKDIR /app/API
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out ./
COPY wait-for.sh /wait-for.sh
RUN chmod +x /wait-for.sh

ENTRYPOINT ["/wait-for.sh", "db1:3306", "db2:3306", "--", "dotnet", "API.dll"]


