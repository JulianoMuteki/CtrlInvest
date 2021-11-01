using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace CtrlInvest.ImportHistorical
{
    /// <summary>
    /// A 'ConcreteStrategy' class
    /// </summary>
    public class ConcreteStrategyA : Strategy
    {
        private const string FileName = "Historical.txt";
        public override void DownloadDataFromYahoo(string ticker, long dtStart, long dtEnd)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/csv"));

                HttpResponseMessage response = client.GetAsync(
                    string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval=1d&events=history&includeAdjustedClose=true",
                        ticker, dtStart, dtEnd)).Result;
                response.EnsureSuccessStatusCode();
                string conteudo =
                    response.Content.ReadAsStringAsync().Result;

                // dynamic resultado = JsonConvert.DeserializeObject(conteudo);
                Console.WriteLine(conteudo);
                File.WriteAllTextAsync(FileName, conteudo);
            }
        }

        public override void SaveHistoricalInDatabase()
        {
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fullPath = System.IO.Path.GetFullPath(FileName, basePath);
            string[] lines = System.IO.File.ReadAllLines(basePath + FileName);


        }
    }
}