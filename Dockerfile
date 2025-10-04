# Stage 1 - Build and publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Fcg.Game.sln", "./"]
COPY ["Fcg.Game.Api/Fcg.Game.Api.csproj", "Fcg.Game.Api/"]
COPY ["Fcg.Game.Application/Fcg.Game.Application.csproj", "Fcg.Game.Application/"]
COPY ["Fcg.Game.Domain/Fcg.Game.Domain.csproj", "Fcg.Game.Domain/"]
COPY ["Fcg.Game.Infrastructure/Fcg.Game.Infrastructure.csproj", "Fcg.Game.Infrastructure/"]

RUN dotnet restore "Fcg.Game.Api/Fcg.Game.Api.csproj"

COPY . .

WORKDIR "/src/Fcg.Game.Api"
RUN dotnet publish "Fcg.Game.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# Stage 2 - Start api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5265

EXPOSE 5265

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Fcg.Game.Api.dll"]