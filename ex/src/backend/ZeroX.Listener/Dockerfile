FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ZeroX.Listener/ZeroX.Listener.csproj", "ZeroX.Listener/"]
COPY ["ZeroX.DB/ZeroX.DB.csproj", "ZeroX.DB/"]
COPY ["ZeroX.Infrastructure/ZeroX.Infrastructure.csproj", "ZeroX.Infrastructure/"]
RUN dotnet restore "ZeroX.Listener/ZeroX.Listener.csproj"
COPY . .
WORKDIR "/src/ZeroX.Listener"
RUN dotnet build "ZeroX.Listener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZeroX.Listener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZeroX.Listener.dll"]
