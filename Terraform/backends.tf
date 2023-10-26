resource "azurerm_api_management_backend" "this" {
  name                = var.api_name
  resource_group_name = azurerm_api_management_api.this.resource_group_name
  api_management_name = azurerm_api_management_api.this.api_management_name
  protocol            = "http"
  url                 = var.backend_url

  credentials {
    authorization {
      scheme    = "Bearer"
      parameter = "{{CHANGE ME}}"
    }
  }

  tls {
    validate_certificate_chain = false
    validate_certificate_name  = true
  }

  depends_on = [
    azurerm_api_management_named_value.net_auth_token
  ]
}
