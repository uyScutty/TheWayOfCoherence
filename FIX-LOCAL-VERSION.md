# Fix Lokal Version

Hvis Docker versionen virker men den lokale version ikke, er det sandsynligvis et database problem.

## Problem: Lokal database mangler migrations

Den lokale database (localhost\SQLEXPRESS01) har sandsynligvis ikke de samme migrations som Docker databasen.

## Løsning 1: Kør migrations lokalt

```bash
# Naviger til projekt root
cd TheWayOfCoherence

# Kør migrations mod lokal database
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj
```

## Løsning 2: Tjek connection string

Tjek om din lokale SQL Server kører og connection string er korrekt:

```bash
# Test connection til lokal SQL Server
sqlcmd -S localhost\SQLEXPRESS01 -E -Q "SELECT @@VERSION"
```

Hvis det ikke virker, prøv:

```bash
# Alternativ connection string format
sqlcmd -S "localhost\SQLEXPRESS01" -E -Q "SELECT @@VERSION"
```

## Løsning 3: Opret database lokalt

Hvis databasen ikke eksisterer:

```bash
# Connect til SQL Server
sqlcmd -S localhost\SQLEXPRESS01 -E

# I SQL prompt, kør:
CREATE DATABASE TheWayOfCoherenceDb;
GO
EXIT
```

Derefter kør migrations:

```bash
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj
```

## Løsning 4: Tjek om SQL Server kører

```powershell
# I PowerShell
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Eller tjek i Services (services.msc)
# Søg efter "SQL Server (SQLEXPRESS01)"
```

## Løsning 5: Opdater connection string

Hvis din SQL Server instance har et andet navn, opdater `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=TheWayOfCoherenceDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

Prøv at ændre `SQLEXPRESS01` til:
- `SQLEXPRESS` (standard navn)
- `MSSQLSERVER` (default instance)
- Eller dit specifikke instance navn

## Debug: Se fejlbeskeder

Kør applikationen lokalt og se fejlbeskederne:

```bash
# I Visual Studio: Start med Debug (F5)
# Eller fra command line:
cd TheWayOfCoherence
dotnet run
```

Tjek output for database connection fejl.

## Hvis intet virker

1. **Tjek om Docker SQL Server kører på port 1433** (kan konflikte med lokal SQL Server)
   ```bash
   docker ps | grep coherence-db
   ```
   Hvis den kører, stop den:
   ```bash
   docker-compose stop sqlserver
   ```

2. **Brug Docker SQL Server også lokalt**
   - Start Docker SQL Server
   - Opdater connection string i `appsettings.json`:
   ```json
   "DefaultConnection": "Server=localhost,1433;Database=TheWayOfCoherenceDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=True"
   ```

