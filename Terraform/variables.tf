#############################
# Authentication
#############################
variable "tenant_id" {
  description = "The Tenant ID of the TeamCity Service Principle from TC"
  type        = string
}

variable "client_id" {
  description = "The Client ID of the TeamCity Service Principle from TC"
  type        = string
}

variable "client_secret" {
  description = "The Client Secret of the TeamCity Service Principle from TC"
  type        = string
}

variable "subscription_id" {
  description = "The Subscription ID of the Azure Subscription from TC"
  type        = string
}

#############################
# Azure Setup Info
#############################
variable "location" {
  description = "Geographical location of Azure Resources used"
  type        = string
  default     = "centralus"
}

variable "apim_resource_group_name" {
  description = "Resource group name that holds the APIM instance from TC"
  type        = string
}

variable "api_management_name" {
  description = "Name of the APIM instance from TC"
  type        = string
}

variable "storage_account_name" {
  description = "Name of the Storage Account from TC"
  type        = string
}

variable "storage_container_name" {
  description = "Name of the Storage Accounts Container to be used for storing state and swagger.json files"
  type        = string
}

#############################
# API information
#############################
variable "api_name" {
  description = "Name of the project API from TC"
  type        = string
}

variable "environment" {
  description = "Envrionments for Resource Groups from TC"
  type        = string
}

variable "backend_url" {
  description = "Backend connection url, environment specific"
  type        = string
}

#############################
# Key Vault information
#############################
variable "key_vault_name" {
  description = "Name of the Key Vault from TC"
  type        = string
}

variable "key_vault_resource_group_name" {
  description = "Resource group name the Key Vault is under from TC"
  type        = string
}

#############################
# Key Vault Secrets
#############################
variable "net_auth_token" {
  description = ".NET API Auth token secret name, environment specific"
  type = object({
    key_vault_name   = string
    named_value_name = string
  })
}

#############################
# Named Values
#############################
variable "api_client_id" {
  description = "API Service Principal Client ID"
  type = object({
    name  = string
    value = string
  })
}

variable "api_scope" {
  description = "API Service Principal Scope"
  type = object({
    name  = string
    value = string
  })
}
