# Deploy Applikationen til Azure App Service

Nu hvor infrastructure er oprettet, skal vi deploye selve applikationen.

## Step 1: Find App Service Navn

```powershell
# Se alle App Services i resource group
az webapp list --resource-group WayOfCoherence --query "[].{Name:name, URL:defaultHostName}" --output table
```

Eller se i Azure Portal:
- Gå til Resource Group "WayOfCoherence"
- Find App Service (fx: `app-coherence-xxxxx`)

## Step 2: Build Applikationen

```powershell
# Naviger til projekt root (hvis ikke allerede der)
cd C:\Users\sero-\source\repos\TheWayOfCoherence

# Build for Release
dotnet publish TheWayOfCoherence/TheWayOfCoherenceWeb.csproj -c Release -o ./publish
```

Dette opretter en `publish` mappe med alle filer klar til deployment.

## Step 3: Deploy til Azure App Service

```powershell
# Kør på ÉN linje (ikke med backslashes!)
az webapp deploy --resource-group WayOfCoherence --name app-coherence-ft6g36b3e77c2 --src-path ./publish --type zip
```

Dette uploader og deployer applikationen til Azure.

## Step 4: Konfigurer Connection String

```powershell
# Find SQL Server navn først
az sql server list --resource-group WayOfCoherence --query "[].{Name:name, FQDN:fullyQualifiedDomainName}" --output table

# Sæt connection string (erstatt <app-service-name>, <sql-server-name>, og <password>)
az webapp config connection-string set \
  --resource-group WayOfCoherence \
  --name <app-service-name> \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

**Husk**: Brug det password du satte i `azure-deploy.parameters.json` (fx: `MyStr0ng!P@ssw0rd123`)

## Step 5: Kør Migrations

```powershell
# Kør migrations mod Azure SQL Database
# Erstatt <sql-server-name> og <password> med de rigtige værdier
dotnet ef database update \
  --project Infrastructure \
  --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj \
  --connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Step 6: Test Deployment

```powershell
# Hent app URL
az webapp show \
  --resource-group WayOfCoherence \
  --name <app-service-name> \
  --query defaultHostName \
  --output tsv
```

Åbn URL'en i browser (fx: `https://app-coherence-xxxxx.azurewebsites.net`)

## Step 7: Tjek Logs (Hvis Der Er Problemer)

```powershell
# Se real-time logs
az webapp log tail \
  --resource-group WayOfCoherence \
  --name <app-service-name>

# Eller se i Azure Portal:
# App Service → Log stream
```

## Troubleshooting

### Fejl: "Cannot connect to database"
- Tjek SQL Server firewall: Azure Portal → SQL Server → Networking
- Sørg for "Allow Azure services" er aktiveret

### Fejl: "App not starting"
- Tjek logs: `az webapp log tail`
- Verificer connection string er sat korrekt

### Fejl: "Migrations failed"
- Tjek connection string format
- Verificer SQL Server firewall tillader din IP

