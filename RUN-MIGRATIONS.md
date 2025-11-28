# Kør Migrations - Løsning

Problemet er at Program.cs prøver at oprette roller før migrations er kørt. Her er løsningen:

## Løsning 1: Kør migrations direkte (Anbefalet)

```bash
# Start webapp container (uden at køre applikationen)
docker-compose up -d webapp

# Kør migrations direkte med connection string
docker-compose exec webapp dotnet ef database update \
  --project /app/Infrastructure \
  --startup-project /app/TheWayOfCoherenceWeb.csproj \
  --context AppDbContext \
  --connection "Server=sqlserver,1433;Database=TheWayOfCoherenceDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

## Løsning 2: Kør migrations fra Infrastructure projektet

```bash
# Start webapp
docker-compose up -d webapp

# Naviger til Infrastructure i containeren og kør migrations
docker-compose exec webapp bash -c "cd /app && dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherenceWeb.csproj"
```

## Løsning 3: Brug environment variable

```bash
# Start webapp
docker-compose up -d webapp

# Sæt connection string som environment variable og kør migrations
docker-compose exec -e ConnectionStrings__DefaultConnection="Server=sqlserver,1433;Database=TheWayOfCoherenceDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=True" webapp dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherenceWeb.csproj
```

## Løsning 4: Kør SQL script direkte (Hvis migrations stadig fejler)

Hvis migrations stadig fejler, kan vi køre SQL scriptet direkte:

```bash
# Find migrations SQL fil
docker-compose exec webapp find /app -name "*.sql" -o -name "*InitialCreate*"

# Eller kør SQL direkte fra migration filen
# Først, se migrations:
docker-compose exec webapp dotnet ef migrations list --project Infrastructure --startup-project TheWayOfCoherenceWeb.csproj

# Generer SQL script:
docker-compose exec webapp dotnet ef migrations script --project Infrastructure --startup-project TheWayOfCoherenceWeb.csproj --output /tmp/migration.sql

# Kør SQL script:
docker-compose exec webapp cat /tmp/migration.sql | docker exec -i coherence-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -d TheWayOfCoherenceDb
```

## Efter migrations er kørt

Når migrations er kørt succesfuldt:

```bash
# Genstart webapp
docker-compose restart webapp

# Tjek logs
docker-compose logs -f webapp

# Test applikationen
curl http://localhost:8080/api/health
```

## Hvis intet virker

Prøv at køre migrations lokalt først (uden Docker) for at se om problemet er med Docker eller migrations:

```bash
# Lokalt (uden Docker)
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj
```

