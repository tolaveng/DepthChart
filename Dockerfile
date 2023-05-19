FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5001 5000 80 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR "/source"
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Server/Server.csproj", "Server/"]

RUN dotnet restore "Server/Server.csproj"
COPY . .
RUN dotnet build "Server/Server.csproj" -c Debug -o /app

FROM build AS publish
RUN dotnet publish "Server/Server.csproj" -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Server.dll"]
