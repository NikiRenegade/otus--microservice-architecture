FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserService/Presentation/UserService.Presentation.API/UserService.Presentation.API.csproj", "UserService/Presentation/UserService.Presentation.API/"]
COPY ["UserService/Application/UserService.Application/UserService.Application.csproj", "UserService/Application/UserService.Application/"]
COPY ["UserService/Domain/UserService.Domain/UserService.Domain.csproj", "UserService/Domain/UserService.Domain/"]
COPY ["UserService/Domain/UserService.Domain.Interfaces/UserService.Domain.Interfaces.csproj", "UserService/Domain/UserService.Domain.Interfaces/"]
COPY ["UserService/Infrastructure/UserService.Infrastructure.Repositories/UserService.Infrastructure.Repositories.csproj", "UserService/Infrastructure/UserService.Infrastructure.Repositories/"]
COPY ["UserService/Infrastructure/UserService.Infrastructure.EntityFramework/UserService.Infrastructure.EntityFramework.csproj", "UserService/Infrastructure/UserService.Infrastructure.EntityFramework/"]
RUN dotnet restore "UserService/Presentation/UserService.Presentation.API/UserService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/UserService/Presentation/UserService.Presentation.API"
RUN dotnet build "UserService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS api
LABEL target=api
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Presentation.API.dll"]

FROM build AS migration
LABEL target=migration
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef", "database", "update", \
 "--project", "UserService/Infrastructure/UserService.Infrastructure.EntityFramework", \
 "--startup-project", "UserService/Presentation/UserService.Presentation.API"]
