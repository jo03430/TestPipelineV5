terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.6.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  client_id       = var.client_id
  client_secret   = var.client_secret
  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id

  features {}
}

resource "azurerm_storage_blob" "this" {
  name                   = "${var.environment}-${var.api_name}.json"
  storage_account_name   = var.storage_account_name
  storage_container_name = var.storage_container_name
  type                   = "Block"
  source                 = "${var.api_name}.json"
}
