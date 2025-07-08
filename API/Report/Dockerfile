# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HRMS.Report.API/HRMS.Report.API.csproj", "HRMS.Report.API/"]
COPY ["HRMS.Report.Infrastructure/HRMS.Report.Infrastructure.csproj", "HRMS.Report.Infrastructure/"]
COPY ["HRMS.Report.Service/HRMS.Report.Service.csproj", "HRMS.Report.Service/"]
COPY ["HRMS.Report.Types/HRMS.Report.Types.csproj", "HRMS.Report.Types/"]
RUN dotnet restore "./HRMS.Report.API/HRMS.Report.API.csproj"
COPY . .
WORKDIR "/src/HRMS.Report.API"
RUN dotnet build "./HRMS.Report.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HRMS.Report.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRMS.Report.API.dll"]