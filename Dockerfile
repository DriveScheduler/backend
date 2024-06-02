# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY *.sln .
COPY src/API/*.csproj ./src/API/
COPY src/Application/*.csproj ./src/Application/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/ 
COPY test/UseCases/*.csproj ./test/UseCases/ 
COPY test/Integration/*.csproj ./test/Integration/ 
RUN dotnet restore

# Copy the rest of the application code
COPY src/API/. ./src/API/
COPY src/Application/. ./src/Application/
COPY src/Domain/. ./src/Domain/
COPY src/Infrastructure/. ./src/Infrastructure/
COPY test/UseCases/. ./test/UseCases/ 
COPY test/Integration/. ./test/Integration/ 

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY src/Infrastructure/database.db ./

# Expose the port your application will run on
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "API.dll"]