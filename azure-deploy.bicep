// Azure Bicep template for deploying The Way of Coherence

@description('The location for all resources')
param location string = resourceGroup().location

@description('The name of the App Service')
param appServiceName string = 'app-coherence-${uniqueString(resourceGroup().id)}'

@description('The name of the SQL Server')
param sqlServerName string = 'sql-coherence-${uniqueString(resourceGroup().id)}'

@description('The name of the SQL Database')
param sqlDatabaseName string = 'TheWayOfCoherenceDb'

@description('SQL Server administrator login')
@secure()
param sqlAdminLogin string

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string

@description('The SKU for the App Service Plan')
param appServicePlanSku string = 'B1' // Basic tier - change to S1, P1V2, etc. for production

@description('The SKU for the SQL Database')
param sqlDatabaseSku string = 'Basic' // Basic tier - change to S0, S1, etc. for production

// Key Vault
var keyVaultName = 'kv-coherence-${uniqueString(resourceGroup().id)}'

// Key Vault
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    enabledForDeployment: false
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: false
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: subscription().tenantId // Will be updated with managed identity
        permissions: {
          secrets: ['get', 'list']
        }
      }
    ]
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

// Store SQL Connection String in Key Vault
resource sqlConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'ConnectionStrings--DefaultConnection'
  properties: {
    value: 'Server=${sqlServer.properties.fullyQualifiedDomainName};Database=${sqlDatabaseName};User Id=${sqlAdminLogin};Password=${sqlAdminPassword};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}

// App Service Managed Identity
resource appServiceIdentity 'Microsoft.Web/sites/config@2023-01-01' = {
  parent: appService
  name: 'identity'
  properties: {
    type: 'SystemAssigned'
  }
}

// Grant Key Vault access to App Service Managed Identity
resource keyVaultAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2023-07-01' = {
  parent: keyVault
  name: 'app-service-access'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: appServiceIdentity.properties.principalId
        permissions: {
          secrets: ['get', 'list']
        }
      }
    ]
  }
  dependsOn: [
    appService
    appServiceIdentity
  ]
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: 'asp-coherence-${uniqueString(resourceGroup().id)}'
  location: location
  sku: {
    name: appServicePlanSku
    tier: appServicePlanSku == 'B1' ? 'Basic' : 'Standard'
  }
  properties: {
    reserved: true // Required for Linux
  }
}

// App Service
resource appService 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|9.0'
      alwaysOn: true
      http20Enabled: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'AIService__BaseUrl'
          value: 'http://localhost:8000' // Update if you have a separate AI service
        }
        {
          name: 'KeyVault__VaultUri'
          value: keyVault.properties.vaultUri
        }
      ]
      keyVaultReferenceIdentity: appServiceIdentity.properties.principalId
    }
  }
}

// SQL Server
resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
  }
}

// SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: sqlDatabaseSku
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648 // 2 GB
  }
}

// SQL Server Firewall Rule - Allow Azure Services
resource sqlFirewallRuleAzure 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// App Service Configuration - Reference Key Vault Secret
resource appServiceConfig 'Microsoft.Web/sites/config@2023-01-01' = {
  parent: appService
  name: 'connectionstrings'
  properties: {
    DefaultConnection: {
      type: 'AzureKeyVault'
      value: '@Microsoft.KeyVault(SecretUri=${keyVault.properties.vaultUri}secrets/ConnectionStrings--DefaultConnection/)'
    }
  }
  dependsOn: [
    keyVault
    sqlConnectionStringSecret
    appServiceIdentity
  ]
}

// Outputs
output appServiceName string = appService.name
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output sqlServerName string = sqlServer.name
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output keyVaultName string = keyVault.name
output keyVaultUri string = keyVault.properties.vaultUri

