# Tilføj Connection String Efter Publish

Du har allerede oprettet publish profilen. Nu skal du bare tilføje connection string.

## Metode 1: Via Visual Studio Publish Dialog (Nemmes)

1. **Højreklik på `TheWayOfCoherence` projekt** → **"Publish"**

2. **Klik "Edit"** (eller "Configure" ved siden af App Service navnet)

3. **Gå til "Settings" tab**

4. **Under "Connection strings", klik "Add"**

5. **Tilføj connection string:**
   - **Name**: `DefaultConnection`
   - **Value**: `Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Type**: SQLAzure
   
   **Husk**: Erstatt `<sql-server-name>` med dit SQL Server navn

6. **Klik "Save"**

7. **Klik "Publish" igen** (dette vil deploye med den nye connection string)

## Metode 2: Via Azure Portal

1. **Gå til Azure Portal** → Resource Group "WayOfCoherence"

2. **Klik på din App Service** (`app-coherence-ft6g36b3e77c2`)

3. **I venstre menu, vælg: "Configuration"**

4. **Under "Connection strings", klik "+ New connection string"**

5. **Tilføj:**
   - **Name**: `DefaultConnection`
   - **Value**: `Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Type**: SQLAzure

6. **Klik "OK"**

7. **Klik "Save"** (øverst)

8. **App Service vil genstarte automatisk**

## Find SQL Server Navn

Hvis du ikke kender SQL Server navnet:

```powershell
az sql server list --resource-group WayOfCoherence --query "[].{Name:name, FQDN:fullyQualifiedDomainName}" --output table
```

Eller se i Azure Portal:
- Resource Group "WayOfCoherence"
- Find SQL Server (fx: `sql-coherence-xxxxx`)

## Efter Connection String er Sat

1. **Kør migrations** (hvis ikke allerede kørt):
   ```powershell
   dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj --connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   ```

2. **Test applikationen:**
   - Gå til: `https://app-coherence-ft6g36b3e77c2.azurewebsites.net`

