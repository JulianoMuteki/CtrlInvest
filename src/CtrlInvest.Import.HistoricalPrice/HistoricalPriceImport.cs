using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class HistoricalPriceImport
    {
        ITicketAppService ticketAppService;
        IList<DownloadHistoricalPriceModel> downloadHistoricalPriceModels;
        public List<Task> TaskList = new List<Task>();

        Broker broker = new Broker();

        // Constructor
        public HistoricalPriceImport(ServiceProvider serviceProvider)
        {            
            this.ticketAppService = serviceProvider.GetService<ITicketAppService>();
            this.downloadHistoricalPriceModels = new List<DownloadHistoricalPriceModel>();
        }

        public void DoImportOperation()
        {
            CreateHistoricalPricesToImport();
            foreach (var downloadHistoricalPriceModel in downloadHistoricalPriceModels)
            {
                var historicalPriceList = DownloadHistorical(downloadHistoricalPriceModel.TicketCode, downloadHistoricalPriceModel.DateStart,
                                                             downloadHistoricalPriceModel.DateEnd);

                SendtoBroker(historicalPriceList);
            }
            Task.WaitAll(TaskList.ToArray());
        }

        private void SendtoBroker(string historicalPriceList)
        {
            TaskList.Add(Task.Factory.StartNew(() => broker.SendMessageToRabbitMQ(historicalPriceList)));
        }

        private string DownloadHistorical(string ticker, DateTime dtStart, DateTime dtEnd)
        {
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
            var ticketSyncs = this.ticketAppService.GetAllTicketsSyncs();
            foreach (var ticketSync in ticketSyncs)
            {
                Ticket ticket = this.ticketAppService.GetById(ticketSync.TickerID);
                // Get tha latest historical
                var latestHistorical = this.ticketAppService.GetLatestHistoricalByTicker(ticket.Ticker);
                // Check if is needed to get more historical
                if (latestHistorical == null)
                {
                    downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket.Ticker, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date));
                }
                else if (latestHistorical.Date < DateTime.Now.Date.AddDays(-1))
                {
                    downloadHistoricalPriceModels.Add(new DownloadHistoricalPriceModel(ticket.Ticker, latestHistorical.Date.AddDays(1), DateTime.Now.Date));
                }
            }
        }

        private long ConvertToTimestamp(DateTime value)
        {
            var UnixTimeStamp = value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long timeStamp = Convert.ToInt64(UnixTimeStamp);
            return timeStamp;
        }

    }
}
