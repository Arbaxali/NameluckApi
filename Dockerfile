# Use the official .NET 6.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the solution file and the entire source code to the container
COPY *.sln ./
COPY ./NameLuckMapper ./NameLuckMapper

# Restore the dependencies
RUN dotnet restore

# Build and publish the project
RUN dotnet publish -c Release -o out

# Use the official .NET 6.0 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy the published output to the runtime container
COPY --from=build-env /app/out .

# Expose port 80
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "NameLuckMapper.dll"]
