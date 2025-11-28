# Kør Migrations Lokalt

## Metode 1: Package Manager Console (Visual Studio) - Anbefalet

1. **Åbn Package Manager Console i Visual Studio**
   - View → Other Windows → Package Manager Console

2. **Sæt Default Project til Infrastructure**
   - I dropdown øverst i Package Manager Console, vælg: `Infrastructure`

3. **Kør migrations kommando:**
   ```powershell
   Update-Database -StartupProject TheWayOfCoherence
   ```

   Eller hvis det ikke virker:
   ```powershell
   Update-Database -Project Infrastructure -StartupProject TheWayOfCoherence
   ```

## Metode 2: dotnet CLI (Command Line)

1. **Åbn PowerShell eller Command Prompt**
   - Naviger til projekt root:
   ```powershell
   cd C:\Users\sero-\source\repos\TheWayOfCoherence
   ```

2. **Kør migrations:**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj
   ```

## Hvis "dotnet ef" ikke virker

Hvis du får fejl om at `dotnet ef` ikke er installeret:

```bash
# Installer Entity Framework Tools globalt
dotnet tool install --global dotnet-ef

# Hvis det allerede er installeret, opdater det:
dotnet tool update --global dotnet-ef
```

## Tjek om det virker

Efter migrations er kørt, kan du:

1. **Tjek database:**
   ```sql
   -- I SQL Server Management Studio eller via sqlcmd
   USE TheWayOfCoherenceDb;
   SELECT name FROM sys.tables;
   ```

2. **Start applikationen lokalt:**
   - Tryk F5 i Visual Studio
   - Eller: `dotnet run` i TheWayOfCoherence mappen

## Troubleshooting

### Fejl: "No DbContext was found"

Løsning: Sørg for at du kører kommandoen fra projekt root, og at Infrastructure projektet har reference til Domain.

### Fejl: "Cannot connect to database"

Løsning: 
1. Tjek om SQL Server kører lokalt
2. Tjek connection string i `appsettings.json`
3. Prøv at connect manuelt:
   ```bash
   sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -C
   ```

### Fejl: "Database does not exist"

Løsning: Opret database først:
```sql
CREATE DATABASE TheWayOfCoherenceDb;
```

