# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["TheWayOfCoherence.sln", "."]
COPY ["TheWayOfCoherence/TheWayOfCoherenceWeb.csproj", "TheWayOfCoherence/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "TheWayOfCoherence/TheWayOfCoherenceWeb.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/TheWayOfCoherence"
RUN dotnet build "TheWayOfCoherenceWeb.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TheWayOfCoherenceWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published files
COPY --from=publish /app/publish .

# Set ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port (default for ASP.NET Core)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl --fail http://localhost:8080/api/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "TheWayOfCoherenceWeb.dll"]

