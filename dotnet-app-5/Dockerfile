# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore
COPY ["dotnet-app-5/dotnet-app-5.csproj", "dotnet-app-5/"]
RUN dotnet restore "dotnet-app-5/dotnet-app-5.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/dotnet-app-5"
RUN dotnet build "dotnet-app-5.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "dotnet-app-5.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage (aspnet)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the app listens on all network interfaces
ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

# Optional non-root user
# RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

ENTRYPOINT ["dotnet", "dotnet-app-5.dll"]
