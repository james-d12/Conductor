# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY ./src ./
COPY Directory.Build.props ./

WORKDIR Conductor.Api

RUN dotnet restore Conductor.Api.csproj

RUN dotnet publish Conductor.Api.csproj -c Release --no-restore -o /app/out/Conductor.Api

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build "/app/out/Conductor.Api" ./

ENTRYPOINT ["./Conductor.Api"]