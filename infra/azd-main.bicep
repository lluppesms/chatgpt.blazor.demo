// ----------------------------------------------------------------------------------------------------
// This BICEP file is the main entry point for the azd command
// ----------------------------------------------------------------------------------------------------
// Note: The custom parameters need to be added to your .env file by running the azd set command
//   See ./azure/readme.md for more information
// ----------------------------------------------------------------------------------------------------
// Note: Resources need azd-env-name tag and azd-service-name tags (it's already in the bicep files):
//   See https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/faq
// ----------------------------------------------------------------------------------------------------
param name string
param location string
param principalId string = ''
param runDateTime string = utcNow()

// CUSTOM PARAMETERS: ---------------------------------------------------------------------------------
param openAIApiKey string = ''
param openAIResourceName string = ''
param dallEApiKey string = ''
param adDomain string = ''
param adTenantId string = ''
param adClientId string = ''

// --------------------------------------------------------------------------------
targetScope = 'subscription'

// --------------------------------------------------------------------------------
var tags = {
    Application: name
    LastDeployed: runDateTime
    'azd-env-name': name
}
var deploymentSuffix = '-${runDateTime}'

// --------------------------------------------------------------------------------
resource resourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
    name: 'rg-${name}'
    location: location
    tags: tags
}

module resources './Bicep/main.bicep' = {
    name: 'resources-${deploymentSuffix}'
    scope: resourceGroup
    params: {
        location: location
        fullAppName: name
        keyVaultOwnerUserId: principalId
        environmentCode: 'azd'
        openAIApiKey: openAIApiKey
        openAIResourceName: openAIResourceName
        dallEApiKey: dallEApiKey
        adDomain: adDomain
        adTenantId: adTenantId
        adClientId: adClientId
    }
}
