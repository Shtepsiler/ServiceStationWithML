#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JOBS.API/JOBS.API.csproj", "JOBS.API/"]
COPY ["JOBS.BLL/JOBS.BLL.csproj", "JOBS.BLL/"]
COPY ["JOBS.DAL/JOBS.DAL.csproj", "JOBS.DAL/"]
RUN dotnet restore "./JOBS.API/./JOBS.API.csproj"
COPY . .
WORKDIR "/src/JOBS.API"

RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet build "./JOBS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JOBS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JOBS.API.dll"]