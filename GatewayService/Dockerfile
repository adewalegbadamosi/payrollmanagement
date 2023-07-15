
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# WORKDIR /app

# EXPOSE 80
# EXPOSE 443

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./
# RUN dotnet publish  "GatewayService.csproj" -c Release -o out

# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT [ "dotnet", "Gateway.dll" ]


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS build
WORKDIR /src
COPY ["GatewayService/GatewayService.csproj", "GatewayService/"]
RUN dotnet restore "GatewayService/GatewayService.csproj"
COPY . .
WORKDIR "/src/GatewayService"
# ENTRYPOINT ["dotnet", "watch", "run"] // for live reload
RUN dotnet build "GatewayService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GatewayService.csproj" -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]