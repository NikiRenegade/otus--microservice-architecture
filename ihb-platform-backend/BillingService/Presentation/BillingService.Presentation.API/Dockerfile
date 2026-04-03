FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BillingService/Presentation/BillingService.Presentation.API/BillingService.Presentation.API.csproj", "BillingService/Presentation/BillingService.Presentation.API/"]
COPY ["BillingService/Application/BillingService.Application/BillingService.Application.csproj", "BillingService/Application/BillingService.Application/"]
COPY ["BillingService/Domain/BillingService.Domain/BillingService.Domain.csproj", "BillingService/Domain/BillingService.Domain/"]
COPY ["BillingService/Domain/BillingService.Domain.Interfaces/BillingService.Domain.Interfaces.csproj", "BillingService/Domain/BillingService.Domain.Interfaces/"]
COPY ["BillingService/Infrastructure/BillingService.Infrastructure.Repositories/BillingService.Infrastructure.Repositories.csproj", "BillingService/Infrastructure/BillingService.Infrastructure.Repositories/"]
COPY ["BillingService/Infrastructure/BillingService.Infrastructure.EntityFramework/BillingService.Infrastructure.EntityFramework.csproj", "BillingService/Infrastructure/BillingService.Infrastructure.EntityFramework/"]
COPY ["BillingService/Infrastructure/BillingService.Infrastructure/BillingService.Infrastructure.csproj", "BillingService/Infrastructure/BillingService.Infrastructure/"]
COPY ["Shared/Shared.RabbitMq/Shared.RabbitMq.csproj", "Shared/Shared.RabbitMq/"]
RUN dotnet restore "BillingService/Presentation/BillingService.Presentation.API/BillingService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/BillingService/Presentation/BillingService.Presentation.API"
RUN dotnet build "BillingService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BillingService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "BillingService/Infrastructure/BillingService.Infrastructure.EntityFramework", \
 "--startup-project", "BillingService/Presentation/BillingService.Presentation.API"]

