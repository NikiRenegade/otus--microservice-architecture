#!/bin/bash
set -e

# ========= CONFIG =========
SERVICE_NAME="InventoryService"
DOTNET_VERSION="net9.0"
SOLUTION_NAME="ihb-platform-backend"
# ==========================
cd "$SOLUTION_NAME"
echo "Adding service: $SERVICE_NAME"

# ================= SERVICE FOLDER =================
mkdir -p "$SERVICE_NAME"
cd "$SERVICE_NAME"

# ================= Application =================
mkdir -p Application
dotnet new classlib \
  -n "$SERVICE_NAME.Application" \
  -f "$DOTNET_VERSION" \
  -o "Application/$SERVICE_NAME.Application"
  
mkdir -p Application/$SERVICE_NAME.Application/Mappers
mkdir -p Application/$SERVICE_NAME.Application/Services
mkdir -p Application/$SERVICE_NAME.Application/DTOs
touch Application/$SERVICE_NAME.Application/Mappers/.gitkeep
touch Application/$SERVICE_NAME.Application/Services/.gitkeep
touch Application/$SERVICE_NAME.Application/DTOs/.gitkeep
# ================= Domain =================
mkdir -p Domain
dotnet new classlib \
  -n "$SERVICE_NAME.Domain" \
  -f "$DOTNET_VERSION" \
  -o "Domain/$SERVICE_NAME.Domain"

mkdir -p Domain/$SERVICE_NAME.Domain/Entities

touch Domain/$SERVICE_NAME.Domain/Entities/.gitkeep

dotnet new classlib \
  -n "$SERVICE_NAME.Domain.Interfaces" \
  -f "$DOTNET_VERSION" \
  -o "Domain/$SERVICE_NAME.Domain.Interfaces"

mkdir -p Domain/$SERVICE_NAME.Domain.Interfaces/Repositories
mkdir -p Domain/$SERVICE_NAME.Domain.Interfaces/Services

touch Domain/$SERVICE_NAME.Domain.Interfaces/Repositories/.gitkeep
touch Domain/$SERVICE_NAME.Domain.Interfaces/Services/.gitkeep

# ================= Infrastructure =================
mkdir -p Infrastructure

dotnet new classlib \
  -n "$SERVICE_NAME.Infrastructure" \
  -f "$DOTNET_VERSION" \
  -o "Infrastructure/$SERVICE_NAME.Infrastructure"

mkdir -p Infrastructure/$SERVICE_NAME.Infrastructure/Services

touch Infrastructure/$SERVICE_NAME.Infrastructure/Services/.gitkeep


dotnet new classlib \
  -n "$SERVICE_NAME.Infrastructure.EntityFramework" \
  -f "$DOTNET_VERSION" \
  -o "Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework"

mkdir -p Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Configurations
mkdir -p Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Contexts
mkdir -p Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Migrations

touch Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Configurations/.gitkeep
touch Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Contexts/.gitkeep
touch Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/Migrations/.gitkeep

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

mkdir -p Presentation/$SERVICE_NAME.Presentation.API/Controllers

touch Presentation/$SERVICE_NAME.Presentation.API/Controllers/.gitkeep

# ================= ADD TO SOLUTION =================
cd ..

dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Application/$SERVICE_NAME.Application/$SERVICE_NAME.Application.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain/$SERVICE_NAME.Domain.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain.Interfaces/$SERVICE_NAME.Domain.Interfaces.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure/$SERVICE_NAME.Infrastructure.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.EntityFramework/$SERVICE_NAME.Infrastructure.EntityFramework.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure.Repositories/$SERVICE_NAME.Infrastructure.Repositories.csproj"
dotnet sln "$SOLUTION_NAME.sln" add "$SERVICE_NAME/Presentation/$SERVICE_NAME.Presentation.API/$SERVICE_NAME.Presentation.API.csproj"

# ================= REFERENCES =================

dotnet add "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain.Interfaces" reference \
  "$SERVICE_NAME/Domain/$SERVICE_NAME.Domain"

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
  "$SERVICE_NAME/Infrastructure/$SERVICE_NAME.Infrastructure"

echo "Service $SERVICE_NAME successfully added to $SOLUTION_NAME.sln"
