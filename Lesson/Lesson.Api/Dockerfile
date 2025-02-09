FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5060

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/Lesson/Lesson.Api/Lesson.Api.csproj", "Lesson.Api/"]
COPY ["/Lesson/Lesson.Application/Lesson.Application.csproj", "Lesson.Application/"]
COPY ["/Lesson/Lesson.Domain/Lesson.Domain.csproj", "Lesson.Domain/"]
COPY ["../Core/Core.Domain/Core.Domain.csproj", "../Core/Core.Domain/"]
COPY ["../Core/Core/Core.csproj", "../Core/Core/"]
COPY ["../Shared/Shared/Shared.csproj", "../Shared/Shared/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/"]
COPY ["Lesson/Lesson.Infrastructure/Lesson.Infrastructure.csproj", "Lesson.Infrastructure/"]

RUN dotnet restore "Lesson.Api/Lesson.Api.csproj"

COPY . .
WORKDIR "/src/Lesson/Lesson.Api"
RUN dotnet build "Lesson.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Lesson.Api.dll"]
