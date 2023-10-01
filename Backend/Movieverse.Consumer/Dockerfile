FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Movieverse.Consumer/Movieverse.Consumer.csproj", "Movieverse.Consumer/"]
COPY ["Movieverse.Contracts/Movieverse.Contracts.csproj", "Movieverse.Contracts/"]
COPY ["Movieverse.Domain/Movieverse.Domain.csproj", "Movieverse.Domain/"]
RUN dotnet restore "Movieverse.Consumer/Movieverse.Consumer.csproj"
COPY . .
WORKDIR "/src/Movieverse.Consumer"
RUN dotnet build "Movieverse.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movieverse.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movieverse.Consumer.dll"]
