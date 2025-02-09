FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5801

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/Gateway/Gateway/Gateway.csproj", "Gateway/"]
COPY ["../Core/Core.Domain/Core.Domain.csproj", "../Core/Core.Domain/"]
COPY ["../Core/Core/Core.csproj", "../Core/Core/"]
COPY ["../Shared/Shared/Shared.csproj", "../Shared/Shared/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Gateway/Gateway.csproj"

COPY . .
WORKDIR "/src/Gateway/Gateway"
RUN dotnet publish "Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

COPY Gateway/Gateway/ocelot.json /app/ocelot.json

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]
