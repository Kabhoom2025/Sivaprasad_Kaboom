# Step 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore
COPY . .
RUN dotnet restore

# Build and publish the release
RUN dotnet publish -c Release -o /app/publish

# Step 2: Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy build output
COPY --from=build /app/publish .

# Expose port 80 for Render
EXPOSE 80

# Start your app (update DLL name if needed)
ENTRYPOINT ["dotnet", "Kaboom.dll"]
