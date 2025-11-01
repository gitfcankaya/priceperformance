# Build stage for backend
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-backend
WORKDIR /src

# Copy solution and project files
COPY ["PricePerformance.sln", "./"]
COPY ["src/PricePerformance.API/PricePerformance.API.csproj", "src/PricePerformance.API/"]
COPY ["src/PricePerformance.Core/PricePerformance.Core.csproj", "src/PricePerformance.Core/"]
COPY ["src/PricePerformance.Infrastructure/PricePerformance.Infrastructure.csproj", "src/PricePerformance.Infrastructure/"]

# Restore dependencies
RUN dotnet restore

# Copy remaining files and build
COPY . .
WORKDIR "/src/src/PricePerformance.API"
RUN dotnet build "PricePerformance.API.csproj" -c Release -o /app/build

# Publish stage
FROM build-backend AS publish
RUN dotnet publish "PricePerformance.API.csproj" -c Release -o /app/publish

# Build stage for frontend
FROM node:20-alpine AS build-frontend
WORKDIR /app

# Copy package files
COPY client/package*.json ./
RUN npm install

# Copy remaining files and build
COPY client/ ./
RUN npm run build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy backend
COPY --from=publish /app/publish .

# Copy frontend build to wwwroot
COPY --from=build-frontend /app/dist ./wwwroot

ENTRYPOINT ["dotnet", "PricePerformance.API.dll"]
