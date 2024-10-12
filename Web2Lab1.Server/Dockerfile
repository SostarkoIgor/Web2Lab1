# Use the official ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official build image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiranje nuget config i csproj datoteka
#COPY ["web2lab1.client/nuget.config", "web2lab1.client/"]
COPY ["Web2Lab1.Server/Web2Lab1.Server.csproj", "Web2Lab1.Server/"]
#COPY ["web2lab1.client/web2lab1.client.csproj", "web2lab1.client/"]  # Pravi naziv csproj datoteke

# Restore dependencies
RUN dotnet restore "./Web2Lab1.Server/Web2Lab1.Server.csproj"

# Kopiranje svih drugih datoteka
COPY . .

# Build the application
WORKDIR "/src/Web2Lab1.Server"
RUN dotnet build "./Web2Lab1.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web2Lab1.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Use the base image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web2Lab1.Server.dll"]
