#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CtrlInvest.Receive.HistoricalData/CtrlInvest.Receive.HistoricalData.csproj", "src/CtrlInvest.Receive.HistoricalData/"]
COPY ["src/CtrlInvest.MessageBroker/CtrlInvest.MessageBroker.csproj", "src/CtrlInvest.MessageBroker/"]
COPY ["src/CtrlInvest.Domain/CtrlInvest.Domain.csproj", "src/CtrlInvest.Domain/"]
COPY ["src/CtrlInvest.CrossCutting/CtrlInvest.CrossCutting.csproj", "src/CtrlInvest.CrossCutting/"]
COPY ["src/CtrlInvest.Services/CtrlInvest.Services.csproj", "src/CtrlInvest.Services/"]
COPY ["src/CtrlInvest.Infra.Repository/CtrlInvest.Infra.Repository.csproj", "src/CtrlInvest.Infra.Repository/"]
COPY ["src/CtrlInvest.Infra.Context/CtrlInvest.Infra.Context.csproj", "src/CtrlInvest.Infra.Context/"]
COPY ["src/CtrlInvest.Import.HistoricalPrice/CtrlInvest.Import.HistoricalPrice.csproj", "src/CtrlInvest.Import.HistoricalPrice/"]
RUN dotnet restore "src/CtrlInvest.Receive.HistoricalData/CtrlInvest.Receive.HistoricalData.csproj"
COPY . .
WORKDIR "/src/src/CtrlInvest.Receive.HistoricalData"
RUN dotnet build "CtrlInvest.Receive.HistoricalData.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CtrlInvest.Receive.HistoricalData.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CtrlInvest.Receive.HistoricalData.dll"]