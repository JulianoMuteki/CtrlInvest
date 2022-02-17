

using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.MessageBroker.Common;
using System;
using System.Collections.Generic;

namespace CtrlInvest.Import.Dividends.Services
{
    public class EarningService : IEarningService
    {
        private readonly ITicketAppService _ticketAppService;
        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
        public EarningService(ITicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }
        public void DoImportOperation()
        {
           // _logger.LogInformation($"Start Import Operation");
            // _messageBroker.Init();

            IList<DownloadHistoricalValueModel<Ticket>> downloadHistoricalValueModels = GetListHistoricalEarningsToImport();
            foreach (var downloadHistoricalValueModel in downloadHistoricalValueModels)
            {
                var historicalEarningList = DownloadHistoricalEarningFromYahoo(downloadHistoricalValueModel.ticket.Ticker, downloadHistoricalValueModel.DateStart,
                                                             downloadHistoricalValueModel.DateEnd);

                //raise event >= historicalEarningList
                // ProcessCompleted?.Invoke(historicalEarningList, downloadHistoricalValueModel.ticket.Ticker);
                OnThresholdReached(historicalEarningList, downloadHistoricalValueModel.ticket);
            }
        }

        protected virtual void OnThresholdReached(string historicalEarningList, Ticket ticket)
        {
            ThresholdReachedEventArgs args = new()
            {
                HistoricalEarningList = historicalEarningList,
                Ticket = ticket
            };

            ThresholdReached?.Invoke(this, args);

            //EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
            //if (handler != null)
            //{
            //    handler(this, args);
            //}
        }

        private IList<DownloadHistoricalValueModel<Ticket>> GetListHistoricalEarningsToImport()
        {
            IList<DownloadHistoricalValueModel<Ticket>> downloadHistoricalEarningsModels = new List<DownloadHistoricalValueModel<Ticket>>();
            // Get Tickets with sync enable
            var ticketSyncs = _ticketAppService.GetAllTicketsSyncs();
            foreach (var ticketSync in ticketSyncs)
            {
                Ticket ticket = _ticketAppService.GetById(ticketSync.TickerID);

                // Get tha latest earning
                var latestEarning = _ticketAppService.GetLastEarningByTicker(ticket.Id);
                // Check if is needed to get more historical
                if (latestEarning == null)
                {
                    downloadHistoricalEarningsModels.Add(new DownloadHistoricalValueModel<Ticket>(ticket, ticketSync.DateStart, DateTime.Now.AddDays(-1).Date));
                }
                else if (latestEarning.DateWith < DateTime.Now.Date.AddDays(-1))
                {
                    downloadHistoricalEarningsModels.Add(new DownloadHistoricalValueModel<Ticket>(ticket, latestEarning.DateWith.AddDays(1), DateTime.Now.Date));
                }
            }

            return downloadHistoricalEarningsModels;
        }

        //private string DownloadHistoricalEarningFromFundamentus(string ticker)
        //{
        //    string ticketCode = ticker.Replace(".SA", "");
        //    string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    string customePath = System.IO.Path.Combine(basePath, @"Helpers\geckodriver-v0.30.0-win64");
        //    //string fullPath = System.IO.Path.GetFullPath("geckodriver.exe", customePath);

        //    IWebDriver driver = new FirefoxDriver(customePath);
        //    driver.Manage().Window.Maximize();
        //    driver.Navigate().GoToUrl(string.Format("https://fundamentus.com.br/proventos.php?papel={0}&tipo=2", ticketCode));
        //    IWebElement element = driver.FindElement(By.Id("resultado"));
        //    var outerHTML = element.GetAttribute("outerHTML");

        //   // Console.WriteLine(outerHTML);
        //   // File.WriteAllText(FileName, outerHTML);
        //    driver.Quit();

        //    return outerHTML;
        //}

        private string DownloadHistoricalEarningFromYahoo(string ticker, DateTime dtStart, DateTime dtEnd)
        {
          //  _logger.LogInformation($"Starting donwload from server ...");
            string allResults = string.Empty;
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/csv"));

                long dt1 = ConvertToTimestamp(dtStart); // will get always datestart + 1 day
                long dt2 = ConvertToTimestamp(dtEnd);  // will get always dateend - 1 day

                // href = "https://query1.finance.yahoo.com/v7/finance/download/VALE?period1=1016668800&period2=1644969600&interval=1d&events=div&includeAdjustedClose=true"
                string uri = string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=div&includeAdjustedClose=true",
                        ticker, dt1, dt2);
                System.Net.Http.HttpResponseMessage response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                allResults =
                    response.Content.ReadAsStringAsync().Result;
                response.Dispose();
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex.Message);
            }
            return allResults;
        }
        private long ConvertToTimestamp(DateTime value)
        {
            DateTime originDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = value.AddHours(21).ToUniversalTime() - originDate;
            return (long)Math.Floor(diff.TotalSeconds);
        }
    }
}
