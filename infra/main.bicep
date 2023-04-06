// ----------------------------------------------------------------------------------------------------
// This BICEP file is the main entry point for the azd command
// ----------------------------------------------------------------------------------------------------
// Note: Resources need azd-env-name tag and azd-service-name tags:
//   See https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/faq
// ----------------------------------------------------------------------------------------------------
param name string
param location string
param principalId string = ''
param runDateTime string = utcNow()

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

// pull additional settings/keys from a JSON file that the user can edit locally
var azdKeys = loadJsonContent('./azdKeys.json')

module resources './Bicep/main.bicep' = {
    name: 'resources-${deploymentSuffix}'
    scope: resourceGroup
    params: {
        location: location
        fullAppName: name
        keyVaultOwnerUserId: principalId
        environmentCode: 'azd'
        openAIApiKey: azdKeys.openAIApiKey
        openAIResourceName: azdKeys.openAIResourceName
        adDomain: azdKeys.adDomain
        adTenantId: azdKeys.adTenantId
        adClientId: azdKeys.adClientId
    }
}
