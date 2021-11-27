using CtrlInvest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace CtrlInvest.ImportHistorical
{
    /// <summary>
    /// A 'ConcreteStrategy' class
    /// </summary>
    public class HistoricalPriceYahoo : DataImport
    {
        private const string FileName = "Historical.txt";
        public override void DownloadHistoricalToText(string ticker, DateTime dtStart, DateTime dtEnd)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/csv"));
                //OLD test
                //https://query1.finance.yahoo.com/v7/finance/download/TAEE11.SA?period1=1194912000&amp;period2=1635984000&amp;interval=1d&amp;events=history&amp;includeAdjustedClose=true

                long dt1 = ConvertToTimestamp(dtStart);
                long dt2 = ConvertToTimestamp(dtEnd);

                HttpResponseMessage response = client.GetAsync(
                    string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true",
                        ticker, dt1, dt2)).Result;
                response.EnsureSuccessStatusCode();
                string conteudo =
                    response.Content.ReadAsStringAsync().Result;

                // dynamic resultado = JsonConvert.DeserializeObject(conteudo);
                Console.WriteLine(conteudo);
                File.WriteAllText(FileName, conteudo);
            }
        }

        public override IList<HistoricalPrice> ConvertHistoricalToList(Ticket ticket)
        {
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fullPath = System.IO.Path.GetFullPath(FileName, basePath);
            string[] lines = System.IO.File.ReadAllLines(fullPath);
            int skipFirstLine = 0;
            IList<HistoricalPrice> historicalsList = new List<HistoricalPrice>();

            foreach (string line in lines)
            {
                if (skipFirstLine > 0)
                {
                    string[] subs = line.Split(',');
                    Console.WriteLine("\t" + line);
                    if(subs[6] != "null")
                    {
                        HistoricalPrice history = new HistoricalPrice()
                        {
                            TickerCode = ticket.Ticker,
                            Date = Convert.ToDateTime(subs[0]),
                            Open = double.Parse(subs[1]),
                            High = double.Parse(subs[2]),
                            Low = double.Parse(subs[3]),
                            Close = double.Parse(subs[4]),
                            AdjClose = double.Parse(subs[5]),
                            Volume = Convert.ToInt32(subs[6]),
                            TickerID = ticket.Id
                        };
                        historicalsList.Add(history);
                    }
                }
                else
                    skipFirstLine++;
            }

            return historicalsList;
        }
        private long ConvertToTimestamp(DateTime value)
        {
            var UnixTimeStamp = value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long timeStamp = Convert.ToInt64(UnixTimeStamp);
            return timeStamp;
        }
    }
}