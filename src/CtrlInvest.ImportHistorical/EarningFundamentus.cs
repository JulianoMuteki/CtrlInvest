using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.ImportHistorical
{
    public class EarningFundamentus: DataImport<Earning>
    {
        private const string FileName = "Earnings.txt";
        public void DownloadHistoricalToText(string ticker, DateTime dtStart, DateTime dtEnd)
        {
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string customePath = System.IO.Path.Combine(basePath, @"Helpers\geckodriver-v0.30.0-win64");
            string fullPath = System.IO.Path.GetFullPath("geckodriver.exe", customePath);

            IWebDriver driver = new FirefoxDriver(customePath);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://fundamentus.com.br/proventos.php?papel=TAEE4&tipo=2");
            IWebElement element = driver.FindElement(By.Id("resultado"));
            var outerHTML = element.GetAttribute("outerHTML");

            Console.WriteLine(outerHTML);
            File.WriteAllText(FileName, outerHTML);
            driver.Quit();
        }

        public IList<Earning> ConvertHistoricalToList(Ticket ticket)
        {
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fullPath = System.IO.Path.GetFullPath(FileName, basePath);
            string outerHTML = System.IO.File.ReadAllText(fullPath);

            HtmlDocument doc = new();
            doc.LoadHtml(outerHTML);

            //var nodesHeader = doc.DocumentNode.SelectNodes("//table/thead/tr");
            //var table = new DataTable("MyTable");
            //var headers = nodesHeader[0]
            //    .Elements("th")
            //    .Select(th => th.InnerText.Trim());
            //foreach (var header in headers)
            //{
            //    table.Columns.Add(header);
            //}
            
            var nodesRows = doc.DocumentNode.SelectNodes("//table/tbody/tr");
            var rows = nodesRows.Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Trim())
                .ToArray());
            foreach (var row in rows)
            {
                Earning earning = new Earning();                
                DateTime dt = DateTime.ParseExact(row[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dt2 = DateTime.ParseExact(row[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);

                earning.DateWith = dt;
                earning.ValueIncome = Double.Parse(row[1]);
                earning.Type = row[2];
                earning.PaymentDate = dt2;
                earning.Quantity = Int32.Parse(row[4]);
                //table.Rows.Add(row);
            }

            IList<Earning> list = new List<Earning>();
            return list;
        }
        private long ConvertToTimestamp(DateTime value)
        {
            var UnixTimeStamp = value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long timeStamp = Convert.ToInt64(UnixTimeStamp);
            return timeStamp;
        }
    }
}
