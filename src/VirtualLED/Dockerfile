# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the .csproj first for restore
COPY VirtualLED/VirtualLED.csproj ./VirtualLED/
RUN dotnet restore ./VirtualLED/VirtualLED.csproj

# Copy everything else
COPY . .

# Build the project
WORKDIR /src/VirtualLED
RUN dotnet build VirtualLED.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish VirtualLED.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VirtualLED.dll"]