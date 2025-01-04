FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Ocr.Api/Ocr.Api.csproj", "Ocr.Api/"]

# Class libraries
COPY ["Ocr.Data/Ocr.Data.csproj", "Ocr.Data/"]
COPY ["Ocr.Services/Ocr.Services.csproj", "Ocr.Services/"]

RUN dotnet restore "Ocr.Api/Ocr.Api.csproj"
COPY . .
WORKDIR "/src/Ocr.Api"
RUN dotnet build "Ocr.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ocr.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocr.Api.dll"]