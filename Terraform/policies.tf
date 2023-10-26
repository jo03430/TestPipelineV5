resource "azurerm_api_management_api_policy" "this" {
  api_name            = azurerm_api_management_api.this.name
  resource_group_name = azurerm_api_management_api.this.resource_group_name
  api_management_name = azurerm_api_management_api.this.api_management_name
  xml_content         = file("./policies/all_operations.cshtml")

  depends_on = [
    // Need to add additional named values that this policy requires
    azurerm_api_management_named_value.api_client_id,
    azurerm_api_management_named_value.api_scope,
    azurerm_api_management_backend.this
  ]

  lifecycle {
    replace_triggered_by = [
      azurerm_api_management_api.this
    ]
  }
}
