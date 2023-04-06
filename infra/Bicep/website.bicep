// --------------------------------------------------------------------------------
// This BICEP file will create a Azure Function
// --------------------------------------------------------------------------------
param webSiteName string = ''
param location string = resourceGroup().location
param appInsightsLocation string = resourceGroup().location
param commonTags object = {}
@allowed(['F1','B1','B2','S1','S2','S3'])
param sku string = 'B1'

// --------------------------------------------------------------------------------
var templateTag = { TemplateFile: '~website.bicep'}
var azdTag = { 'azd-service-name': 'web' }
var tags = union(commonTags, templateTag)
var webSiteTags = union(commonTags, templateTag, azdTag)

// --------------------------------------------------------------------------------
var linuxFxVersion = 'DOTNETCORE|7.0' // 	The runtime stack of web app
var webAppKind = 'linux'
var webSiteAppServicePlanName = toLower('${webSiteName}-appsvc')
var webSiteAppInsightsName = toLower('${webSiteName}-insights')

// --------------------------------------------------------------------------------
resource webSiteAppServicePlanResource 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: webSiteAppServicePlanName
  location: location
  tags: tags
  sku: {
    name: sku
  }
  kind: webAppKind
  properties: {
    reserved: true
  }
}

resource webSiteAppServiceResource 'Microsoft.Web/sites@2020-06-01' = {
  name: webSiteName
  location: location
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
  tags: webSiteTags
  properties: {
    serverFarmId: webSiteAppServicePlanResource.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      appSettings: []
      minTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
      remoteDebuggingEnabled: false
    }
  }
}

resource webSiteAppInsightsResource 'Microsoft.Insights/components@2020-02-02' = {
  name: webSiteAppInsightsName
  location: appInsightsLocation
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

output principalId string = webSiteAppServiceResource.identity.principalId
output name string = webSiteName
output appInsightsName string = webSiteAppInsightsName
output appInsightsKey string = webSiteAppInsightsResource.properties.InstrumentationKey
