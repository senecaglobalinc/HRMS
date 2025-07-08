# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HRMS.Employee/HRMS.Employee.API.csproj", "HRMS.Employee/"]
COPY ["HRMS.Employee.Database/HRMS.Employee.Database.csproj", "HRMS.Employee.Database/"]
COPY ["HRMS.Employee.Entities/HRMS.Employee.Entities.csproj", "HRMS.Employee.Entities/"]
COPY ["HRMS.Employee.Service/HRMS.Employee.Service.csproj", "HRMS.Employee.Service/"]
COPY ["HRMS.Employee.Infrastructure/HRMS.Employee.Infrastructure.csproj", "HRMS.Employee.Infrastructure/"]
COPY ["HRMS.Employee.Types/HRMS.Employee.Types.csproj", "HRMS.Employee.Types/"]
COPY ["entrypoint.sh", "entrypoint.sh"]
RUN dotnet restore "./HRMS.Employee/HRMS.Employee.API.csproj"
COPY . .
WORKDIR "/src/HRMS.Employee"
RUN dotnet build "./HRMS.Employee.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HRMS.Employee.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh /app/entrypoint.sh
 
# Make sure the script is executable
RUN chmod +x /app/entrypoint.sh

# Set the script as the entrypoint
CMD ["entrypoint.sh"]
 
# Default command to run on container start
ENTRYPOINT ["dotnet", "HRMS.Employee.API.dll"]