FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InventoryService/Presentation/InventoryService.Presentation.API/InventoryService.Presentation.API.csproj", "InventoryService/Presentation/InventoryService.Presentation.API/"]
COPY ["InventoryService/Application/InventoryService.Application/InventoryService.Application.csproj", "InventoryService/Application/InventoryService.Application/"]
COPY ["InventoryService/Domain/InventoryService.Domain/InventoryService.Domain.csproj", "InventoryService/Domain/InventoryService.Domain/"]
COPY ["InventoryService/Domain/InventoryService.Domain.Interfaces/InventoryService.Domain.Interfaces.csproj", "InventoryService/Domain/InventoryService.Domain.Interfaces/"]
COPY ["InventoryService/Infrastructure/InventoryService.Infrastructure.EntityFramework/InventoryService.Infrastructure.EntityFramework.csproj", "InventoryService/Infrastructure/InventoryService.Infrastructure.EntityFramework/"]
COPY ["InventoryService/Infrastructure/InventoryService.Infrastructure.Repositories/InventoryService.Infrastructure.Repositories.csproj", "InventoryService/Infrastructure/InventoryService.Infrastructure.Repositories/"]
COPY ["InventoryService/Infrastructure/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "InventoryService/Infrastructure/InventoryService.Infrastructure/"]
COPY ["Shared/Shared.RabbitMq/Shared.RabbitMq.csproj", "Shared/Shared.RabbitMq/"]
RUN dotnet restore "InventoryService/Presentation/InventoryService.Presentation.API/InventoryService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/InventoryService/Presentation/InventoryService.Presentation.API"
RUN dotnet build "./InventoryService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventoryService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "InventoryService/Infrastructure/InventoryService.Infrastructure.EntityFramework", \
 "--startup-project", "InventoryService/Presentation/InventoryService.Presentation.API"]
