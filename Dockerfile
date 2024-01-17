#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 5678

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SwiftLink.Presentation/SwiftLink.Presentation.csproj", "src/SwiftLink.Presentation/"]
COPY ["src/SwiftLink.Infrastructure/SwiftLink.Infrastructure.csproj", "src/SwiftLink.Infrastructure/"]
COPY ["src/SwiftLink.Application/SwiftLink.Application.csproj", "src/SwiftLink.Application/"]
COPY ["src/SwiftLink.Domain/SwiftLink.Domain.csproj", "src/SwiftLink.Domain/"]
COPY ["src/SwiftLink.Shared/SwiftLink.Shared.csproj", "src/SwiftLink.Shared/"]
RUN dotnet restore "./src/SwiftLink.Presentation/./SwiftLink.Presentation.csproj"
COPY . .
WORKDIR "/src/src/SwiftLink.Presentation"
RUN dotnet build "./SwiftLink.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SwiftLink.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SwiftLink.Presentation.dll"]