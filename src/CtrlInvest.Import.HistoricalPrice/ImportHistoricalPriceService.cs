using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.MessageBroker.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class ImportHistoricalPriceService : IImportHistoricalPriceService
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly ILogger<Worker> _logger;
        private IList<DownloadHistoricalPriceModel> _downloadHistoricalPriceModels;
        public List<Task> TaskList = new List<Task>();
        private IMessageBroker _messageBroker;

        // Constructor
        public ImportHistoricalPriceService(ILogger<Worker> logger, ITicketAppService ticketAppService, IMessageBroker messageBroker)
        {
            _logger = logger;
            _ticketAppService = ticketAppService;
            _messageBroker = messageBroker;
            _downloadHistoricalPriceModels = new List<DownloadHistoricalPriceModel>();
        }

        public void DoImportOperation()
        {
            _logger.LogInformation($"Start Import Operation");
            _messageBroker.Init();

            CreateHistoricalPricesToImport();
            foreach (var downloadHistoricalPriceModel in _downloadHistoricalPriceModels)
            {
                var historicalPriceList = DownloadHistorical(downloadHistoricalPriceModel.ticket.Ticker, downloadHistoricalPriceModel.DateStart,
                                                             downloadHistoricalPriceModel.DateEnd); ;

                SendtoBroker(historicalPriceList, downloadHistoricalPriceModel.ticket);
            }
            _downloadHistoricalPriceModels = new List<DownloadHistoricalPriceModel>();
            _messageBroker.Dispose();
        }

        private void SendtoBroker(string historicalPriceList, Ticket ticket)
        {
            try
            {
                _logger.LogInformation("Start send messega to Broker");
                using var sr = new StringReader(historicalPriceList);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    PackageMessage packageMessage = new PackageMessage()
                    {
                        Message = line,
                        TicketCode = ticket.Ticker,
                        TicketID = ticket.Id
                    };

                    string message = JsonSerialize.JsonSerializer<PackageMessage>(packageMessage);
                    // _logger.LogInformation($"Send to Broker message: {message}");
                    _messageBroker.SendMessageToRabbitMQ(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private string DownloadHistorical(string ticker, DateTime dtStart, DateTime dtEnd)
        {
            _logger.LogInformation($"Starting donwload from server ...");
            string allResults = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("text/csv"));

                    //var dttest  = dtStart.AddHours(21);
                    //DateTime originDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    //TimeSpan diff = dttest.ToUniversalTime() - originDate;
                    //var datestamp = (long)Math.Floor(diff.TotalSeconds);

                    long dt1 = ConvertToTimestamp(dtStart); // will get always datestart + 1 day
                    long dt2 = ConvertToTimestamp(dtEnd);  // will get always dateend - 1 day
                    string uri = string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true",
                            ticker, dt1, dt2);
                    HttpResponseMessage response = client.GetAsync(uri).Result;
                    response.EnsureSuccessStatusCode();
                    allResults =
                        response.Content.ReadAsStringAsync().Result;
                    response.Dispose();                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return allResults;
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
                    _downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket, ticketSync.DateStart, DateTime.Now.Date));
                }
                else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                {
                    _downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket, latestHistorical.Date, DateTime.Now.Date));
                }
            }

            _logger.LogInformation($"Tickets count to import: {_downloadHistoricalPriceModels.Count}");
        }

        private long ConvertToTimestamp(DateTime value)
        {
            DateTime originDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = value.AddHours(21).ToUniversalTime() - originDate;
            return (long)Math.Floor(diff.TotalSeconds);
        }

    }
}
