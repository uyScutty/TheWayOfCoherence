# Korrigerede Steps - SQL Server Connection

## Problem: sqlcmd ikke fundet

SQL Server 2022 bruger en anden sti til sqlcmd. Prøv disse steps:

## Step 1: Stop alt
```bash
docker-compose down
```

## Step 2: Start SQL Server
```bash
docker-compose up -d sqlserver
```

## Step 3: Vent og tjek logs
```bash
# Følg logs - vent til du ser "SQL Server is now ready"
docker-compose logs -f sqlserver
```

Vent minimum 60-90 sekunder. Når du ser `SQL Server is now ready for client connections` - tryk Ctrl+C.

## Step 4: Test SQL Server (korrigeret kommando)

Prøv denne kommando i stedet:

```bash
docker exec coherence-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION"
```

Hvis det ikke virker, prøv:

```bash
# Alternativ 1
docker exec coherence-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION"

# Alternativ 2
docker exec coherence-db /usr/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION"
```

## Step 5: Hvis sqlcmd ikke virker - brug alternativ metode

Hvis ingen af kommandoerne virker, kan vi teste på en anden måde:

```bash
# Tjek om container kører
docker ps | grep coherence-db

# Tjek logs for fejl
docker logs coherence-db --tail 50

# Test om port 1433 er åben
docker exec coherence-db netstat -an | grep 1433
```

## Step 6: Opret database manuelt (hvis sqlcmd virker)

```bash
# Connect til SQL Server
docker exec -it coherence-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C

# I SQL prompt, kør:
CREATE DATABASE TheWayOfCoherenceDb;
GO
EXIT
```

## Step 7: Kør migrations

```bash
docker-compose run --rm webapp dotnet ef database update --project /app
```

## Step 8: Start webapp

```bash
docker-compose up -d webapp
docker-compose logs -f webapp
```

## Alternativ: Start uden health check

Hvis health check stadig giver problemer, kan du starte webapp uden dependency:

```bash
# Start SQL Server
docker-compose up -d sqlserver

# Vent 90 sekunder manuelt

# Start webapp uden dependency check
docker-compose up -d --no-deps webapp

# Kør migrations
docker-compose exec webapp dotnet ef database update --project /app
```

