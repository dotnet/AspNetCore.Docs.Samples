locals {
  suffix        = "devopsvanillaexample-${terraform.workspace}"
  postgres_user = "psqladmin"
}

resource "azurerm_resource_group" "res-0" {
  location = "eastus"
  name     = "rg-${local.suffix}"
}

resource "azurerm_postgresql_flexible_server" "res-1" {
  delegated_subnet_id = azurerm_subnet.res-11.id

  location               = azurerm_resource_group.res-0.location
  name                   = "sql-${local.suffix}"
  resource_group_name    = azurerm_resource_group.res-0.name
  zone                   = "3"
  administrator_login    = local.postgres_user
  administrator_password = random_password.password.result
  sku_name               = "B_Standard_B1ms"
  version                = "14"
  storage_mb             = 131072
  private_dns_zone_id    = azurerm_private_dns_zone.res-2.id
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
  zone_name           = azurerm_private_dns_zone.res-2.name
  depends_on = [
    azurerm_private_dns_zone.res-2,
  ]
}

resource "azurerm_private_dns_zone_virtual_network_link" "res-5" {
  name                  = "${azurerm_private_dns_zone.res-2.name}-dblink"
  private_dns_zone_name = azurerm_private_dns_zone.res-2.name
  resource_group_name   = azurerm_resource_group.res-0.name
  virtual_network_id    = azurerm_virtual_network.res-9.id
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
  name                  = "${azurerm_private_dns_zone.res-6.name}-applink"
  private_dns_zone_name = azurerm_private_dns_zone.res-6.name
  resource_group_name   = azurerm_resource_group.res-0.name
  virtual_network_id    = azurerm_virtual_network.res-9.id
  depends_on = [
    azurerm_private_dns_zone.res-6,
    azurerm_virtual_network.res-9,
  ]
}

resource "azurerm_virtual_network" "res-9" {
  address_space       = ["10.0.0.0/16"]
  location            = azurerm_resource_group.res-0.location
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
  virtual_network_name = azurerm_virtual_network.res-9.name
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
  virtual_network_name = azurerm_virtual_network.res-9.name
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
  virtual_network_name = azurerm_virtual_network.res-9.name
  depends_on = [
    azurerm_virtual_network.res-9,
  ]
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
  location                  = azurerm_resource_group.res-0.location
  name                      = "app-${local.suffix}"
  resource_group_name       = azurerm_resource_group.res-0.name
  service_plan_id           = azurerm_service_plan.res-13.id
  virtual_network_subnet_id = azurerm_subnet.res-10.id
  connection_string {
    name  = "AZURE_POSTGRESQL_CONNECTIONSTRING"
    type  = "PostgreSQL"
    value = "Server=${azurerm_postgresql_flexible_server.res-1.name}.postgres.database.azure.com;Database=db-devopsvanillaexample-dev;Port=5432;Ssl Mode=Require;User Id=${local.postgres_user};Password=${random_password.password.result};"
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

resource "random_password" "password" {
  length = 16
  //  special          = true
  //  override_special = "!#$%&*()-_=+[]{}<>:?"
}