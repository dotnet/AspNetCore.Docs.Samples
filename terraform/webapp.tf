resource "azurerm_service_plan" "main" {
  location            = azurerm_resource_group.main.location
  name                = "plan-${local.suffix}"
  os_type             = "Windows"
  resource_group_name = azurerm_resource_group.main.name
  sku_name            = "B1"
}

resource "azurerm_windows_web_app" "main" {
  location            = azurerm_resource_group.main.location
  name                = "app-${local.suffix}"
  resource_group_name = azurerm_resource_group.main.name
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    always_on = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT" = terraform.workspace
  }

  identity {
    type = "SystemAssigned"
  }
}
