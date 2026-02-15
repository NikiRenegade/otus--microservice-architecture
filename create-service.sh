#!/bin/bash
set -e

# ========= CONFIG =========
SERVICE_NAME="UserService"
DOTNET_VERSION="net9.0"
SOLUTION_NAME="platform"
ROOT_DIR="platform"
# ==========================

echo "🚀 Adding service: $SERVICE_NAME"

cd "$ROOT_DIR"

# ================= SERVICE FOLDER =================
mkdir -p "$SERVICE_NAME"
cd "$SERVICE_NAME"

# ================= Application =================
mkdir -p Application
dotnet new classlib \
  -n "$SERVICE_NAME.Application" \
  -f "$DOTNET_VERSION" \
  -o "Application/$SERVICE_NAME.Application"

# ================= Domain =================
mkdir -p Domain
dotnet new classlib \
  -n "$SERVICE_NAME.Domain" \
  -f "$DOTNET_VERSION" \
  -o "Domain/$SERVICE_NAME.Domain"

dotnet new classlib \
  -n "$SERVICE_NAME.Domain.Interfaces" \
  -f "$DOTNET_VERSION" \
  -o "Domain/$SERVICE_NAME.Domain.Interfaces"

# ================= Infrastructure =================
mkdir -p Infrastructure

dotnet new classlib \
  -n "$SERVICE_NAME.Infrastructure" \
  -f "$DOTNET_VERSION" \
  -o "Infrastructure/$SERVICE_NAME.Infrastructure"

dotnet new classlib \
  -n "$SERVICE_NAME.Infrastructure.EntityFramework" \
  -f "$DOTNET_VERSION" \
  -o "Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework"

dotnet new classlib \
  -n "$SERVICE_NAME.Infrastructure.Repositories" \
  -f "$DOTNET_VERSION" \
  -o "Infrastructure/$SERVICE_NAME.Infrastructure.Repositories"

# ================= Presentation =================
mkdir -p Presentation
dotnet new webapi \
  -n "$SERVICE_NAME.Presentation.API" \
  -f "$DOTNET_VERSION" \
  -o "Presentation/$SERVICE_NAME.Presentation.API" \
  --no-https

# ================= ADD TO SOLUTION =================
cd ..

dotnet sln "$SOLUTION_NAME.sln" add \
  --solution-folder "$SERVICE_NAME/Application" \
  "$SERVICE_NAME/Application/$SERVICE_NAME.Application/$SERVICE_NAME.Application.csproj"

dotnet sln "$SOLUTION_NAME.sln" add \
  --solution-folder "$SERVICE_NAME/Domain" \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain/$SERVICE_NAME.Domain.csproj" \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain.Interfaces/$SERVICE_NAME.Domain.Interfaces.csproj"

dotnet sln "$SOLUTION_NAME.sln" add \
  --solution-folder "$SERVICE_NAME/Infrastructure" \
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure/$SERVICE_NAME.Infrastructure.csproj" \
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/$SERVICE_NAME.Infrastructure.EntityFramework.csproj" \
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.Repositories/$SERVICE_NAME.Infrastructure.Repositories.csproj"

dotnet sln "$SOLUTION_NAME.sln" add \
  --solution-folder "$SERVICE_NAME/Presentation" \
  "$SERVICE_NAME/Presentation/$SERVICE_NAME.Presentation.API/$SERVICE_NAME.Presentation.API.csproj"

# ================= REFERENCES =================

dotnet add "$SERVICE_NAME/Application/$SERVICE_NAME.Application" reference \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain" \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain.Interfaces"

dotnet add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure" reference \
  "$SERVICE_NAME/Application/$SERVICE_NAME.Application"

dotnet add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework" reference \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain"

dotnet add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.Repositories" reference \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain" \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain.Interfaces" \
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework"

dotnet add "$SERVICE_NAME/Presentation/$SERVICE_NAME.Presentation.API" reference \
  "$SERVICE_NAME/Application/$SERVICE_NAME.Application" \
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.Repositories"

echo "Service $SERVICE_NAME successfully added to $SOLUTION_NAME.sln"
