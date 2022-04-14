

docker build -t receive-historical-data -f ./src/CtrlInvest.Receive.HistoricalData/Dockerfile .
#docker run -it --rm -e DOTNET_ENVIRONMENT='Development' -p 8000:80 receive-historical-data:latest