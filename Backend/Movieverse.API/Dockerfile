FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Movieverse.API/Movieverse.API.csproj", "Movieverse.API/"]
COPY ["Movieverse.Application/Movieverse.Application.csproj", "Movieverse.Application/"]
COPY ["Movieverse.Contracts/Movieverse.Contracts.csproj", "Movieverse.Contracts/"]
COPY ["Movieverse.Domain/Movieverse.Domain.csproj", "Movieverse.Domain/"]
COPY ["Movieverse.Infrastructure/Movieverse.Infrastructure.csproj", "Movieverse.Infrastructure/"]
RUN dotnet restore "Movieverse.API/Movieverse.API.csproj"
COPY . .
WORKDIR "/src/Movieverse.API"
RUN dotnet build "Movieverse.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movieverse.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movieverse.API.dll"]
