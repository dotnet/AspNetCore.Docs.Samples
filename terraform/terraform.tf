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
