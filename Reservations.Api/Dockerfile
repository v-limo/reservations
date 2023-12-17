FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5165

ENV ASPNETCORE_URLS=http://+:5165

USER root

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Reservations.Api/Reservations.Api.csproj", "Reservations.Api/"]
RUN dotnet restore "Reservations.Api/Reservations.Api.csproj"
COPY . .
WORKDIR "/src/Reservations.Api"
RUN dotnet build "Reservations.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Reservations.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Reservations.Api/Migrations", "Migrations/"]
COPY ["Reservations.Api/reservationDB.db", "data/"]
ENTRYPOINT ["dotnet", "Reservations.Api.dll"]
