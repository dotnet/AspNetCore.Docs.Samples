resource "azurerm_mssql_server" "main" {
  name                         = "sql-${local.suffix}"
  resource_group_name          = azurerm_resource_group.res-0.name
  location                     = azurerm_resource_group.res-0.location
  version                      = "12.0"
  administrator_login          = local.mssql_user
  administrator_login_password = random_password.password.result
}

resource "azurerm_mssql_database" "main" {
  name      = "db-${local.suffix}"
  server_id = azurerm_mssql_server.main.id
  sku_name  = "Basic"
}
