FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DemoWebShop.csproj", "DemoWebShop/"]
RUN dotnet restore "DemoWebShop/DemoWebShop.csproj"


WORKDIR "/src/DemoWebShop"
COPY . .

RUN dotnet build "DemoWebShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoWebShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebShop.dll"]