FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OCR/OCR.csproj", "OCR/"]
RUN dotnet restore "OCR/OCR.csproj"
COPY . .
WORKDIR "/src/OCR"
RUN dotnet build "OCR.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OCR.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Migrations stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrations
WORKDIR /app
COPY --from=publish /app/publish . 
# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef --version 9
# Add the global tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"
# Run database migrations
ENTRYPOINT ["dotnet-ef", "database", "update"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OCR.dll"]