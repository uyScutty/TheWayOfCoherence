# Deployment Guide - The Way of Coherence

Denne guide beskriver, hvordan du deployer The Way of Coherence applikationen til produktion med fokus på sikkerhed, HTTPS og korrekt konfiguration.

## Indhold

1. [Lokalt Docker Setup](#lokalt-docker-setup)
2. [Azure Deployment](#azure-deployment)
3. [Security Konfiguration](#security-konfiguration)
4. [HTTPS og Certifikater](#https-og-certifikater)
5. [Database Migration](#database-migration)
6. [Troubleshooting](#troubleshooting)

---

## Lokalt Docker Setup

### Forudsætninger
- Docker Desktop installeret
- Minimum 4GB RAM tildelt til Docker

### Kør applikationen lokalt med Docker

1. **Klon repository og naviger til projektet**
   ```bash
   cd TheWayOfCoherence
   ```

2. **Opdater connection string i docker-compose.yml**
   ```yaml
   ConnectionStrings__DefaultConnection=Server=sqlserver;Database=TheWayOfCoherenceDb;User Id=sa;Password=YourStrong@Password123;...
   ```

3. **Byg og start containere**
   ```bash
   docker-compose up --build
   ```

4. **Kør database migrations**
   ```bash
   docker-compose exec webapp dotnet ef database update --project /app
   ```

5. **Åbn applikationen**
   - Web app: http://localhost:8080
   - SQL Server: localhost:1433

### Docker Commands

```bash
# Stop containere
docker-compose down

# Stop og fjern volumes (sletter data)
docker-compose down -v

# Se logs
docker-compose logs -f webapp

# Kør kommando i container
docker-compose exec webapp bash
```

---

## Azure Deployment

### Forudsætninger
- Azure subscription
- Azure CLI installeret
- Visual Studio Code med Azure Tools extension (valgfrit)

### Metode 1: Bicep Template (Infrastructure as Code)

1. **Login til Azure**
   ```bash
   az login
   az account set --subscription "Your Subscription Name"
   ```

2. **Opret Resource Group**
   ```bash
   az group create --name rg-coherence-prod --location westeurope
   ```

3. **Deploy med Bicep**
   ```bash
   az deployment group create \
     --resource-group rg-coherence-prod \
     --template-file azure-deploy.bicep \
     --parameters azure-deploy.parameters.json \
     --parameters sqlAdminLogin=coherenceadmin sqlAdminPassword="YourStrongPassword123!"
   ```

4. **Deploy applikationen**
   ```bash
   # Build applikationen
   dotnet publish -c Release -o ./publish
   
   # Deploy til Azure App Service
   az webapp deploy \
     --resource-group rg-coherence-prod \
     --name <app-service-name-from-output> \
     --src-path ./publish \
     --type zip
   ```

### Metode 2: Azure Portal (GUI)

1. **Opret App Service**
   - Gå til Azure Portal
   - Opret ny "Web App"
   - Vælg:
     - Runtime stack: .NET 9.0
     - Operating System: Linux
     - Plan: Basic B1 (minimum)

2. **Opret SQL Database**
   - Opret ny "SQL Database"
   - Vælg Basic tier
   - Opret ny SQL Server med stærkt password

3. **Konfigurer Connection String**
   - I App Service → Configuration → Connection strings
   - Tilføj: `DefaultConnection`
   - Værdi: `Server=tcp:<server>.database.windows.net,1433;Database=<db>;User ID=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`

4. **Deploy kode**
   - App Service → Deployment Center
   - Vælg GitHub eller Local Git
   - Følg instruktionerne

### Metode 3: GitHub Actions CI/CD

1. **Opret Azure Service Principal**
   ```bash
   az ad sp create-for-rbac --name "coherence-deploy" --role contributor \
     --scopes /subscriptions/{subscription-id}/resourceGroups/rg-coherence-prod \
     --sdk-auth
   ```

2. **Tilføj secrets til GitHub**
   - Gå til GitHub repository → Settings → Secrets
   - Tilføj:
     - `AZURE_CREDENTIALS`: Output fra step 1
     - `AZURE_RESOURCE_GROUP`: rg-coherence-prod
     - `SQL_CONNECTION_STRING`: Din SQL connection string

3. **Push til main branch**
   - GitHub Actions vil automatisk deploye

---

## Security Konfiguration

### Security Headers

Applikationen inkluderer følgende security headers (konfigureret i `Program.cs`):

- **X-Content-Type-Options**: `nosniff` - Forhindrer MIME-sniffing
- **X-Frame-Options**: `DENY` - Forhindrer clickjacking
- **X-XSS-Protection**: `1; mode=block` - XSS beskyttelse
- **Referrer-Policy**: `strict-origin-when-cross-origin`
- **Content-Security-Policy**: Konfigureret for produktion

### HTTPS

- **HSTS**: Aktiveret i produktion (HTTP Strict Transport Security)
- **HTTPS Redirect**: Automatisk redirect fra HTTP til HTTPS
- **TLS Minimum**: 1.2 (konfigureret i Azure)

### Cookie Security

- **SameSite**: Lax (konfigureret i `Program.cs`)
- **Secure**: Automatisk i produktion når HTTPS er aktiveret

### Azure App Service Security Settings

1. **HTTPS Only**: Aktiveret
2. **Minimum TLS Version**: 1.2
3. **FTPS State**: Disabled (hvis ikke brugt)

---

## HTTPS og Certifikater

### Azure App Service (Automatisk)

Azure App Service inkluderer automatisk HTTPS med wildcard certifikat:
- Certifikatet fornyes automatisk
- Ingen ekstra konfiguration nødvendig

### Custom Domain med SSL

1. **Tilføj custom domain**
   - App Service → Custom domains
   - Tilføj dit domæne

2. **Tilføj SSL certifikat**
   - App Service → TLS/SSL settings
   - Upload dit certifikat eller brug App Service Managed Certificate (gratis)

3. **Force HTTPS**
   - Aktiveret automatisk via `UseHttpsRedirection()` i `Program.cs`

---

## Database Migration

### Første Deployment

1. **Kør migrations lokalt først (test)**
   ```bash
   dotnet ef database update --project Infrastructure
   ```

2. **I Azure (efter deployment)**
   ```bash
   # Via Azure CLI
   az webapp ssh --resource-group rg-coherence-prod --name <app-name>
   cd /home/site/wwwroot
   dotnet ef database update --project .
   ```

### Automatisk Migration

For at køre migrations automatisk ved deployment, tilføj til `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
```

⚠️ **Advarsel**: Dette kan være risikabelt i produktion. Overvej at bruge separate migration scripts.

---

## Konfiguration

### Environment Variables

Sæt følgende i Azure App Service → Configuration → Application settings:

| Setting | Værdi | Beskrivelse |
|---------|-------|-------------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Environment |
| `ConnectionStrings__DefaultConnection` | `<connection-string>` | Database connection |
| `AIService__BaseUrl` | `http://ai-service:8000` | AI service URL |

### Connection String Format

**Azure SQL:**
```
Server=tcp:<server>.database.windows.net,1433;Database=<db>;User ID=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

**Local SQL Server:**
```
Server=localhost;Database=TheWayOfCoherenceDb;Trusted_Connection=True;TrustServerCertificate=True;
```

---

## Troubleshooting

### Applikationen starter ikke

1. **Tjek logs**
   ```bash
   az webapp log tail --resource-group rg-coherence-prod --name <app-name>
   ```

2. **Tjek health endpoint**
   - Gå til: `https://<app-name>.azurewebsites.net/health`

3. **Tjek database connectivity**
   - Verificer connection string
   - Tjek firewall rules i SQL Server

### Database connection fejl

1. **Tjek SQL Server firewall**
   - Azure Portal → SQL Server → Networking
   - Tillad Azure services: Ja
   - Tilføj din IP hvis nødvendig

2. **Verificer connection string**
   - Tjek username/password
   - Tjek server name

### HTTPS redirect loop

1. **Tjek proxy konfiguration**
   - Hvis du bruger en reverse proxy (fx Azure Front Door), tilføj:
   ```csharp
   app.UseForwardedHeaders();
   ```

2. **Tjek ASPNETCORE_URLS**
   - Sørg for at den ikke er sat til HTTP i produktion

---

## Best Practices

1. **Brug Key Vault for secrets**
   - Gem connection strings og API keys i Azure Key Vault
   - Reference i App Service configuration

2. **Enable Application Insights**
   - Tilføj Application Insights til monitoring
   - Track performance og errors

3. **Backup strategi**
   - Konfigurer automatiske backups for SQL Database
   - Test restore procedure

4. **Monitoring**
   - Sæt op alerts for errors
   - Monitor performance metrics

5. **Staging Environment**
   - Opret separate App Service for staging
   - Test før production deployment

---

## Support

For spørgsmål eller problemer, kontakt development teamet eller se:
- [Azure App Service Documentation](https://docs.microsoft.com/azure/app-service/)
- [ASP.NET Core Security](https://docs.microsoft.com/aspnet/core/security/)

