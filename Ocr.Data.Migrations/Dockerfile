FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ocr.Data.Migrations/Ocr.Data.Migrations.csproj", "Ocr.Data.Migrations/"]
RUN dotnet restore "Ocr.Data.Migrations/Ocr.Data.Migrations.csproj"
COPY . .
WORKDIR "/src/Ocr.Data.Migrations"
RUN dotnet build "Ocr.Data.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ocr.Data.Migrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocr.Data.Migrations.dll"]
