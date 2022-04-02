#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
RUN apt-get update
RUN apt-get install -y xvfb
RUN echo "deb http://deb.debian.org/debian/ unstable main contrib non-free" >> /etc/apt/sources.list.d/debian.list
RUN apt-get update
RUN apt-get install -y libcrypt1
RUN apt-get install -y firefox

RUN apt-get update                             \
 && apt-get install -y --no-install-recommends \
    ca-certificates curl firefox-esr           \
 && rm -fr /var/lib/apt/lists/*                \
 && curl -L https://github.com/mozilla/geckodriver/releases/download/v0.30.0/geckodriver-v0.30.0-linux64.tar.gz | tar xz -C /usr/local/bin \
 && apt-get purge -y ca-certificates curl

WORKDIR /app
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CtrlInvest.Import.Dividends/CtrlInvest.Import.Dividends.csproj", "src/CtrlInvest.Import.Dividends/"]
COPY ["src/CtrlInvest.MessageBroker/CtrlInvest.MessageBroker.csproj", "src/CtrlInvest.MessageBroker/"]
COPY ["src/CtrlInvest.Domain/CtrlInvest.Domain.csproj", "src/CtrlInvest.Domain/"]
COPY ["src/CtrlInvest.CrossCutting/CtrlInvest.CrossCutting.csproj", "src/CtrlInvest.CrossCutting/"]
COPY ["src/CtrlInvest.Services/CtrlInvest.Services.csproj", "src/CtrlInvest.Services/"]
COPY ["src/CtrlInvest.Infra.Repository/CtrlInvest.Infra.Repository.csproj", "src/CtrlInvest.Infra.Repository/"]
COPY ["src/CtrlInvest.Infra.Context/CtrlInvest.Infra.Context.csproj", "src/CtrlInvest.Infra.Context/"]
COPY ["src/CtrlInvest.CrossCutting.Ioc/CtrlInvest.CrossCutting.Ioc.csproj", "src/CtrlInvest.CrossCutting.Ioc/"]
RUN dotnet restore "src/CtrlInvest.Import.Dividends/CtrlInvest.Import.Dividends.csproj"
COPY . .
WORKDIR "/src/src/CtrlInvest.Import.Dividends"
RUN dotnet build "CtrlInvest.Import.Dividends.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CtrlInvest.Import.Dividends.csproj" -c Release -o /app/publish

FROM base AS final
RUN pwd
WORKDIR ../Drivers/geckodriver-v0.30.0-win64/
COPY --from=build /src/src/CtrlInvest.Import.Dividends/Drivers/geckodriver-v0.30.0-win64/ .
RUN pwd
RUN cd

WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=base /usr/local/bin/geckodriver .
RUN export DISPLAY=:20
RUN Xvfb :20 -screen 0 1366x768x16 &

#ENTRYPOINT ["/bin/sh", "-c", "/usr/bin/xvfb-run -a $@", ""] 
#CMD ["dotnet", "CtrlInvest.Import.Dividends.dll"]

#ENTRYPOINT ["dotnet", "CtrlInvest.Import.Dividends.dll"]
