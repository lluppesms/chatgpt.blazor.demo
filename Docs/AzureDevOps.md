# Azure DevOps Deployment Template Notes

## 1. Azure DevOps Template Definitions

Typically, you would want to set up the either first option or the second and third option, but not all three jobs.

- **infra-and-website-pipeline.yml:** Deploys the main.bicep template, builds the website code, then deploys the website to the Azure App Service
- **infra-only-pipeline.yml:** Deploys the main.bicep template and does nothing else
- **app-only-pipeline.yml:** Builds the function app and then deploys the website to the Azure App Service

---

## 2. Deploy Environments

These Azure DevOps YML files were designed to run as multi-stage environment deploys (i.e. DEV/QA/PROD). Each Azure DevOps environments can have permissions and approvals defined. For example, DEV can be published upon change, and QA/PROD environments can require an approval before any changes are made.

---

## 3. Setup Steps

- [Create Azure DevOps Service Connections](https://docs.luppes.com/CreateServiceConnections/)

- [Create Azure DevOps Environments](https://docs.luppes.com/CreateDevOpsEnvironments/)

- Create Azure DevOps Variable Groups - see [Azure Devops Setup](../.azdo/pipelines/readme.md)

- [Create Azure DevOps Pipeline(s)](https://docs.luppes.com/CreateNewPipeline/)

- [Deploy the Azure Resources and Application](./Docs/DeployApplication.md)

---

## 4. Creating the variable group "ChatGPT"

See the [Azure Devops Setup](../.azdo/pipelines/readme.md) for more information
