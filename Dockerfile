# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["asp.csproj", "./"]
RUN dotnet restore "asp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "asp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "asp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "asp.dll"]