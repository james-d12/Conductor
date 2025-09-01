# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY ./src ./
COPY Directory.Build.props ./

WORKDIR Conductor.Cli

RUN dotnet restore Conductor.Cli.csproj

RUN dotnet publish Conductor.Cli.csproj -c Release --no-restore -o /app/out/Conductor.Cli

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Install Git and Go with the Terraform Config Inspect Tool
RUN apt update && apt install golang git -y 
RUN go install github.com/hashicorp/terraform-config-inspect@latest
ENV PATH="/usr/local/go/bin:/root/go/bin:${PATH}"

# Install Helmchart
RUN apt install curl -y
RUN curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3
RUN chmod 700 get_helm.sh
RUN ./get_helm.sh

COPY --from=build "/app/out/Conductor.Cli" ./

ENTRYPOINT ["./Conductor.Cli"]