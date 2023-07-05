$RESOURCE_GROUP_NAME='rg-devopsvanillaexample'
$STORAGE_ACCOUNT_NAME='sadevopsvanillaexample'
$Env:ARM_ACCESS_KEY=$(az storage account keys list --resource-group $RESOURCE_GROUP_NAME --account-name $STORAGE_ACCOUNT_NAME --query '[0].value' -o tsv)
