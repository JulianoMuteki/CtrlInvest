#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CtrlInvest.API.StockExchange/CtrlInvest.API.StockExchange.csproj", "src/CtrlInvest.API.StockExchange/"]
COPY ["src/CtrlInvest.Services/CtrlInvest.Services.csproj", "src/CtrlInvest.Services/"]
COPY ["src/CtrlInvest.Domain/CtrlInvest.Domain.csproj", "src/CtrlInvest.Domain/"]
COPY ["src/CtrlInvest.CrossCutting/CtrlInvest.CrossCutting.csproj", "src/CtrlInvest.CrossCutting/"]
COPY ["src/CtrlInvest.Infra.Repository/CtrlInvest.Infra.Repository.csproj", "src/CtrlInvest.Infra.Repository/"]
COPY ["src/CtrlInvest.Infra.Context/CtrlInvest.Infra.Context.csproj", "src/CtrlInvest.Infra.Context/"]
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