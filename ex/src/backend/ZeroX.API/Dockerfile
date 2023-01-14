FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ZeroX.API/ZeroX.API.csproj", "ZeroX.API/"]
COPY ["ZeroX.DB/ZeroX.DB.csproj", "ZeroX.DB/"]
COPY ["ZeroX.Infrastructure/ZeroX.Infrastructure.csproj", "ZeroX.Infrastructure/"]
RUN dotnet restore "ZeroX.API/ZeroX.API.csproj"
COPY . .
WORKDIR "/src/ZeroX.API"
RUN dotnet build "ZeroX.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZeroX.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZeroX.API.dll"]
