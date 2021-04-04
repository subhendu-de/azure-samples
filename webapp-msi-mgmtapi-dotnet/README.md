## System assigned Managed Identity to access Azure Management REST API
<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-sample">About The Sample</a>
       <ul>
            <li><a href="#built-with">Built With</a></li>
       </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#developer-sandbox">Developer Sandbox</a></li>
        <li><a href="#azure-environment">Azure Environment</a></li>
      </ul>
    </li>
  </ol>
</details>

## About The Sample

There are many scenarios to access the Azure management library to perform repetitive tasks. The management library has a set of rest api protected over Azure active directory using oauth protocol. It works by passing a bearer token supplied by the Azure AD token endpoint. There are three ways to receive the token from Azure AD.

- System-assigned managed identity
- User-assigned managed identity
- Service principle

The managed identity(both system and user) does not require any credentials to define in the application to work. The Azure AD and Azure services can manage them. Conversely, the service principle works differently. It uses client id and client secret/certificate to define in the application to receive the token. It can follow two different grant types - client credential and password. 
This sample demonstrates accessing the Azure management rest api using the system-assigned managed identity. Please refer [here](/webapp-usrmsi-mgmtapi-dotnet/README.md) for user-assigned managed identity.

### Built With

Following technologies, frameworks and tools are used to build the demo
* [ASP.NET Core MVC](https://dotnet.microsoft.com/apps/aspnet)
* [Azure Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
* [Azure Management REST API](https://github.com/Azure/azure-sdk-for-net)
* [Azure CLI for Azure App Service](https://docs.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest)

## Getting Started

This is a sample demonstration to call the Azure management rest api from an app service. The app service is using a system-assigned managed identity to read the resources under a resource group. The main objective is to showcase

- how to setup system-assigned managed identity of an azure app service
- how the system-assigned managed identity can access the Azure resource group with a reader role

### Developer Sandbox

The application is built to run in the developer machine and the Azure environment as well. The ```AzureCliCredential``` is used to take the identity context from the Azure cli to authenticate and receive the access token. It requires logging in to Azure via ```az login``` first and uses the cli's currently logged in identity. Similarly, the ```ManagedIdentityCredential``` is used for the Azure environment to take the identity context from the msi token endpoint. To know more about the available authentication modes, please refer [here](https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme)

This is an example of how to run the application in the local development environment.

1. The application uses user-secret file in local development environment. Please change the ```{subscription-id}``` and ```{resource-group-name}``` in the user-secret file. The application will read the resources under this ```{resource-group-name}``` mentioned in the file. The content of the file looks like

```json
{
  "SubscriptionId": "{subscription-id}",
  "ResourceGroup": "{resource-group-name}"
}
```

2. Please use  ```az login``` to logging into Azure. This step is mandatory to read the identity context from Azure cli.

3. Run the following commands to build and run the asp.net core mvc application.

 ```sh
<local-path>\webapp-msi-mgmtapi-dotnet> dotnet build .\webapp-msi-mgmtapi-dotnet.csproj -c Release
<local-path>\webapp-msi-mgmtapi-dotnet> dotnet run .\webapp-msi-mgmtapi-dotnet.csproj -c Release 
```

4. Access the site ```https://localhost:5001/Home/Index```and it displays the list of resources with metadata.

### Azure Environment

1. Run the following scripts in windows command line

```bat
set project=webapp-msi-mgmtapi-dotnet/webapp-msi-mgmtapi-dotnet.csproj
set subscriptionid={subscription-id}
set targetresourcegroup={target-resource-group-name}
set appsvc=myappsvc%RANDOM%
set webapp=mywebapp%RANDOM%
set resourcegroup={resource-group-name}
set gitrepo=https://github.com/subhendu-de/azure-samples

az group create --location southindia --name %resourcegroup%
az appservice plan create --name %appsvc% --resource-group %resourcegroup% --sku FREE
az webapp create --name %webapp% --resource-group %resourcegroup% --plan %appsvc%
az webapp config appsettings set --resource-group %resourcegroup% --name %webapp% --settings SubscriptionId=%subscriptionid% ResourceGroup=%targetresourcegroup% PROJECT=%project%
az webapp identity assign --resource-group %resourcegroup% --name %webapp% --role reader --scope /subscriptions/%subscriptionid%/resourceGroups/%targetresourcegroup%
az webapp deployment source config --name %webapp% --resource-group %resourcegroup% --repo-url %gitrepo% --branch main --manual-integration
```

Please update the ```{subscription-id}```, ```{resource-group-name}``` and ```{target-resource-group-name}``` in the script. The application will read the resources under this ```{target-resource-group-name}``` mentioned in the script.

2. Access the site and it displays the list of resources with metadata.
