// --------------------------------------------------------------------------------
// Bicep file that builds all the resource names used by other Bicep templates
// --------------------------------------------------------------------------------
@description('Supply a fullAppName in the format of orgName-appName ')
param fullAppName string = ''
@allowed(['dev','qa','prod','demo','stg','ct','azd','gha','azdo'])
param environmentCode string = 'azd'

// --------------------------------------------------------------------------------
var sanitizedEnvironment = toLower(environmentCode)
var sanitizedFullAppNameWithDashes = replace(replace(toLower(fullAppName), ' ', ''), '_', '')
var sanitizedFullAppName = replace(replace(replace(toLower(fullAppName), ' ', ''), '-', ''), '_', '')

// pull resource abbreviations from a common JSON file
var resourceAbbreviations = loadJsonContent('./resourceAbbreviations.json')

// --------------------------------------------------------------------------------

var webSiteName = environmentCode == 'azd' ? '${sanitizedFullAppNameWithDashes}-azd' : toLower('${sanitizedFullAppNameWithDashes}-${sanitizedEnvironment}')
output webSiteName string               = webSiteName
output webSiteAppServicePlanName string = '${webSiteName}-${resourceAbbreviations.appServicePlanSuffix}'
output webSiteAppInsightsName string    = '${webSiteName}-${resourceAbbreviations.appInsightsSuffix}'

// Key Vaults and Storage Accounts can only be 24 characters long
output keyVaultName string              = take('${sanitizedFullAppName}${resourceAbbreviations.keyVaultAbbreviation}${sanitizedEnvironment}', 24)
output storageAccountName string        = take('${sanitizedFullAppName}${resourceAbbreviations.storageAccountSuffix}${sanitizedEnvironment}', 24)
