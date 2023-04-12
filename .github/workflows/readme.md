# Set up GitHub Secrets

The GitHub workflows in this project require several secrets set at the repository level.

---

## Azure Resource Creation Credentials

You need to set up the Azure Credentials secret in the GitHub Secrets at the Repository level before you do anything else.

See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/deploy-github-actions for more info.

It should look something like this:

AZURE_CREDENTIALS:

``` bash
{
  "clientId": "<GUID>", 
  "clientSecret": "<GUID>", 
  "subscriptionId": "<GUID>", 
  "tenantId": "<GUID>"
}
```

---

## Azure Web App Publishing Credentials

AZURE_PUBLISH_PROFILE:

Once you deploy the web application, you will need to download a Publish Profile from the Azure Portal and add it as a secret named `AZURE_PUBLISH_PROFILE` to the repository. This is used by the GitHub Actions to deploy the web application and contains XML when you download it from the portal.

---

## Bicep Configuration Values

These variables and secrets are used by the Bicep templates to configure the resource names that are deployed.  Make sure the App_Name variable is unique to your deploy. It will be used as the basis for the website name and for all the other Azure resources, which must be globally unique.
To create these additional secrets and variables, customize and run this command:

Required Values:

``` bash
gh auth login

gh variable set ORG_NAME -b 'xxx'
gh variable set APP_NAME -b 'chatgpt'
gh variable set WEBSITE_SKU -b 'B1'
gh variable set RESOURCE_GROUP_PREFIX -b 'rg_blazingchat'
gh variable set AZURE_LOCATION -b 'eastus'
gh variable set AZURE_SUBSCRIPTION_ID -b '<yourAzureSubscriptionId>'
gh variable set AZURE_SUBSCRIPTION_NAME -b '<yourAzureSubscriptionName>'
gh variable set SERVICE_CONNECTION_NAME -b '<yourServiceConnection>'
gh variable set KEYVAULT_OWNER_USERID -b '<owner1SID>'
gh variable set OPENAI_RESOURCE_NAME -b '<yourOpenAIAzureResource>'
gh variable set AZURE_AD_DOMAIN -b 'yourDomain.onmicrosoft.com'
gh variable set AZURE_AD_TENANTID -b 'yourTenantId'
gh variable set AZURE_AD_CLIENTID -b 'yourClientId'

gh secret set OPENAI_API_KEY -b '<yourOpenAIApiKey>'
gh secret set DALLE_API_KEY -b '<yourDallEApiKey>'
```

---

## References

[Deploying ARM Templates with GitHub Actions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/deploy-github-actions)

[GitHub Secrets CLI](https://cli.github.com/manual/gh_secret_set)
