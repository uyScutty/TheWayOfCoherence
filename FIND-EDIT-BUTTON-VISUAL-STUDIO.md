# Find Edit Knappen i Visual Studio Publish

## Hvor er Edit Knappen?

Når du har åbnet Publish dialog (højreklik på projekt → Publish), kan "Edit" knappen være på forskellige steder:

### Sted 1: Ved Siden af App Service Navnet
- I Publish dialog, se øverst hvor App Service navnet vises
- Der skulle være en **"Edit"** eller **"Configure"** knap ved siden af navnet
- Eller en **"..."** (tre prikker) menu

### Sted 2: I Toolbar
- Se i toolbar i Publish dialog
- Der kan være en **"Settings"** eller **"Configure"** knap

### Sted 3: Højreklik Menu
- Højreklik på publish profilen i Solution Explorer
- Under "Solution Items" eller projektet, kan der være en publish profil
- Højreklik på den → **"Edit"**

## Alternativ: Tilføj Connection String Først, Deploy Bagefter

Hvis du ikke kan finde Edit knappen, kan du:

### Option A: Tilføj via Azure Portal (Nemmes)

1. **Gå til Azure Portal**
2. **Resource Group "WayOfCoherence"**
3. **Klik på App Service** (`app-coherence-ft6g36b3e77c2`)
4. **Configuration** → **Connection strings**
5. **+ New connection string**
6. **Tilføj connection string**
7. **Save**

Derefter kan du deploye fra Visual Studio - connection string er allerede sat!

### Option B: Find Publish Profil Fil

1. **I Solution Explorer**, se efter en mappe eller fil med navn som:
   - `Properties\PublishProfiles\`
   - Eller en `.pubxml` fil

2. **Højreklik på den** → **"Edit"**

## Step-by-Step: Tilføj Connection String i Azure Portal

1. **Åbn Azure Portal** (portal.azure.com)
2. **Søg efter "WayOfCoherence"** eller gå til Resource Groups
3. **Klik på Resource Group "WayOfCoherence"**
4. **Klik på App Service** (navnet starter med `app-coherence-`)
5. **I venstre menu, scroll ned og find "Configuration"**
6. **Klik på "Configuration"**
7. **Under "Connection strings" tab, klik "+ New connection string"**
8. **Udfyld:**
   - **Name**: `DefaultConnection`
   - **Value**: `Server=tcp:<sql-server-name>.database.windows.net,1433;Database=TheWayOfCoherenceDb;User ID=coherenceadmin;Password=MyStr0ng!P@ssw0rd123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Type**: Dropdown → Vælg `SQLAzure`
9. **Klik "OK"**
10. **Klik "Save"** (øverst i blå bjælke)
11. **Bekræft** når den spørger om genstart

## Find SQL Server Navn

Hvis du ikke kender SQL Server navnet:

```powershell
az sql server list --resource-group WayOfCoherence --query "[].name" --output tsv
```

Eller i Azure Portal:
- Resource Group "WayOfCoherence"
- Find SQL Server (fx: `sql-coherence-xxxxx`)

## Efter Connection String er Sat

1. **Gå tilbage til Visual Studio**
2. **Klik "Publish" knappen** i Publish dialog
3. **Vent på deployment**
4. **Kør migrations** (hvis nødvendigt)

