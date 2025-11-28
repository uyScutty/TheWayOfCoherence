# Korrekt Package Manager Console Kommando

## Problem: Projekt navn er forkert

Projektnavnet er `TheWayOfCoherenceWeb`, ikke `TheWayOfCoherence`.

## Korrekt kommando:

```powershell
Update-Database -Project Infrastructure -StartupProject TheWayOfCoherenceWeb -Context AppDbContext
```

Eller hvis Default Project er sat til Infrastructure:

```powershell
Update-Database -StartupProject TheWayOfCoherenceWeb
```

## Alternativ: Brug projekt path

Hvis det stadig ikke virker, brug fuld path:

```powershell
Update-Database -Project Infrastructure -StartupProject TheWayOfCoherence\TheWayOfCoherenceWeb.csproj -Context AppDbContext
```

## Tjek projekt navne

For at se alle projekter i solution:

```powershell
Get-Project
```

Eller se solution structure:
```powershell
Get-Project -All | Select-Object Name
```

