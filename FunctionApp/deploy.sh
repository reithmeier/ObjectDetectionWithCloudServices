#!/bin/bash
$resourceGroup = "rg_clc3_project"
$location = "westeurope"
$storageAccountName = "clc3functions"
$functionAppName = "clc3Functions"
# publish the code
dotnet publish -c Release
$publishFolder = "bin/Release/netcoreapp2.2/publish"
# create the zip
$publishZip = "publish.zip"
if(Test-path $publishZip) {Remove-item $publishZip}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($publishFolder, $publishZip)
# deploy the zipped package
az functionapp deployment source config-zip `
 -g $resourceGroup -n $functionAppName --src $publishZip