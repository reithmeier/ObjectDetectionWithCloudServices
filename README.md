# Object Detection using Azure Cognitive Services

## Requirements
* .NET Core 2.2 SDK
* Azure CLI
* Node.js
* Angular CLI

Required Azure Services:
* Azure Cosmos DB account 
* Cognitive Services
* Azure Functions App Service
* Storage Account


## Deployment

Rename `App.sample.config` to `App.config` in `Logic/` and `DAL/` and set the values accordingly.

Create the services by running `install.azcli`.

Deploy the functions with:
```bash
ressourceGroup='rg_clc3_project'
functions='clc3Functions' 

# build the project
cd FunctionApp
dotnet publish -c Release
cd ..
# zip the project
[io.compression.zipfile]::CreateFromDirectory("FunctionApp/bin/Release/netcoreapp2.2/publish", "publish.zip")
# deploy the project
az functionapp deployment source config-zip `
    -g $ressourceGroup -n $functions --src 'publish.zip'
```

Enable static Website for Storage Account:
```bash
storageAccount='clc3staticwebstorage'
az storage blob service-properties update --account-name $storageAccount --static-website --404-document 'index.html' --index-document 'index.html'
```

Deploy the client: 
```bash
storageAccount='clc3staticwebstorage'
cd ImageWeb
ng build --prod
cd ..
az storage blob upload-batch -s 'ImageWeb/dist/ImageWeb/' -d \$web --account-name $storageAccount
```

