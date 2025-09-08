# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore (better caching)
COPY dotnet-app-5/*.csproj ./dotnet-app-5/
RUN dotnet restore "dotnet-app-5/dotnet-app-5.csproj"

# Copy the rest and build
COPY dotnet-app-5/ ./dotnet-app-5/
WORKDIR /src/dotnet-app-5
RUN dotnet build "dotnet-app-5.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "dotnet-app-5.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "dotnet-app-5.dll"]
