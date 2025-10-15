# =====================
# Build Stage
# =====================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copy project files
COPY ./src/Conductor.Portal.Web/Conductor.Portal.Web.csproj ./src/Conductor.Portal.Web/
COPY Directory.Build.props ./
COPY . ./

WORKDIR /app/src/Conductor.Portal.Web

# Restore and publish
RUN dotnet restore Conductor.Portal.Web.csproj
RUN dotnet publish Conductor.Portal.Web.csproj -c Release --no-restore -o /app/out/Conductor.Portal.Web


# =====================
# Runtime Stage
# =====================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# =====================
# Create non-root user
# =====================
RUN useradd -m appuser
USER appuser
WORKDIR /app

# =====================
# Expose ports
# =====================
EXPOSE 8080
EXPOSE 8081

# =====================
# Copy application
# =====================
COPY --from=build /app/out/Conductor.Portal.Web ./

# =====================
# Entrypoint
# =====================
ENTRYPOINT ["./Conductor.Portal.Web"]
