resource "azurerm_key_vault" "main" {
  name                = "kv${replace(replace(local.suffix, "-", ""), "devopsvanillaexample", "orldevops")}"
  resource_group_name = azurerm_resource_group.res-0.name
  sku_name            = "standard"
  location            = azurerm_resource_group.res-0.location
  tenant_id           = data.azurerm_client_config.current.tenant_id

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions = [
      "Set", "Get"
    ]
  }
}

resource "azurerm_key_vault_secret" "main" {
  name         = "sqlconnectionstring"
  key_vault_id = azurerm_key_vault.main.id
  value        = "Server=tcp:sql-devopsvanillaexample-dev.database.windows.net,1433;Initial Catalog=db-devopsvanillaexample-dev;Persist Security Info=False;User ID=sqladmin;Password=${random_password.password.result};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
