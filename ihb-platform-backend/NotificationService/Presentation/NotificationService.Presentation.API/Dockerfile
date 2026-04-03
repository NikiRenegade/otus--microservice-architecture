FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotificationService/Presentation/NotificationService.Presentation.API/NotificationService.Presentation.API.csproj", "NotificationService/Presentation/NotificationService.Presentation.API/"]
COPY ["NotificationService/Application/NotificationService.Application/NotificationService.Application.csproj", "NotificationService/Application/NotificationService.Application/"]
COPY ["NotificationService/Domain/NotificationService.Domain/NotificationService.Domain.csproj", "NotificationService/Domain/NotificationService.Domain/"]
COPY ["NotificationService/Domain/NotificationService.Domain.Interfaces/NotificationService.Domain.Interfaces.csproj", "NotificationService/Domain/NotificationService.Domain.Interfaces/"]
COPY ["NotificationService/Infrastructure/NotificationService.Infrastructure.Repositories/NotificationService.Infrastructure.Repositories.csproj", "NotificationService/Infrastructure/NotificationService.Infrastructure.Repositories/"]
COPY ["NotificationService/Infrastructure/NotificationService.Infrastructure.EntityFramework/NotificationService.Infrastructure.EntityFramework.csproj", "NotificationService/Infrastructure/NotificationService.Infrastructure.EntityFramework/"]
COPY ["NotificationService/Infrastructure/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService/Infrastructure/NotificationService.Infrastructure/"]
COPY ["Shared/Shared.RabbitMq/Shared.RabbitMq.csproj", "Shared/Shared.RabbitMq/"]
RUN dotnet restore "NotificationService/Presentation/NotificationService.Presentation.API/NotificationService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/NotificationService/Presentation/NotificationService.Presentation.API"
RUN dotnet build "NotificationService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotificationService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "NotificationService/Infrastructure/NotificationService.Infrastructure.EntityFramework", \
 "--startup-project", "NotificationService/Presentation/NotificationService.Presentation.API"]

