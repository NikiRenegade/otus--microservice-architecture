FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DeliveryService/Presentation/DeliveryService.Presentation.API/DeliveryService.Presentation.API.csproj", "DeliveryService/Presentation/DeliveryService.Presentation.API/"]
COPY ["DeliveryService/Application/DeliveryService.Application/DeliveryService.Application.csproj", "DeliveryService/Application/DeliveryService.Application/"]
COPY ["DeliveryService/Domain/DeliveryService.Domain/DeliveryService.Domain.csproj", "DeliveryService/Domain/DeliveryService.Domain/"]
COPY ["DeliveryService/Domain/DeliveryService.Domain.Interfaces/DeliveryService.Domain.Interfaces.csproj", "DeliveryService/Domain/DeliveryService.Domain.Interfaces/"]
COPY ["DeliveryService/Infrastructure/DeliveryService.Infrastructure/DeliveryService.Infrastructure.csproj", "DeliveryService/Infrastructure/DeliveryService.Infrastructure/"]
COPY ["DeliveryService/Infrastructure/DeliveryService.Infrastructure.EntityFramework/DeliveryService.Infrastructure.EntityFramework.csproj", "DeliveryService/Infrastructure/DeliveryService.Infrastructure.EntityFramework/"]
COPY ["DeliveryService/Infrastructure/DeliveryService.Infrastructure.Repositories/DeliveryService.Infrastructure.Repositories.csproj", "DeliveryService/Infrastructure/DeliveryService.Infrastructure.Repositories/"]
RUN dotnet restore "DeliveryService/Presentation/DeliveryService.Presentation.API/DeliveryService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/DeliveryService/Presentation/DeliveryService.Presentation.API"
RUN dotnet build "./DeliveryService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeliveryService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "DeliveryService/Infrastructure/DeliveryService.Infrastructure.EntityFramework", \
 "--startup-project", "DeliveryService/Presentation/DeliveryService.Presentation.API"]

