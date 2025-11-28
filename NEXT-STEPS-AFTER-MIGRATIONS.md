# Næste Steps - Efter Migrations er Kørt

Nu hvor migrations er kørt, kan du teste applikationen!

## Option 1: Test Lokal Version (Anbefalet først)

### Step 1: Start applikationen lokalt

**I Visual Studio:**
- Tryk **F5** (eller Debug → Start Debugging)
- Eller højreklik på `TheWayOfCoherence` projekt → Debug → Start New Instance

**Eller fra Command Line:**
```powershell
cd C:\Users\sero-\source\repos\TheWayOfCoherence\TheWayOfCoherence
dotnet run
```

### Step 2: Test applikationen

- Åbn browser og gå til: `https://localhost:7194` (eller den port Visual Studio viser)
- Test at siden loader
- Test at du kan se blog posts
- Test login/registration

### Step 3: Tjek om alt virker

- ✅ Forsiden viser blog posts fra databasen
- ✅ Du kan logge ind
- ✅ Admin dashboard virker (hvis du er logget ind som admin)

## Option 2: Test Docker Version

Hvis du vil teste Docker versionen:

### Step 1: Start Docker containere

```powershell
cd C:\Users\sero-\source\repos\TheWayOfCoherence
docker-compose up -d
```

### Step 2: Vent på at containere starter

```powershell
# Følg logs
docker-compose logs -f webapp
```

Vent til du ser: `Now listening on: http://[::]:8080`

### Step 3: Test Docker versionen

- Åbn browser: `http://localhost:8080`
- Test at siden virker
- Test health endpoint: `http://localhost:8080/api/health`

## Hvad skal du teste?

### Grundlæggende funktionalitet:
1. ✅ Forsiden loader og viser blog posts
2. ✅ Navigation virker
3. ✅ Login/Registration virker
4. ✅ Admin dashboard virker (hvis admin bruger eksisterer)

### Admin funktionalitet:
1. ✅ Opret ny post (`/admin/posts/create`)
2. ✅ Se posts på forsiden efter oprettelse
3. ✅ Health check endpoint virker

## Troubleshooting

### Hvis lokal version ikke virker:

1. **Tjek connection string** i `appsettings.json`
   - Skal pege på din lokale SQL Server eller Docker SQL Server

2. **Tjek om SQL Server kører**
   - Hvis lokal: Tjek Services (services.msc)
   - Hvis Docker: `docker ps | grep coherence-db`

3. **Tjek logs**
   - I Visual Studio: Output window
   - Eller: Console output fra `dotnet run`

### Hvis Docker version ikke virker:

1. **Tjek containere kører:**
   ```powershell
   docker-compose ps
   ```

2. **Tjek logs:**
   ```powershell
   docker-compose logs webapp
   docker-compose logs sqlserver
   ```

3. **Test health endpoint:**
   ```powershell
   curl http://localhost:8080/api/health
   ```

## Anbefalet rækkefølge

1. **Test lokal version først** (nemmest at debugge)
2. **Når lokal version virker**, test Docker version
3. **Sammenlign** at begge virker ens

## Næste læringsmål

Nu hvor du har:
- ✅ Docker setup
- ✅ Migrations kørende
- ✅ Lokal og Docker versioner

Du kan nu:
- Teste deployment til Azure
- Konfigurere CI/CD
- Optimere Docker images
- Tilføje monitoring

