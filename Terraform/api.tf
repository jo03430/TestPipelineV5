resource "azurerm_api_management_api" "this" {
  name                  = lower(var.api_name)
  resource_group_name   = var.apim_resource_group_name
  api_management_name   = var.api_management_name
  revision              = "1"
  display_name          = var.api_name
  protocols             = ["https"]
  subscription_required = false
  path                  = "external/${lower(var.api_name)}"

  import {
    content_format = "openapi+json-link"
    content_value  = azurerm_storage_blob.this.url
  }
}
