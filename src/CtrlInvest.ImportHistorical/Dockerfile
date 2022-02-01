#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CtrlInvest.ImportHistorical/CtrlInvest.ImportHistorical.csproj", "src/CtrlInvest.ImportHistorical/"]
COPY ["src/CtrlInvest.Infra.Context/CtrlInvest.Infra.Context.csproj", "src/CtrlInvest.Infra.Context/"]
COPY ["src/CtrlInvest.Domain/CtrlInvest.Domain.csproj", "src/CtrlInvest.Domain/"]
COPY ["src/CtrlInvest.CrossCutting/CtrlInvest.CrossCutting.csproj", "src/CtrlInvest.CrossCutting/"]
COPY ["src/CtrlInvest.CrossCutting.Ioc/CtrlInvest.CrossCutting.Ioc.csproj", "src/CtrlInvest.CrossCutting.Ioc/"]
COPY ["src/CtrlInvest.Services/CtrlInvest.Services.csproj", "src/CtrlInvest.Services/"]
COPY ["src/CtrlInvest.Infra.Repository/CtrlInvest.Infra.Repository.csproj", "src/CtrlInvest.Infra.Repository/"]
RUN dotnet restore "src/CtrlInvest.ImportHistorical/CtrlInvest.ImportHistorical.csproj"
COPY . .
WORKDIR "/src/src/CtrlInvest.ImportHistorical"
RUN dotnet build "CtrlInvest.ImportHistorical.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CtrlInvest.ImportHistorical.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CtrlInvest.ImportHistorical.dll"]