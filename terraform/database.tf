resource "random_password" "password" {
  length = 16
}

resource "azurerm_mssql_server" "main" {
  name                         = "sql-${local.suffix}"
  resource_group_name          = azurerm_resource_group.main.name
  location                     = azurerm_resource_group.main.location
  version                      = "12.0"
  administrator_login          = local.mssql_user
  administrator_login_password = random_password.password.result
}

resource "azurerm_mssql_database" "main" {
  name      = "db-${local.suffix}"
  server_id = azurerm_mssql_server.main.id
  sku_name  = "Basic"
}

// Allows Azure resources to access the SQL server
resource "azurerm_mssql_firewall_rule" "azure" {
  name             = "Azure access"
  server_id        = azurerm_mssql_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}
