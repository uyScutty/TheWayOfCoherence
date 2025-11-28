# Quick Deploy Commands - Kør Disse Nu

## Step 1: Build Applikationen

```powershell
cd C:\Users\sero-\source\repos\TheWayOfCoherence
dotnet publish TheWayOfCoherence/TheWayOfCoherenceWeb.csproj -c Release -o ./publish
```

## Step 2: Deploy til Azure (PÅ ÉN LINJE!)

```powershell
az webapp deploy --resource-group WayOfCoherence --name app-coherence-ft6g36b3e77c2 --src-path ./publish --type zip
```

## Step 3: Find SQL Server Navn

```powershell
az sql server list --resource-group WayOfCoherence --query "[].{Name:name, FQDN:fullyQualifiedDomainName}" --output table
```

## Step 4: Konfigurer Connection String (PÅ ÉN LINJE!)

```powershell
az webapp config connection-string set --resource-group WayOfCoherence --name app-coherence-ft6g36b3e77c2 --connection-string-type SQLAzure --settings DefaultConnection="Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

**Husk**: Erstatt `<sql-server-name>` med det navn du fandt i Step 3!

## Step 5: Kør Migrations (PÅ ÉN LINJE!)

```powershell
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj --connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

**Husk**: Erstatt `<sql-server-name>` med det navn du fandt i Step 3!

## Step 6: Test Applikationen

```powershell
az webapp show --resource-group WayOfCoherence --name app-coherence-ft6g36b3e77c2 --query defaultHostName --output tsv
```

Åbn URL'en i browser!

