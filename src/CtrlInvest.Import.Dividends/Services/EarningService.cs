

using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.MessageBroker.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using OpenQA.Selenium.Remote;
using Microsoft.Extensions.Configuration;

namespace CtrlInvest.Import.Dividends.Services
{
    public class EarningService : IEarningService
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly IConfiguration _configuration;

        public event EventHandler<ImportDataFromServerEventArgs> ThresholdReached;
        private IWebDriver _driver;
        public EarningService(ITicketAppService ticketAppService, IConfiguration configuration)
        {
            _ticketAppService = ticketAppService;
            _configuration = configuration;
        }

        public void LoadPage(string UrlPaginaCotacoes)
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl(UrlPaginaCotacoes);
        }

        private void CreateInstanceWebDriver()
        {
            string host = _configuration.GetValue<string>("WebDriverConfig:HostName");
            string port = _configuration.GetValue<string>("WebDriverConfig:Port");

            _driver = new RemoteWebDriver(
                new Uri($"http://{host}:{port}/wd/hub"),
                new FirefoxOptions());
        }

        private void DestroyInstanceWebDriver()
        {
            if (_driver != null)
            {
                _driver.Close();
                _driver.Quit();
                _driver = null;
            }
        }

        public void DoImportOperation()
        {
            // _logger.LogInformation($"Start Import Operation");
            CreateInstanceWebDriver();
            try
            {
                IList<DownloadHistoricalValueModel<Ticket>> downloadHistoricalValueModels = GetListHistoricalEarningsToImport();
                foreach (var downloadHistoricalValueModel in downloadHistoricalValueModels)
                {
                    var historicalEarningList = DownloadHistoricalEarningFromFundamentus(downloadHistoricalValueModel.ticket.Ticker, downloadHistoricalValueModel.DateStart,
                                                                 downloadHistoricalValueModel.DateEnd);

                    OnThresholdReached(historicalEarningList, downloadHistoricalValueModel.ticket);
                }
            }
            catch (Exception ex)
            {

            }
            DestroyInstanceWebDriver();
        }

        protected virtual void OnThresholdReached(string historicalEarningList, Ticket ticket)
        {
            ImportDataFromServerEventArgs args = new()
            {
                HistoricalDataMessage = historicalEarningList,
                Ticket = ticket
            };

            ThresholdReached?.Invoke(this, args);
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

        private string DownloadHistoricalEarningFromFundamentus(string ticker, DateTime dtStart, DateTime dtEnd)
        {
            string ticketCode = ticker.Replace(".SA", "");
            LoadPage(string.Format("https://fundamentus.com.br/proventos.php?papel={0}&tipo=2", ticketCode));

            IWebElement element = _driver.FindElement(By.Id("resultado"));
            var outerHTML = element.GetAttribute("outerHTML");

            return ConvertToList(outerHTML);
        }

        private string ConvertToList(string tableHTML)
        {
            HtmlDocument doc = new();
            doc.LoadHtml(tableHTML);

            var nodesRows = doc.DocumentNode.SelectNodes("//table/tbody/tr");
            var rows = nodesRows.Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Trim())
                .Select(x => x).ToList());

            var result = rows.Select(x => string.Join<string>('|', x)).ToList();

            string teste = string.Join<string>(Environment.NewLine, result);

            return teste;
        }

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

        public void StopOperation()
        {
            DestroyInstanceWebDriver();
        }
    }
}
