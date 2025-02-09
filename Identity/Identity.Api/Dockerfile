FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5067

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Identity/Identity.Api/Identity.Api.csproj", "Identity.Api/"]
COPY ["Identity/Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Identity/Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["../Core/Core.Domain/Core.Domain.csproj", "../Core/Core.Domain/"]
COPY ["../Core/Core/Core.csproj", "../Core/Core/"]
COPY ["../Shared/Shared/Shared.csproj", "../Shared/Shared/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/"]
COPY ["Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]

RUN dotnet restore "Identity.Api/Identity.Api.csproj"

COPY . .
WORKDIR "/src/Identity/Identity.Api"
RUN dotnet publish "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

#COPY Identity/Identity.Api/appsettings.json /app/appsettings.json
#COPY Identity/Identity.Api/appsettings.Development.json /app/appsettings.Development.json
ENTRYPOINT ["dotnet", "Identity.Api.dll"]