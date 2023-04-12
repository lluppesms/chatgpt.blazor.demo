# Azure DevOps Deployment Template Notes

## 1. Azure DevOps Template Definitions

Typically, you would want to set up either Option (a), or Option (b) AND Option (c), but not all three jobs.

- **infra-and-website-pipeline.yml:** Deploys the main.bicep template, builds the website code, then deploys the website to the Azure App Service
- **infra-only-pipeline.yml:** Deploys the main.bicep template and does nothing else
- **app-only-pipeline.yml:** Builds the website and then deploys the website to the Azure App Service

---

## 2. Deploy Environments

These YML files were designed to run as multi-stage environment deploys (i.e. DEV/QA/PROD). Each Azure DevOps environments can have permissions and approvals defined. For example, DEV can be published upon change, and QA/PROD environments can require an approval before any changes are made.

---

## 3. Setup Steps

- [Create Azure DevOps Service Connections](https://docs.luppes.com/CreateServiceConnections/)

- [Create Azure DevOps Environments](https://docs.luppes.com/CreateDevOpsEnvironments/)

- Create Azure DevOps Variable Groups -- see next step

- [Create Azure DevOps Pipeline(s)](https://docs.luppes.com/CreateNewPipeline/)

- Run the infra-and-website-pipeline.yml pipeline to deploy the project to an Azure subscription.

---

## 4. These pipelines needs a variable group named "ChatGPT"

To create this variable group, customize and run this command in the Azure Cloud Shell:

``` bash
az login
az pipelines variable-group create 
  --organization=https://dev.azure.com/<yourAzDOOrg>/ 
  --project='<yourAzDOProject>'
  --name ChatGPT 
  --variables 
      location='eastus'
      resourceGroupPrefix='rg_blazingchat'
      serviceConnectionName='<yourServiceConnection>'
      subscriptionId='<yourSubscriptionId>'
      subscriptionName='<yourAzureSubscriptionName>'
      keyVaultOwnerUserId='<owner1SID>'
      adDomain='yourDomain.onmicrosoft.com'
      adTenantId='yourTenantId'
      adClientId='yourClientId'
      orgName='<yourInitials>'
      appName='ChatGPT'
      webSiteSku='B1'
      environmentCode='demo'
      openAIApiKey='<yourOpenAIApiKey>'
      openAIResourceName='<yourOpenAIAzureResource>'
      dalleApiKey='<yourDallEApiKey>'
```
