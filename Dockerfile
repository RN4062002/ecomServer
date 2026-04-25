# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["ecomServer.csproj", "."]
RUN dotnet restore "./ecomServer.csproj"

# Copy the rest of the code and build the app
COPY . .
RUN dotnet build "ecomServer.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "ecomServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render provides PORT at runtime. Default to 10000 for local container runs.
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "export ASPNETCORE_HTTP_PORTS=; export ASPNETCORE_URLS=http://+:${PORT:-10000}; exec dotnet ecomServer.dll"]
