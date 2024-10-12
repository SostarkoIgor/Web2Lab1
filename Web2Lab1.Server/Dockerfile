#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#COPY ["web2lab1.client/nuget.config", "web2lab1.client/"]
RUN pwd
COPY ["Web2Lab1.Server.csproj", "Web2Lab1.Server/"]
#COPY ["web2lab1.client/web2lab1.client.esproj", "web2lab1.client/"]
RUN dotnet restore "./Web2Lab1.Server/Web2Lab1.Server.csproj"
COPY . .
WORKDIR "/src/Web2Lab1.Server"
RUN dotnet build "./Web2Lab1.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web2Lab1.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web2Lab1.Server.dll"]