mod command;
mod config;
mod resource_template;

use crate::command::cli;
use crate::config::{handle_config_setup, handle_config_info};
use crate::resource_template::{
    CreateResourceTemplateRequest, create_resource_template, get_resource_templates,
};

#[tokio::main]
async fn main() {
    let matches = cli().get_matches();

    match matches.subcommand() {
        Some(("config", sub_matches)) => match sub_matches.subcommand() {
            Some(("setup", _)) => {
                handle_config_setup().unwrap_or_else(|e| {
                    eprintln!("Config setup failed: {}", e);
                    std::process::exit(1);
                });
            }
            Some(("info", _)) => {
                handle_config_info().unwrap_or_else(|e| {
                    eprintln!("Config info failed: {}", e);
                    std::process::exit(1);
                });
            }
            _ => unreachable!(),
        }
        Some(("resource-template", sub_matches)) => match sub_matches.subcommand() {
            Some(("get", _)) => {
                println!("Ran the resource template get sub command");
                let rts = get_resource_templates().await.unwrap();

                for rt in rts {
                    println!("Name: {0}", rt.name)
                }
            }
            Some(("create", _)) => {
                println!("Ran the resource template create sub command");
                let request = CreateResourceTemplateRequest {
                    name: "Test Template".to_string(),
                    resource_template_type: "test.template".to_string(),
                    description: "A test template".to_string(),
                    provider: 0,
                };
                create_resource_template(request).await.unwrap();
            }
            _ => unreachable!(),
        },
        Some(("application", _)) => {
            println!("Ran the application sub command");
        }
        Some(("environment", _)) => {
            println!("Ran the environment sub command");
        }
        Some(("organisation", _)) => {
            println!("Ran the organisation sub command");
        }
        _ => unreachable!(),
    }
}
