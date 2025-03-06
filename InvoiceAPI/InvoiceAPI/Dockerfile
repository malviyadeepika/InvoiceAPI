# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj and restore dependencies (improves caching)
COPY ["InvoiceAPI/InvoiceAPI.csproj", "InvoiceAPI/"]
RUN dotnet restore "./InvoiceAPI/InvoiceAPI.csproj"

# Copy the entire project and build
COPY . .
WORKDIR "/src/InvoiceAPI"
RUN dotnet build "./InvoiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InvoiceAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final stage: Run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoiceAPI.dll"]
