#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IDENTITY.API/IDENTITY.API.csproj", "IDENTITY.API/"]
COPY ["IDENTITY.BLL/IDENTITY.BLL.csproj", "IDENTITY.BLL/"]
COPY ["IDENTITY.DAL/IDENTITY.DAL.csproj", "IDENTITY.DAL/"]
RUN dotnet restore "./IDENTITY.API/./IDENTITY.API.csproj"
COPY . .
WORKDIR "/src/IDENTITY.API"
RUN dotnet build "./IDENTITY.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IDENTITY.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDENTITY.API.dll"]