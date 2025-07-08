# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HRMS.Project/HRMS.Project.API.csproj", "HRMS.Project/"]
COPY ["HRMS.Project.Database/HRMS.Project.Database.csproj", "HRMS.Project.Database/"]
COPY ["HRMS.Project.Entities/HRMS.Project.Entities.csproj", "HRMS.Project.Entities/"]
COPY ["HRMS.Project.Infrastructure/HRMS.Project.Infrastructure.csproj", "HRMS.Project.Infrastructure/"]
COPY ["HRMS.Project.Service/HRMS.Project.Service.csproj", "HRMS.Project.Service/"]
COPY ["HRMS.Project.Types/HRMS.Project.Types.csproj", "HRMS.Project.Types/"]
RUN dotnet restore "./HRMS.Project/HRMS.Project.API.csproj"
COPY . .
WORKDIR "/src/HRMS.Project"
RUN dotnet build "./HRMS.Project.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HRMS.Project.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRMS.Project.API.dll"]