# Utiliser l’image officielle de .NET SDK pour la build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copier les fichiers .csproj et restaurer les dépendances
COPY *.sln .
COPY AzureShopAPI/*.csproj ./AzureShopAPI/
RUN dotnet restore

# Copier tout le reste et builder
COPY AzureShopAPI/. ./AzureShopAPI/
WORKDIR /app/AzureShopAPI
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/AzureShopAPI/out ./

# Lancer l'app
ENTRYPOINT ["dotnet", "AzureShopAPI.dll"]
