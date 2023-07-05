resource "azurerm_key_vault" "main" {
  name                = "kv-${replace(local.suffix, "devopsvanillaexample", "orldevops")}"
  resource_group_name = azurerm_resource_group.main.name
  sku_name            = "standard"
  location            = azurerm_resource_group.main.location
  tenant_id           = data.azurerm_client_config.current.tenant_id

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions = ["Set", "Get", "Delete"]
  }
}

resource "azurerm_key_vault_secret" "main" {
  name         = "sqlconnectionstring"
  key_vault_id = azurerm_key_vault.main.id
  value        = "Server=tcp:${azurerm_mssql_server.main.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.main.name};Persist Security Info=False;User ID=${local.mssql_user};Password=${random_password.password.result};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
