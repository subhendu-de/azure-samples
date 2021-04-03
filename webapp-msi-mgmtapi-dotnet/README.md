## Managed Identity to access Azure Management REST API
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
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#deployment">Deployment</a></li>
      </ul>
    </li>
  </ol>
</details>

## About The Sample

This is a sample to demonstrate to call the Azure management rest api from an app service. The app service is using system assigned managed identity to read the resources under a resource group.

### Built With

Following technologies, frameworks and tools are used
* [ASP.NET Core MVC](https://dotnet.microsoft.com/apps/aspnet)
* [Azure Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
* [Azure Management REST API](https://github.com/Azure/azure-sdk-for-net)
* [Azure CLI for Azure App Service](https://docs.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest)

## Getting Started

This is a sample to demonstrate to call the Azure management rest api from an app service. The app service is using system assigned managed identity to read the resources under a resource group.

### Prerequisites

The application is built in such a way that it runs in the developer machine and in the Azure environment as well. ```AzureCliCredential``` is used to take the identity context from the Azure cli. It requires logging in to Azure via ```az login```, and will use the cli's currently logged in identity. ```ManagedIdentityCredential``` is used for the Azure environment.

This is an example of how to run the application is the local development environment.

1. Change the ```{subscription-id}``` and ```{resource-group-name}``` in the user-secret file. The application will read the resources under this ```{resource-group-name}```. The content of the user-secret looks like

```json
{
  "SubscriptionId": "{subscription-id}",
  "ResourceGroup": "{resource-group-name}"
}
```

2. Please use  ```az login``` to logging into Azure.
3. Run the following commands to build and run the asp.net core mvc application.

 ```sh
dotnet build
dotnet run 
```

### Deployment

1. Run the following scripts in windows command line

```bat
set subscriptionid={subscription-id}
set targetresourcegroup={target-resource-group-name}
set appsvc=myappsvc%RANDOM%
set webapp=mywebapp%RANDOM%
set resourcegroup=IndiaDC1
set gitrepo=https://github.com/subhendu-de/azure-samples

az group create --location southindia --name %resourcegroup%
az appservice plan create --name %appsvc% --resource-group %resourcegroup% --sku FREE
az webapp create --name %webapp% --resource-group %resourcegroup% --plan %appsvc%
az webapp config appsettings set --resource-group %resourcegroup% --name %webapp% --settings SubscriptionId=%subscriptionid% ResourceGroup=%targetresourcegroup%
az webapp identity assign --resource-group %resourcegroup% --name %webapp% --role reader --scope /subscriptions/%subscriptionid%/%targetresourcegroup%
az webapp deployment source config --name %webapp% --resource-group %resourcegroup% --repo-url %gitrepo% --branch main --manual-integration
```

Please update the ```{subscription-id}``` and ```{target-resource-group-name}```. The application will read the resources under this ```{target-resource-group-name}```

2. Access the site and it displays the list of resources with metadata.
