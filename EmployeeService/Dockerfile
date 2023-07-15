# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# WORKDIR /app

# EXPOSE 80
# EXPOSE 443

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./
# RUN dotnet publish "EmployeeService.csproj" -c Release -o out

# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /app
# COPY --from=build-env /app/out .
# ENTRYPOINT [ "dotnet", "Employee.dll" ]


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS build
WORKDIR /src
COPY ["EmployeeService/EmployeeService.csproj", "EmployeeService/"]
RUN dotnet restore "EmployeeService/EmployeeService.csproj"
COPY . .
WORKDIR "/src/EmployeeService"
# ENTRYPOINT ["dotnet", "watch", "run"] // Live reload

RUN dotnet build "EmployeeService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeService.csproj" -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.dll"]

