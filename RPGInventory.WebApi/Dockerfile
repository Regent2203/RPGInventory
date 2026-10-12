FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

RUN mkdir -p /app/data


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RPGInventory.WebApi/RPGInventory.WebApi.csproj", "RPGInventory.WebApi/"]
COPY ["RPGInventory.Application/RPGInventory.Application.csproj", "RPGInventory.Application/"]
COPY ["RPGInventory.Infrastructure/RPGInventory.Infrastructure.csproj", "RPGInventory.Infrastructure/"]
COPY ["RPGInventory.Domain/RPGInventory.Domain.csproj", "RPGInventory.Domain/"]

RUN dotnet restore "RPGInventory.WebApi/RPGInventory.WebApi.csproj"

COPY . .
WORKDIR "/src/RPGInventory.WebApi"
RUN dotnet build "RPGInventory.WebApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "RPGInventory.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RPGInventory.WebApi.dll"]