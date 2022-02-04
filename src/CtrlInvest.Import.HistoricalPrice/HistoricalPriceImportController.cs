using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class HistoricalPriceImportController: IHistoricalPriceImportController
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly ILogger<Worker> _logger;
        private IList<DownloadHistoricalPriceModel> _downloadHistoricalPriceModels;
        public List<Task> TaskList = new List<Task>();
        private Broker broker = new Broker();

        // Constructor
        public HistoricalPriceImportController(ILogger<Worker> logger, ITicketAppService ticketAppService)
        {
            _logger = logger;
            _ticketAppService = ticketAppService;
            _downloadHistoricalPriceModels = new List<DownloadHistoricalPriceModel>();
        }

        public void DoImportOperation()
        {
            _logger.LogInformation($"Start Import Operation");
            CreateHistoricalPricesToImport();
            foreach (var downloadHistoricalPriceModel in _downloadHistoricalPriceModels)
            {
                var historicalPriceList = DownloadHistorical(downloadHistoricalPriceModel.TicketCode, downloadHistoricalPriceModel.DateStart,
                                                             downloadHistoricalPriceModel.DateEnd);

                SendtoBroker(historicalPriceList);
            }
            // Task.WaitAll(TaskList.ToArray());
            _downloadHistoricalPriceModels = new List<DownloadHistoricalPriceModel>();
        }

        private void SendtoBroker(string historicalPriceList)
        {
            _logger.LogInformation($"Send to Broker");
            // TaskList.Add(Task.Factory.StartNew(() => broker.SendMessageToRabbitMQ(historicalPriceList)));
            broker.SendMessageToRabbitMQ(historicalPriceList);
        }

        private string DownloadHistorical(string ticker, DateTime dtStart, DateTime dtEnd)
        {
            _logger.LogInformation($"Starting donwload from server ...");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/csv"));

                long dt1 = ConvertToTimestamp(dtStart);
                long dt2 = ConvertToTimestamp(dtEnd);

                HttpResponseMessage response = client.GetAsync(
                    string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true",
                        ticker, dt1, dt2)).Result;
                response.EnsureSuccessStatusCode();
                string allResults =
                    response.Content.ReadAsStringAsync().Result;

                return allResults;
            }
        }

        private void CreateHistoricalPricesToImport()
        {
            // Get Tickets with sync enable
            var ticketSyncs = _ticketAppService.GetAllTicketsSyncs();
            foreach (var ticketSync in ticketSyncs)
            {
                Ticket ticket = _ticketAppService.GetById(ticketSync.TickerID);
                // Get tha latest historical
                var latestHistorical = _ticketAppService.GetLatestHistoricalByTicker(ticket.Ticker);
                // Check if is needed to get more historical
                if (latestHistorical == null)
                {
                    _downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date));
                }
                else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                {
                    _downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket.Ticker, latestHistorical.Date.AddDays(1), DateTime.Now.Date));
                }
            }

            _logger.LogInformation($"Tickets count to import: {_downloadHistoricalPriceModels.Count}");
        }

        private long ConvertToTimestamp(DateTime value)
        {
            var UnixTimeStamp = value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long timeStamp = Convert.ToInt64(UnixTimeStamp);
            return timeStamp;
        }

    }
}
