FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GatewayService/GatewayService.API/GatewayService.API.csproj", "GatewayService/GatewayService.API/"]
RUN dotnet restore "GatewayService/GatewayService.API/GatewayService.API.csproj"
COPY . .
WORKDIR "/src/GatewayService/GatewayService.API"
RUN dotnet build "GatewayService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GatewayService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GatewayService.API.dll"]
