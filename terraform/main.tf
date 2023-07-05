locals {
  suffix     = "devopsvanillaexample-${terraform.workspace}"
  mssql_user = "sqladmin"
}

resource "azurerm_resource_group" "res-0" {
  location = "eastus"
  name     = "rg-${local.suffix}"
}

resource "azurerm_service_plan" "res-13" {
  location            = azurerm_resource_group.res-0.location
  name                = "ASP-rgdevopsvanillaexampledev-ab67"
  os_type             = "Linux"
  resource_group_name = azurerm_resource_group.res-0.name
  sku_name            = "B1"
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}

resource "azurerm_linux_web_app" "res-14" {
  location            = azurerm_resource_group.res-0.location
  name                = "app-${local.suffix}"
  resource_group_name = azurerm_resource_group.res-0.name
  service_plan_id     = azurerm_service_plan.res-13.id

  site_config {
    always_on = true
  }
}

resource "random_password" "password" {
  length = 16
}
