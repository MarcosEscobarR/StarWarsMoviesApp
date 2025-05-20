# Dockerfile

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos de proyecto
COPY API/appsettings*.json API/
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restaurar dependencias
RUN dotnet restore "API/API.csproj"

# Copiar el resto de la solución
COPY . .

EXPOSE 5001

# Publicar la aplicación
WORKDIR /app/API
RUN dotnet publish -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Instalar netcat para wait-for.sh
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out ./
COPY wait-for.sh /wait-for.sh
RUN chmod +x /wait-for.sh

ENTRYPOINT ["/wait-for.sh", "db1:3306", "db2:3306", "--", "dotnet", "API.dll"]


