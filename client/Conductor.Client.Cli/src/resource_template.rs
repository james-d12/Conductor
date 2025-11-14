use clap::Command;
use reqwest::{Error, StatusCode};
use serde::{Deserialize, Serialize};
use crate::config::get_api_url;

#[derive(Debug, Deserialize, PartialEq)]
pub struct ResourceTemplate {
    pub id: String,
    pub name: String,
    #[serde(rename = "type")]
    pub resource_template_type: String,
    pub description: String,
}

#[derive(Debug, Deserialize)]
pub struct GetResourceTemplatesResponse {
    #[serde(rename = "resourceTemplates")]
    pub resource_templates: Vec<ResourceTemplate>,
}

#[derive(Debug, Serialize)]
pub struct CreateResourceTemplateRequest {
    pub name: String,
    #[serde(rename = "type")]
    pub resource_template_type: String,
    pub description: String,
    pub provider: u8,
}

pub fn get_resource_template_command() -> Command {
    Command::new("resource-template")
        .about("Manage Resource Templates")
        .alias("rt")
        .subcommand_required(true)
        .arg_required_else_help(true)
        .allow_external_subcommands(true)
        .subcommand(Command::new("get").about("Gets Resource Templates"))
        .subcommand(Command::new("delete").about("Delete a Resource Template"))
        .subcommand(Command::new("create").about("Creates a new Resource Template"))
}

pub async fn get_resource_templates() -> Result<Vec<ResourceTemplate>, Error> {
    let api_url = get_api_url().expect("Failed to load API URL. Please run 'cdr config' first.");
    let url = format!("{}/resource-templates", api_url);
    let body = reqwest::get(&url)
        .await?
        .text()
        .await?;
    let resource_templates: GetResourceTemplatesResponse = serde_json::from_str(&body).unwrap();
    Ok(resource_templates.resource_templates)
}

pub async fn create_resource_template(request: CreateResourceTemplateRequest) -> Result<(), Error> {
    let api_url = get_api_url().expect("Failed to load API URL. Please run 'cdr config' first.");
    let url = format!("{}/resource-templates", api_url);
    let client = reqwest::Client::new();
    let response = client
        .post(&url)
        .json(&request)
        .send() 
        .await?;

    let status = response.status();

    if status == StatusCode::OK {
        println!("Created resource template \"{}\"", request.name);
        Ok(())
    } else {
        Err(response.error_for_status().unwrap_err())
    }
}
