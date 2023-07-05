terraform {
  backend "azurerm" {
    resource_group_name  = "tfstate"
    storage_account_name = "sadevopsvanillaexample"
    container_name       = "terraform"
    key                  = "terraform.tfstate"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.56.0"
    }
  }
}

provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {}

locals {
  suffix     = "orldevops-${terraform.workspace}"
  mssql_user = "sqladmin"
}

resource "azurerm_resource_group" "main" {
  location = "eastus"
  name     = "rg-${local.suffix}"
}
