terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }
}

provider "azurerm" {
  features {}
}

variable "resource_group_name" {}

variable "resource_location" {
  default = "westus2"
}

variable "servicebus_namespace_name" {
  default = "sbpostmen"
}

variable "servicebus_topic_name" {
  default = "postcreated"
}

variable "servicebus_subscription_core" {
  default = "postcreatedsubscriptioncore"
}

variable "servicebus_subscription_framework" {
  default = "postcreatedsubscriptionframework"
}

variable "servicebus_subscription_functions" {
  default = "postcreatedsubscriptionfunctions"
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.resource_location
}

resource "azurerm_servicebus_namespace" "sb" {
  name                = var.servicebus_namespace_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Standard"
}

resource "azurerm_servicebus_topic" "sb_topic_msg_sent" {
  name                = var.servicebus_topic_name
  namespace_id        = azurerm_servicebus_namespace.sb.id
  default_message_ttl = "P14D"
}

resource "azurerm_servicebus_subscription" "sb_subscription_msg_sent_core" {
  name                                      = var.servicebus_subscription_core
  topic_id                                  = azurerm_servicebus_topic.sb_topic_msg_sent.id
  max_delivery_count                        = 10
  default_message_ttl                       = "P14D"
  auto_delete_on_idle                       = "P14D"
  dead_lettering_on_filter_evaluation_error = true
  dead_lettering_on_message_expiration      = true
}

resource "azurerm_servicebus_subscription" "sb_subscription_msg_sent_framework" {
  name                                      = var.servicebus_subscription_framework
  topic_id                                  = azurerm_servicebus_topic.sb_topic_msg_sent.id
  max_delivery_count                        = 10
  default_message_ttl                       = "P14D"
  auto_delete_on_idle                       = "P14D"
  dead_lettering_on_filter_evaluation_error = true
  dead_lettering_on_message_expiration      = true
}

resource "azurerm_servicebus_subscription" "sb_subscription_msg_sent_functions" {
  name                                      = var.servicebus_subscription_functions
  topic_id                                  = azurerm_servicebus_topic.sb_topic_msg_sent.id
  max_delivery_count                        = 10
  default_message_ttl                       = "P14D"
  auto_delete_on_idle                       = "P14D"
  dead_lettering_on_filter_evaluation_error = true
  dead_lettering_on_message_expiration      = true
}

data "azurerm_servicebus_namespace" "broker" {
  name                = azurerm_servicebus_namespace.sb.name
  resource_group_name = azurerm_resource_group.rg.name
}
output "BrokerConnectionString" {
  value     = data.azurerm_servicebus_namespace.broker.default_primary_connection_string
  sensitive = true
}
