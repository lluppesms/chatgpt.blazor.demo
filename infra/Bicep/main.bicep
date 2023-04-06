// --------------------------------------------------------------------------------
// Main Bicep file that creates all of the Azure Resources for one environment
// --------------------------------------------------------------------------------
// To deploy this Bicep manually:
// 	 az login
//   az account set --subscription <subscriptionId>
//   az deployment group create -n main-deploy-20230403T093000Z --resource-group rg_blazinchat_demo --template-file 'main.bicep' --parameters orgName=xxx appName=chat environmentCode=demo keyVaultOwnerUserId=<guid> adDomain=yourdomain.onmicrosoft.com adTenantId=<guid> adClientId=<guid>
// --------------------------------------------------------------------------------

param orgName string = ''
param appName string = ''
param fullAppName string = ''
@allowed(['dev','qa','prod','demo','stg','ct','azd','gha','azdo'])
param environmentCode string = 'azd'
param location string = resourceGroup().location
param runDateTime string = utcNow()
param keyVaultOwnerUserId string = ''
param webSiteSku string = 'B1'

param openAIApiKey string = ''
param openAIResourceName string = ''
param openAIImageGenerateUrl string = 'https://api.openai.com/v1/images/generations'
param openAIImageEditUrl string = 'https://api.openai.com/v1/images/edits'
param openAIImageSize string = '512x512'

param adInstance string = environment().authentication.loginEndpoint // 'https://login.microsoftonline.com/'
param adDomain string = ''
param adTenantId string = ''
param adClientId string = ''
param adCallbackPath string = '/signin-oidc'

// --------------------------------------------------------------------------------
var deploymentSuffix = '-${runDateTime}'
// azd deploy only supplies the fullAppName, so use that if orgName is empty, otherwise combine org-app
var fullAppNameVariable = orgName == '' ? fullAppName : '${orgName}-${appName}'
var insightKeyName = 'AppInsightsKey' 
var commonTags = {         
  LastDeployed: runDateTime
  Application: fullAppNameVariable
  Environment: environmentCode
}

// --------------------------------------------------------------------------------
module resourceNames 'resourcenames.bicep' = {
  name: 'resourcenames${deploymentSuffix}'
  params: {
    fullAppName: fullAppNameVariable
    environmentCode: environmentCode
  }
}

module webSiteModule 'website.bicep' = {
  name: 'webSite${deploymentSuffix}'
  params: {
    webSiteName: resourceNames.outputs.webSiteName
    location: location
    appInsightsLocation: location
    commonTags: commonTags
    sku: webSiteSku
  }
}

module keyVaultModule 'keyvault.bicep' = {
  name: 'keyvault${deploymentSuffix}'
  dependsOn: [ webSiteModule ]
  params: {
    keyVaultName: resourceNames.outputs.keyVaultName
    location: location
    commonTags: commonTags
    adminUserObjectIds: [ keyVaultOwnerUserId ]
    applicationUserObjectIds: [ webSiteModule.outputs.principalId ]
  }
}

module keyVaultSecret1 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret1${deploymentSuffix}'
  dependsOn: [ keyVaultModule, webSiteModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: insightKeyName
    secretValue: webSiteModule.outputs.appInsightsKey
  }
}  
module keyVaultSecret2 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret2${deploymentSuffix}'
  dependsOn: [ keyVaultModule, webSiteModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: 'OpenAIApiKey'
    secretValue: openAIApiKey
  }
}  

// In a Linux app service, any nested JSON app key like AppSettings:MyKey needs to be 
// configured in App Service as AppSettings__MyKey for the key name. 
// In other words, any : should be replaced by __ (double underscore).
// NOTE: See https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal  
module webSiteAppSettingsModule 'websiteappsettings.bicep' = {
  name: 'webSiteAppSettings${deploymentSuffix}'
  dependsOn: [ keyVaultSecret1 ]
  params: {
    webAppName: webSiteModule.outputs.name
    appInsightsKey: webSiteModule.outputs.appInsightsKey
    customAppSettings: {
      AppSettings__EnvironmentName: environmentCode

      AppSettings__OpenAIApiKey: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=OpenAIApiKey)'
      AppSettings__OpenAIResourceName: openAIResourceName
      AppSettings__OpenAIImageGenerateUrl: openAIImageGenerateUrl
      AppSettings__OpenAIImageEditUrl: openAIImageEditUrl
      AppSettings__OpenAIImageSize: openAIImageSize

      AzureAD__Instance: adInstance
      AzureAD__Domain: adDomain
      AzureAD__TenantId: adTenantId
      AzureAD__ClientId: adClientId
      AzureAD__CallbackPath: adCallbackPath
    }
  }
}
