# Utilise l'image officielle .NET 9 SDK pour build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copie les fichiers du projet
COPY *.csproj ./
RUN dotnet restore

# Copie tout le reste
COPY . ./
RUN dotnet publish -c Release -o out

# Utilise une image runtime .NET pour exécuter
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Expose le port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AzureShopAPI.dll"]
