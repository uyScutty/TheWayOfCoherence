# Næste Steps - SQL Server er klar! ✅

Nu hvor SQL Server virker, skal vi:
1. Oprette databasen
2. Køre migrations
3. Starte webapp

## Step 1: Opret database (hvis den ikke eksisterer)

```bash
docker exec -it coherence-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TheWayOfCoherenceDb') CREATE DATABASE TheWayOfCoherenceDb;"
```

## Step 2: Kør migrations

```bash
docker-compose run --rm webapp dotnet ef database update --project /app
```

Hvis det fejler, prøv:

```bash
docker-compose exec webapp dotnet ef database update --project /app
```

## Step 3: Start webapp

```bash
docker-compose up -d webapp
```

## Step 4: Tjek logs

```bash
docker-compose logs -f webapp
```

## Step 5: Test applikationen

Åbn i browser:
- Web app: http://localhost:8080
- Health check: http://localhost:8080/api/health

## Hvis migrations fejler

Hvis Step 2 fejler, kan det være fordi webapp containeren ikke er bygget endnu:

```bash
# Byg webapp først
docker-compose build webapp

# Kør migrations
docker-compose run --rm webapp dotnet ef database update --project /app
```

