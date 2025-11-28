# Deploy fra Visual Studio - Step by Step

## Step 1: Åbn Publish Dialog

1. **Højreklik på `TheWayOfCoherence` projekt** i Solution Explorer
2. **Vælg "Publish"**

## Step 2: Vælg Azure App Service

1. I Publish dialog, vælg: **"Azure"**
2. Klik **"Next"**
3. Vælg: **"Azure App Service (Windows)"** eller **"Azure App Service (Linux)"**
   - Hvis du har oprettet Linux App Service, vælg Linux
   - Hvis Windows, vælg Windows
4. Klik **"Next"**

## Step 3: Vælg Din App Service

1. **Login til Azure** (hvis ikke allerede)
   - Klik "Sign in" og login med din Azure for Students konto

2. **Vælg din App Service:**
   - Under "App Service instances", find: `app-coherence-ft6g36b3e77c2`
   - Eller søg efter "coherence" eller "WayOfCoherence"
   - Klik på den

3. Klik **"Finish"**

## Step 4: Konfigurer Connection String

Før du publiserer, skal du konfigurere connection string:

1. **I Publish dialog, klik "Edit"** (eller "Configure" ved siden af App Service navnet)

2. **Gå til "Settings" tab**

3. **Under "Connection strings", klik "Add"**

4. **Tilføj connection string:**
   - **Name**: `DefaultConnection`
   - **Value**: `Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Type**: SQLAzure
   
   **Husk**: Erstatt `<sql-server-name>` med dit SQL Server navn (fx: `sql-coherence-xxxxx`)

5. **Klik "Save"**

## Step 5: Publish!

1. **Klik "Publish"** knappen
2. Vent på at deployment er færdig (tager 2-5 minutter)
3. Visual Studio åbner automatisk applikationen i browser når det er færdigt

## Step 6: Kør Migrations

Efter deployment, skal du køre migrations:

### Metode A: Fra Package Manager Console

```powershell
# I Package Manager Console
Update-Database -Project Infrastructure -StartupProject TheWayOfCoherenceWeb -Connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

### Metode B: Fra PowerShell

```powershell
dotnet ef database update --project Infrastructure --startup-project TheWayOfCoherence/TheWayOfCoherenceWeb.csproj --connection "Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## Step 7: Test Applikationen

1. **Applikationen åbnes automatisk** efter deployment
2. Eller gå til: `https://app-coherence-ft6g36b3e77c2.azurewebsites.net`
3. Test at siden virker!

## Troubleshooting

### Fejl: "Cannot find App Service"
- Tjek at du er logget ind med korrekt Azure konto
- Tjek at App Service findes i Resource Group "WayOfCoherence"

### Fejl: "Deployment failed"
- Tjek logs i Visual Studio Output window
- Tjek Azure Portal → App Service → Deployment Center → Logs

### Fejl: "Database connection failed"
- Verificer connection string er sat korrekt
- Tjek SQL Server firewall: Azure Portal → SQL Server → Networking
- Sørg for "Allow Azure services" er aktiveret

## Tips

- ✅ Visual Studio gemmer publish profil, så næste gang er det bare "Publish" knappen
- ✅ Du kan se deployment status i Visual Studio Output window
- ✅ Efter første deployment, kan du redeploy med F5 (hvis konfigureret)

