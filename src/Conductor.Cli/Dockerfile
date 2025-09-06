# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY ./src ./
COPY Directory.Build.props ./

WORKDIR Conductor.Cli

RUN dotnet restore Conductor.Cli.csproj
RUN dotnet publish Conductor.Cli.csproj -c Release --no-restore -o /app/out/Conductor.Cli

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Install dependencies
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    gpg \
    git \
    golang \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Install Terraform
RUN wget -O- https://apt.releases.hashicorp.com/gpg \
    | gpg --dearmor -o /usr/share/keyrings/hashicorp-archive-keyring.gpg \
 && echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] \
    https://apt.releases.hashicorp.com $(. /etc/os-release && echo $VERSION_CODENAME) main" \
    > /etc/apt/sources.list.d/hashicorp.list \
 && apt-get update && apt-get install -y terraform \
 && rm -rf /var/lib/apt/lists/*

# Install terraform-config-inspect tool
RUN go install github.com/hashicorp/terraform-config-inspect@latest
ENV PATH="/usr/local/go/bin:/root/go/bin:${PATH}"

# Install Helm
RUN curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 \
 && chmod 700 get_helm.sh \
 && ./get_helm.sh \
 && rm get_helm.sh

# Copy build output
COPY --from=build /app/out/Conductor.Cli ./

ENTRYPOINT ["./Conductor.Cli"]
