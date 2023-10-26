data "azurerm_key_vault" "this" {
  name                = var.key_vault_name
  resource_group_name = var.key_vault_resource_group_name
}

data "azurerm_key_vault_secret" "net_auth_token" {
  name         = var.net_auth_token.key_vault_name
  key_vault_id = data.azurerm_key_vault.this.id
}

resource "azurerm_api_management_named_value" "net_auth_token" {
  name                = var.net_auth_token.named_value_name
  resource_group_name = azurerm_api_management_api.this.resource_group_name
  api_management_name = azurerm_api_management_api.this.api_management_name
  display_name        = var.net_auth_token.named_value_name
  secret              = true

  value_from_key_vault {
    secret_id = data.azurerm_key_vault_secret.net_auth_token.id
  }
}

resource "azurerm_api_management_named_value" "api_client_id" {
  name                = var.api_client_id.name
  resource_group_name = azurerm_api_management_api.this.resource_group_name
  api_management_name = azurerm_api_management_api.this.api_management_name
  display_name        = var.api_client_id.name
  secret              = false
  value               = var.api_client_id.value
}

resource "azurerm_api_management_named_value" "api_scope" {
  name                = var.api_scope.name
  resource_group_name = azurerm_api_management_api.this.resource_group_name
  api_management_name = azurerm_api_management_api.this.api_management_name
  display_name        = var.api_scope.name
  secret              = false
  value               = var.api_scope.value
}
