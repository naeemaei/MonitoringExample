FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
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

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "MonitoringExample.Api.dll"]
