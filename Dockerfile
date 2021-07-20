FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MonitoringExample.Api/MonitoringExample.Api.csproj", "MonitoringExample.Api/"]
RUN dotnet restore "MonitoringExample.Api/MonitoringExample.Api.csproj" --disable-parallel

COPY . .
WORKDIR "/src/MonitoringExample.Api"
RUN dotnet build "MonitoringExample.Api.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/MonitoringExample.Api"
RUN dotnet publish "MonitoringExample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitoringExample.Api.dll"]