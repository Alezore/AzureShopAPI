# BUILD
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AzureShopAPI.dll"]
