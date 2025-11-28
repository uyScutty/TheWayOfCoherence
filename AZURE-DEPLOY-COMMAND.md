# Azure Deployment - Korrekt Kommando

## Problem: Backslashes virker ikke i Command Prompt

Backslashes (`\`) er til line continuation i PowerShell, men virker ikke i Command Prompt (cmd.exe).

## Løsning 1: Brug PowerShell (Anbefalet)

1. **Åbn PowerShell** (ikke Command Prompt)
2. **Naviger til projekt root:**
   ```powershell
   cd C:\Users\sero-\source\repos\TheWayOfCoherence
   ```

3. **Login til Azure:**
   ```powershell
   az login
   ```

4. **Kør deployment på ÉN linje:**
   ```powershell
   az deployment group create --resource-group WayOfCoherence --template-file azure-deploy.bicep --parameters azure-deploy.parameters.json
   ```

## Løsning 2: Brug Command Prompt (Hvis du foretrækker det)

I Command Prompt skal kommandoen være på én linje:

```cmd
cd C:\Users\sero-\source\repos\TheWayOfCoherence
az login
az deployment group create --resource-group WayOfCoherence --template-file azure-deploy.bicep --parameters azure-deploy.parameters.json
```

## Vigtigt: Resource Group Navn

Du har oprettet resource group med navnet **"WayOfCoherence"** (ikke "rg-coherence-prod"), så brug det navn!

## Fuld Step-by-Step i PowerShell

```powershell
# 1. Naviger til projekt root
cd C:\Users\sero-\source\repos\TheWayOfCoherence

# 2. Login til Azure
az login

# 3. Vælg subscription (hvis du har flere)
az account list --output table
az account set --subscription "Azure for Students"

# 4. Deploy infrastructure (på én linje!)
az deployment group create --resource-group WayOfCoherence --template-file azure-deploy.bicep --parameters azure-deploy.parameters.json
```

## Hvis du får fejl om resource group ikke findes

Tjek at resource group navnet er korrekt:

```powershell
# Se alle resource groups
az group list --output table
```

Hvis din resource group hedder noget andet, brug det navn i stedet.

## Efter Deployment

Når deployment er færdig (tager 5-10 minutter), kan du se output:

```powershell
# Se deployment output
az deployment group show --resource-group WayOfCoherence --name azure-deploy --query properties.outputs
```

Dette viser App Service navn og SQL Server navn, som du skal bruge i næste steps.

