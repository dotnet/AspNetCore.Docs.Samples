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

output "sqlserver_rg" {
  value = azurerm_mssql_server.main.resource_group_name
}

output "sqlserver_name" {
  value = azurerm_mssql_server.main.name
}

output "sqlserver_username" {
  value = local.mssql_user
}

resource "azurerm_mssql_database" "main" {
  name      = "db-${local.suffix}"
  server_id = azurerm_mssql_server.main.id
  sku_name  = "Basic"
}

output "sqlserver_db" {
  value = azurerm_mssql_database.main.name
}

// Allows Azure resources to access the SQL server
resource "azurerm_mssql_firewall_rule" "azure" {
  name             = "Azure access"
  server_id        = azurerm_mssql_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

output "sqlserver_password" {
  sensitive = true
  value     = random_password.password.result
}
