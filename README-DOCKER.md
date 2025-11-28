# Docker Setup Guide

Denne guide beskriver, hvordan du kører The Way of Coherence applikationen lokalt med Docker.

## Forudsætninger

- Docker Desktop installeret og kørende
- Minimum 4GB RAM tildelt til Docker
- Port 8080 og 1433 skal være ledige

## Hurtig Start

1. **Klon repository og naviger til projektet**
   ```bash
   cd TheWayOfCoherence
   ```

2. **Opdater connection string i docker-compose.yml** (hvis nødvendig)
   - Standard password er: `YourStrong@Password123`
   - **VIGTIGT**: Skift password i produktion!

3. **Byg og start containere**
   ```bash
   docker-compose up --build
   ```

4. **Vent på at containere starter** (første gang kan tage 2-3 minutter)

5. **Kør database migrations**
   ```bash
   # I en ny terminal
   docker-compose exec webapp dotnet ef database update --project /app
   ```

6. **Åbn applikationen**
   - Web app: http://localhost:8080
   - Health check: http://localhost:8080/api/health

## Docker Commands

### Grundlæggende

```bash
# Start containere
docker-compose up

# Start i baggrunden
docker-compose up -d

# Stop containere
docker-compose down

# Stop og fjern volumes (sletter data!)
docker-compose down -v

# Genstart en specifik service
docker-compose restart webapp
```

### Logs

```bash
# Se alle logs
docker-compose logs

# Følg logs i real-time
docker-compose logs -f

# Se logs for specifik service
docker-compose logs -f webapp
```

### Database

```bash
# Kør migrations
docker-compose exec webapp dotnet ef database update --project /app

# Opret ny migration
docker-compose exec webapp dotnet ef migrations add MigrationName --project Infrastructure

# Connect til SQL Server
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Password123
```

### Debugging

```bash
# Åbn bash i webapp container
docker-compose exec webapp bash

# Se container status
docker-compose ps

# Inspect container
docker inspect coherence-webapp
```

## Konfiguration

### Environment Variables

Du kan tilpasse konfigurationen ved at redigere `docker-compose.yml`:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ConnectionStrings__DefaultConnection=...
  - AIService__BaseUrl=http://ai-service:8000
```

### Ports

Standard ports:
- **8080**: Web applikation
- **1433**: SQL Server

For at ændre ports, rediger `docker-compose.yml`:

```yaml
ports:
  - "8080:8080"  # Format: "host:container"
```

## Troubleshooting

### Container starter ikke

1. **Tjek logs**
   ```bash
   docker-compose logs webapp
   ```

2. **Tjek om port er i brug**
   ```bash
   # Windows
   netstat -ano | findstr :8080
   
   # Linux/Mac
   lsof -i :8080
   ```

3. **Prøv at genbygge**
   ```bash
   docker-compose down
   docker-compose build --no-cache
   docker-compose up
   ```

### Database connection fejl

1. **Tjek om SQL Server container er healthy**
   ```bash
   docker-compose ps
   ```

2. **Vent på at SQL Server er klar** (kan tage 30-60 sekunder)

3. **Tjek connection string** i `docker-compose.yml`

### Out of Memory

1. **Tildel mere RAM til Docker**
   - Docker Desktop → Settings → Resources → Memory
   - Minimum 4GB, anbefalet 8GB

2. **Stop andre containere**
   ```bash
   docker stop $(docker ps -q)
   ```

## Production Considerations

⚠️ **Dette setup er til udvikling/test. For produktion:**

1. **Skift alle passwords**
   - SQL Server password
   - App secrets

2. **Brug environment variables**
   - Brug `.env` fil eller Azure Key Vault

3. **Enable HTTPS**
   - Tilføj reverse proxy (nginx/traefik)
   - Konfigurer SSL certifikater

4. **Backup strategi**
   - Konfigurer automatiske backups
   - Test restore procedure

5. **Monitoring**
   - Tilføj logging og monitoring
   - Sæt op alerts

## Cleanup

```bash
# Fjern alle containere, networks og volumes
docker-compose down -v

# Fjern alle Docker images for projektet
docker rmi $(docker images | grep coherence | awk '{print $3}')

# Fjern alle stoppede containere
docker container prune
```

## Yderligere Ressourcer

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [ASP.NET Core Docker Documentation](https://docs.microsoft.com/aspnet/core/host-and-deploy/docker/)

