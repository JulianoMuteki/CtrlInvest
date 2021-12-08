#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CtrlInvest.API.StockExchange/CtrlInvest.API.StockExchange.csproj", "src/CtrlInvest.API.StockExchange/"]
RUN dotnet restore "src/CtrlInvest.API.StockExchange/CtrlInvest.API.StockExchange.csproj"
COPY . .
WORKDIR "/src/src/CtrlInvest.API.StockExchange"
RUN dotnet build "CtrlInvest.API.StockExchange.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CtrlInvest.API.StockExchange.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CtrlInvest.API.StockExchange.dll"]