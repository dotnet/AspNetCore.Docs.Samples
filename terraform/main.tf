locals {
  suffix = "devopsvanillaexample-${terraform.workspace}"
}

resource "azurerm_resource_group" "res-0" {
  location = "eastus"
  name     = "rg-${local.suffix}"
}
resource "azurerm_postgresql_flexible_server" "res-1" {
  delegated_subnet_id  = azurerm_subnet.res-11.id

  location            = "eastus"
  name                = "sql-${local.suffix}"
  resource_group_name = azurerm_resource_group.res-0.name
  zone                = "3"
  depends_on = [
    azurerm_subnet.res-11,
  ]
}
resource "azurerm_private_dns_zone" "res-2" {
  name                = "privatelink.postgres.database.azure.com"
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}
resource "azurerm_private_dns_a_record" "res-3" {
  name                = "c2b5feaa8e0f"
  records             = ["10.0.1.4"]
  resource_group_name = azurerm_resource_group.res-0.name
  ttl                 = 30
  zone_name           = "privatelink.postgres.database.azure.com"
  depends_on = [
    azurerm_private_dns_zone.res-2,
  ]
}
resource "azurerm_private_dns_zone_virtual_network_link" "res-5" {
  name                  = "privatelink.postgres.database.azure.com-dblink"
  private_dns_zone_name = "privatelink.postgres.database.azure.com"
  resource_group_name   = azurerm_resource_group.res-0.name
  virtual_network_id = azurerm_virtual_network.res-9.id
  depends_on = [
    azurerm_private_dns_zone.res-2,
    azurerm_virtual_network.res-9,
  ]
}
resource "azurerm_private_dns_zone" "res-6" {
  name                = "privatelink.redis.cache.windows.net"
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}
resource "azurerm_private_dns_zone_virtual_network_link" "res-8" {
  name                  = "privatelink.redis.cache.windows.net-applink"
  private_dns_zone_name = "privatelink.redis.cache.windows.net"
  resource_group_name   = azurerm_resource_group.res-0.name
  virtual_network_id = azurerm_virtual_network.res-9.id
  depends_on = [
    azurerm_private_dns_zone.res-6,
    azurerm_virtual_network.res-9,
  ]
}
resource "azurerm_virtual_network" "res-9" {
  address_space       = ["10.0.0.0/16"]
  location            = "eastus"
  name                = "app-devopsvanillaexample-devVnet"
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}
resource "azurerm_subnet" "res-10" {
  address_prefixes     = ["10.0.2.0/24"]
  name                 = "app-devopsvanillaexample-devAppSubnet"
  resource_group_name  = azurerm_resource_group.res-0.name
  virtual_network_name = "app-devopsvanillaexample-devVnet"
  delegation {
    name = "dlg-appServices"
    service_delegation {
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
      name    = "Microsoft.Web/serverFarms"
    }
  }
  depends_on = [
    azurerm_virtual_network.res-9,
  ]
}
resource "azurerm_subnet" "res-11" {
  address_prefixes     = ["10.0.1.0/24"]
  name                 = "app-devopsvanillaexample-devDbSubnet"
  resource_group_name  = azurerm_resource_group.res-0.name
  service_endpoints    = ["Microsoft.Storage"]
  virtual_network_name = "app-devopsvanillaexample-devVnet"
  delegation {
    name = "dlg-database"
    service_delegation {
      actions = ["Microsoft.Network/virtualNetworks/subnets/join/action"]
      name    = "Microsoft.DBforPostgreSQL/flexibleServers"
    }
  }
  depends_on = [
    azurerm_virtual_network.res-9,
  ]
}
resource "azurerm_subnet" "res-12" {
  address_prefixes     = ["10.0.0.0/24"]
  name                 = "app-devopsvanillaexample-devSubnet"
  resource_group_name  = azurerm_resource_group.res-0.name
  virtual_network_name = "app-devopsvanillaexample-devVnet"
  depends_on = [
    azurerm_virtual_network.res-9,
  ]
}
resource "azurerm_service_plan" "res-13" {
  location            = "eastus"
  name                = "ASP-rgdevopsvanillaexampledev-ab67"
  os_type             = "Linux"
  resource_group_name = azurerm_resource_group.res-0.name
  sku_name            = "B1"
  depends_on = [
    azurerm_resource_group.res-0,
  ]
}
resource "azurerm_linux_web_app" "res-14" {
  location                  = "eastus"
  name                      = "app-${local.suffix}"
  resource_group_name       = azurerm_resource_group.res-0.name
  service_plan_id = azurerm_service_plan.res-13.id
  virtual_network_subnet_id = azurerm_subnet.res-10.id
  connection_string {
    name  = "AZURE_POSTGRESQL_CONNECTIONSTRING"
    type  = "PostgreSQL"
    value = "Server=sql-devopsvanillaexample-dev.postgres.database.azure.com;Database=db-devopsvanillaexample-dev;Port=5432;Ssl Mode=Require;User Id=lthmnxayst;Password=REDACTED;"
  }
  site_config {
    always_on              = false
    ftps_state             = "FtpsOnly"
    vnet_route_all_enabled = true
  }
  depends_on = [
    azurerm_subnet.res-10,
    azurerm_service_plan.res-13,
  ]
}
resource "azurerm_app_service_custom_hostname_binding" "res-18" {
  app_service_name    = "app-${local.suffix}"
  hostname            = "app-devopsvanillaexample-dev.azurewebsites.net"
  resource_group_name = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_linux_web_app.res-14,
  ]
}
