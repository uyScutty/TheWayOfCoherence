# Fix Entity Framework Tools Problem

## Problem: dotnet-ef tool installation fejler

Dette er et kendt problem med nogle versioner. Her er løsningerne:

## Løsning 1: Brug Package Manager Console direkte (Nemmere!)

Du er allerede i Package Manager Console - brug den direkte:

```powershell
# Sæt Default Project til Infrastructure (i dropdown øverst)
# Derefter kør:
Update-Database -StartupProject TheWayOfCoherence
```

Hvis det ikke virker, prøv:
```powershell
Update-Database -Project Infrastructure -StartupProject TheWayOfCoherence
```

## Løsning 2: Fix dotnet-ef installation

Hvis du stadig vil bruge dotnet CLI:

```powershell
# Fjern eksisterende installation først
dotnet tool uninstall --global dotnet-ef

# Ryd NuGet cache
dotnet nuget locals all --clear

# Installer igen
dotnet tool install --global dotnet-ef --version 8.0.0
```

## Løsning 3: Brug lokal installation i stedet for global

```powershell
# I projekt root
cd C:\Users\sero-\source\repos\TheWayOfCoherence

# Installer lokalt i projektet
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 8.0.0

# Derefter kør migrations
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj
```

## Løsning 4: Brug Visual Studio's indbyggede EF Tools

Visual Studio har allerede EF Tools indbygget - du behøver ikke dotnet-ef!

I Package Manager Console:
```powershell
# Sørg for at Default Project er sat til Infrastructure
Update-Database
```

Eller med fuld specifikation:
```powershell
Update-Database -Project Infrastructure -StartupProject TheWayOfCoherence -Context AppDbContext
```

