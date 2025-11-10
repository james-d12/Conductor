use clap::Command;
use clap::builder::Str;
use reqwest::Error;
use serde::Deserialize;

#[derive(Debug, Deserialize, PartialEq)]
pub struct ResourceTemplate {
    pub id: String,
    pub name: String,
    #[serde(rename = "type")]
    pub resource_template_type: String,
    pub description: String,
}

#[derive(Debug, Deserialize)]
struct ResourceTemplatesResponse {
    #[serde(rename = "resourceTemplates")]
    pub resource_templates: Vec<ResourceTemplate>,
}

fn get_resource_template_command() -> Command {
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

fn get_application_command() -> Command {
    Command::new("application")
        .about("Manage Applications")
        .alias("app")
        .subcommand_required(true)
        .arg_required_else_help(true)
        .allow_external_subcommands(true)
        .subcommand(Command::new("get").about("Gets Applications"))
        .subcommand(Command::new("delete").about("Delete an Application"))
        .subcommand(Command::new("create").about("Creates a new Application"))
}

fn get_environment_command() -> Command {
    Command::new("environment")
        .about("Manage Environments")
        .alias("env")
        .subcommand_required(true)
        .arg_required_else_help(true)
        .allow_external_subcommands(true)
        .subcommand(Command::new("get").about("Gets Environments"))
        .subcommand(Command::new("delete").about("Delete an Environment"))
        .subcommand(Command::new("create").about("Creates a new Environment"))
}

fn get_organisation_command() -> Command {
    Command::new("organisation")
        .about("Manage Organisations")
        .alias("org")
        .subcommand_required(true)
        .arg_required_else_help(true)
        .allow_external_subcommands(true)
        .subcommand(Command::new("get").about("Gets Organisations"))
        .subcommand(Command::new("delete").about("Delete an Organisation"))
        .subcommand(Command::new("create").about("Creates a new Organisation"))
}

fn cli() -> Command {
    Command::new("cdr")
        .about("Manage Conductor through the Cli")
        .subcommand_required(true)
        .arg_required_else_help(true)
        .allow_external_subcommands(true)
        .subcommand(get_resource_template_command())
        .subcommand(get_application_command())
        .subcommand(get_environment_command())
        .subcommand(get_organisation_command())
}

async fn get_resource_templates() -> Result<Vec<ResourceTemplate>, Error> {
    let body = reqwest::get("http://localhost:5222/resource-templates")
        .await?
        .text()
        .await?;
    let resource_templates: ResourceTemplatesResponse = serde_json::from_str(&body).unwrap();
    Ok(resource_templates.resource_templates)
}

#[tokio::main]
async fn main() {
    let matches = cli().get_matches();

    match matches.subcommand() {
        Some(("resource-template", sub_matches)) => match sub_matches.subcommand() {
            Some(("get", sub_matches)) => {
                println!("Ran the resource template get sub command");
                let rts = get_resource_templates().await.unwrap();

                for rt in rts {
                    println!("Name: {0}", rt.name)
                }
            }
            _ => unreachable!(),
        },
        Some(("application", sub_matches)) => {
            println!("Ran the application sub command");
        }
        Some(("environment", sub_matches)) => {
            println!("Ran the environment sub command");
        }
        Some(("organisation", sub_matches)) => {
            println!("Ran the organisation sub command");
        }
        _ => unreachable!(),
    }
}
