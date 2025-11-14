mod command;
mod resource_template;

use crate::command::cli;
use crate::resource_template::{
    CreateResourceTemplateRequest, create_resource_template, get_resource_templates,
};

#[tokio::main]
async fn main() {
    let matches = cli().get_matches();

    match matches.subcommand() {
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
