# Azure Deployment Guide - Step by Step

Denne guide tager dig gennem hele deployment processen til Azure.

## Forudsætninger

- ✅ Azure for Students konto
- Azure CLI installeret (eller installer det nu)

## Step 1: Installer Azure CLI (hvis ikke allerede installeret)

```powershell
# Download og installer fra:
# https://aka.ms/installazurecliwindows

# Eller via winget:
winget install -e --id Microsoft.AzureCLI
```

Efter installation, genstart PowerShell.

## Step 2: Login til Azure

```powershell
az login
```

Dette åbner en browser - login med din Azure for Students konto.

## Step 3: Vælg Subscription

```powershell
# Se alle subscriptions
az account list --output table

# Sæt den rigtige subscription (hvis du har flere)
az account set --subscription "Azure for Students"
```

## Step 4: Opret Resource Group

```powershell
# Opret resource group i West Europe (eller vælg tættest på dig)
az group create --name rg-coherence-prod --location westeurope

# Eller North Europe:
az group create --name rg-coherence-prod --location northeurope
```

## Step 5: Opdater Parameters

Rediger `azure-deploy.parameters.json` og sæt et stærkt password:

```json
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "sqlAdminLogin": {
      "value": "coherenceadmin"
    },
    "sqlAdminPassword": {
      "value": "DitStærkePassword123!"  // <-- Skift dette!
    },
    "appServicePlanSku": {
      "value": "B1"  // Basic tier - gratis for students
    },
    "sqlDatabaseSku": {
      "value": "Basic"  // Basic tier
    }
  }
}
```

## Step 6: Deploy Infrastructure

```powershell
# Deploy med Bicep template
az deployment group create \
  --resource-group rg-coherence-prod \
  --template-file azure-deploy.bicep \
  --parameters azure-deploy.parameters.json
```

Dette tager 5-10 minutter. Vent til det er færdigt.

## Step 7: Hent Output (App Service Name)

Efter deployment, find app service navnet:

```powershell
# Se output fra deployment
az deployment group show \
  --resource-group rg-coherence-prod \
  --name azure-deploy \
  --query properties.outputs
```

Eller se i Azure Portal → Resource Groups → rg-coherence-prod

## Step 8: Build Applikationen

```powershell
# Naviger til projekt root
cd C:\Users\sero-\source\repos\TheWayOfCoherence

# Build for Release
dotnet publish TheWayOfCoherence/TheWayOfCoherenceWeb.csproj -c Release -o ./publish
```

## Step 9: Deploy Applikationen

```powershell
# Deploy til Azure App Service (erstatt <app-service-name> med det rigtige navn)
az webapp deploy \
  --resource-group rg-coherence-prod \
  --name <app-service-name> \
  --src-path ./publish \
  --type zip
```

## Step 10: Konfigurer Connection String

```powershell
# Hent SQL Server navn fra output
$sqlServerName = "sql-coherence-xxxxx"  # Fra deployment output

# Sæt connection string
az webapp config connection-string set \
  --resource-group rg-coherence-prod \
  --name <app-service-name> \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="Server=tcp:$sqlServerName.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=DitStærkePassword123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Step 11: Kør Migrations

```powershell
# Kør migrations mod Azure SQL Database
dotnet ef database update \
  --project Infrastructure \
  --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj \
  --connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=DitStærkePassword123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Step 12: Test Deployment

```powershell
# Hent app URL
az webapp show \
  --resource-group rg-coherence-prod \
  --name <app-service-name> \
  --query defaultHostName \
  --output tsv
```

Åbn URL'en i browser (fx: `https://app-coherence-xxxxx.azurewebsites.net`)

## Troubleshooting

### Fejl: "Subscription not found"
- Tjek at du er logget ind: `az account show`
- Sæt korrekt subscription: `az account set --subscription "Azure for Students"`

### Fejl: "Quota exceeded"
- Azure for Students har begrænsninger
- Prøv at bruge Basic tier (B1) i stedet for Standard

### Fejl: "Database connection failed"
- Tjek firewall rules i SQL Server
- Tilføj din IP: Azure Portal → SQL Server → Networking → Add your IP

### Fejl: "Migrations failed"
- Tjek connection string
- Verificer at SQL Server firewall tillader Azure services

## Vigtige Noter

⚠️ **Kost**: Azure for Students har $100 credit - Basic tier koster ca. $13/måned
⚠️ **HTTPS**: Azure App Service inkluderer automatisk HTTPS
⚠️ **Backup**: Konfigurer automatiske backups for SQL Database
⚠️ **Monitoring**: Overvej at tilføje Application Insights

## Næste Skridt Efter Deployment

1. Konfigurer custom domain (hvis du har et)
2. Tilføj Application Insights for monitoring
3. Konfigurer automatiske backups
4. Sæt op CI/CD pipeline (GitHub Actions)

