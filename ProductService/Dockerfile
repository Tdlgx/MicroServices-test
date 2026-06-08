# 1. Ажиллуулах 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Build хийх 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductService.csproj", "."]
RUN dotnet restore "ProductService.csproj"
COPY . .
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

# 3. Publish хийх
FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]