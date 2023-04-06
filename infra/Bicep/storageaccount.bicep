// --------------------------------------------------------------------------------
// This BICEP file will create storage account
// --------------------------------------------------------------------------------
param storageAccountName string = 'mystorageaccountname'
param location string = resourceGroup().location
param commonTags object = {}

@allowed([ 'Standard_LRS', 'Standard_GRS', 'Standard_RAGRS' ])
param storageSku string = 'Standard_LRS'
param storageAccessTier string = 'Hot'

// --------------------------------------------------------------------------------
var templateTag = { TemplateFile: '~storageAccount.bicep' }
var tags = union(commonTags, templateTag)

// --------------------------------------------------------------------------------
resource storageAccountResource 'Microsoft.Storage/storageAccounts@2019-06-01' = {
    name: storageAccountName
    location: location
    sku: {
        name: storageSku
    }
    tags: tags
    kind: 'StorageV2'
    properties: {
        networkAcls: {
            bypass: 'AzureServices'
            virtualNetworkRules: []
            ipRules: []
            defaultAction: 'Deny' // 'Allow'/'Deny' - security rules want Deny, but that sometimes makes apps break...!
        }
        supportsHttpsTrafficOnly: true
        encryption: {
            services: {
                file: {
                    keyType: 'Account'
                    enabled: true
                }
                blob: {
                    keyType: 'Account'
                    enabled: true
                }
            }
            keySource: 'Microsoft.Storage'
        }
        accessTier: storageAccessTier
        allowBlobPublicAccess: false
        minimumTlsVersion: 'TLS1_2'
    }
}

resource blobServiceResource 'Microsoft.Storage/storageAccounts/blobServices@2019-06-01' = {
    parent: storageAccountResource
    name: 'default'
    properties: {
        cors: {
            corsRules: [
            ]
        }
        deleteRetentionPolicy: {
            enabled: true
            days: 7
        }
    }
}

// @description('Optional. Name of private endpoint to create, if any')
// param privateEndpointName string = ''
@description('Optional unless PE. Name of VNET where private endpoint will be created')
param vnetName string = ''
@description('Optional unless PE. Name of subnet where private endpoint will be created')
param vnetSubnetName string = ''
@description('Optional unless PE. Name of resource group where the VNET lives')
param vnetResourceGroup string = ''


@description('Optional. Array of Private Endpoint details (Required: subnetId; Optional: service, name, privateDnsZoneIds, customDnsConfigs).')
param privateEndpoints array = []
// Sub-Resource Service GroupIDs:
// Blob (blob, blob_secondary)
// Table (table, table_secondary)
// Queue (queue, queue_secondary)
// File (file, file_secondary)
// Web (web, web_secondary)
@description('Private Endpoints')

module storageBlob_privateEndpoints 'vnet.privateendpoint.bicep' = [for (endpoint, index) in privateEndpoints: {
  name: endpoint
  params: {
    remoteResourceId: storageAccountResource.id
    vnetName: vnetName
    vnetSubnetName: vnetSubnetName
    service: 'blob'
    privateDnsZoneIds: []
    customDnsConfigs: []
    // service: contains(endpoint, 'service') ? endpoint.service : 'blob'
    // privateDnsZoneIds: contains(endpoint, 'privateDnsZoneIds') ? endpoint.privateDnsZoneIds : []
    // customDnsConfigs: contains(endpoint, 'customDnsConfigs') ? endpoint.customDnsConfigs : []
    tags: tags
    location: location
  }
}]


output id string = storageAccountResource.id
output name string = storageAccountResource.name
