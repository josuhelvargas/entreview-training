param location string = resourceGroup().location
param storageAccountName string = 'josuevargas${uniqueString(resourceGroup().id)}'
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
  }
}
resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  parent: storageAccount
  name: 'bicepcontainer'
  properties: {
    publicAccess: 'None'
  }
}
output storageAccountName string = storageAccount.name
output containerName string = container.name
output storageAccountId string = storageAccount.id
output containerId string = container.id
output storageAccountPrimaryLocation string = storageAccount.location
output storageAccountPrimaryEndpoints object = storageAccount.properties.primaryEndpoints
output storageAccountPrimaryEndpointsBlob string = storageAccount.properties.primaryEndpoints.blob
output storageAccountPrimaryEndpointsWeb string = storageAccount.properties.primaryEndpoints.web
output storageAccountPrimaryEndpointsQueue string = storageAccount.properties.primaryEndpoints.queue
output storageAccountPrimaryEndpointsTable string = storageAccount.properties.primaryEndpoints.table    
