# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HRMS.Admin.API/HRMS.Admin.API.csproj", "HRMS.Admin.API/"]
COPY ["HRMS.Admin.Database/HRMS.Admin.Database.csproj", "HRMS.Admin.Database/"]
COPY ["HRMS.Admin.Entities/HRMS.Admin.Entities.csproj", "HRMS.Admin.Entities/"]
COPY ["HRMS.Admin.Service/HRMS.Admin.Service.csproj", "HRMS.Admin.Service/"]
COPY ["HRMS.Admin.Infrastructure/HRMS.Admin.Infrastructure.csproj", "HRMS.Admin.Infrastructure/"]
COPY ["HRMS.Admin.Types/HRMS.Admin.Types.csproj", "HRMS.Admin.Types/"]
RUN dotnet restore "./HRMS.Admin.API/HRMS.Admin.API.csproj"
COPY . .
WORKDIR "/src/HRMS.Admin.API"
RUN dotnet build "./HRMS.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HRMS.Admin.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRMS.Admin.API.dll"]