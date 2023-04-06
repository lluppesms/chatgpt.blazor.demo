# Deployment Template Notes

## 1. Template Definitions

Typically, you would want to set up either Option (a), or Option (b) AND Option (c), but not all three jobs.

- **infra-and-website-pipeline.yml:** Deploys the main.bicep template, builds the website code, then deploys the website to the Azure App Service
- **infra-only-pipeline.yml:** Deploys the main.bicep template and does nothing else
- **app-only-pipeline.yml:** Builds the function app and then deploys the website to the Azure App Service

---

## 2. Deploy Environments

These YML files were designed to run as multi-stage environment deploys (i.e. DEV/QA/PROD). Each Azure DevOps environments can have permissions and approvals defined. For example, DEV can be published upon change, and QA/PROD environments can require an approval before any changes are made.

---

## 3. These pipelines needs a variable group named "ChatGPT"

To create these variable groups, customize and run this command in the Azure Cloud Shell, once for each environment:

``` bash
az login
az pipelines variable-group create 
  --organization=https://dev.azure.com/<yourAzDOOrg>/ 
  --project='<yourAzDOProject>'
  --name ChatGPT 
  --variables 
      keyVaultOwnerUserId='<owner1SID>'
      location='eastus'
      resourceGroupPrefix='rg_blazingchat'
      serviceConnectionName='<yourServiceConnection>'
      subscriptionId='<yourSubscriptionId>'
      subscriptionName='<yourAzureSubscriptionName>'
      adDomain='yourDomain.onmicrosoft.com'
      adTenantId='yourTenantId'
      adClientId='yourClientId'
      orgName='<yourInitials>'
      appName='ChatGPT'
      webSiteSku='B1'
      environmentCode='demo'
      openAIApiKey='<yourOpenAIApiKey>'
      openAIResourceName='<yourOpenAIAzureResource>'
      openAIImageGenerateUrl='https://api.openai.com/v1/images/generations'
      openAIImageEditUrl='https://api.openai.com/v1/images/edits'
      openAIImageSize='512x512'
 ```
