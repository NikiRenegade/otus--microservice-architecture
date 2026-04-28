FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderService/Presentation/OrderService.Presentation.API/OrderService.Presentation.API.csproj", "OrderService/Presentation/OrderService.Presentation.API/"]
COPY ["OrderService/Application/OrderService.Application/OrderService.Application.csproj", "OrderService/Application/OrderService.Application/"]
COPY ["OrderService/Domain/OrderService.Domain/OrderService.Domain.csproj", "OrderService/Domain/OrderService.Domain/"]
COPY ["OrderService/Domain/OrderService.Domain.Interfaces/OrderService.Domain.Interfaces.csproj", "OrderService/Domain/OrderService.Domain.Interfaces/"]
COPY ["OrderService/Infrastructure/OrderService.Infrastructure.Repositories/OrderService.Infrastructure.Repositories.csproj", "OrderService/Infrastructure/OrderService.Infrastructure.Repositories/"]
COPY ["OrderService/Infrastructure/OrderService.Infrastructure.EntityFramework/OrderService.Infrastructure.EntityFramework.csproj", "OrderService/Infrastructure/OrderService.Infrastructure.EntityFramework/"]
COPY ["OrderService/Infrastructure/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService/Infrastructure/OrderService.Infrastructure/"]
COPY ["Shared/Shared.ServiceToken/Shared.ServiceToken.csproj", "Shared/Shared.ServiceToken/"]
COPY ["Shared/Shared.RabbitMq/Shared.RabbitMq.csproj", "Shared/Shared.RabbitMq/"]
RUN dotnet restore "OrderService/Presentation/OrderService.Presentation.API/OrderService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/OrderService/Presentation/OrderService.Presentation.API"
RUN dotnet build "OrderService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "OrderService/Infrastructure/OrderService.Infrastructure.EntityFramework", \
 "--startup-project", "OrderService/Presentation/OrderService.Presentation.API"]
