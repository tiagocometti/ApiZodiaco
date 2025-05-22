FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080  # Render usará a porta 10000, mas o EXPOSE é apenas documentação

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiZodiaco/ApiZodiaco.csproj", "ApiZodiaco/"]
RUN dotnet restore "ApiZodiaco/ApiZodiaco.csproj"
COPY . .
WORKDIR "/src/ApiZodiaco"
RUN dotnet build "ApiZodiaco.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiZodiaco.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiZodiaco.dll"]
