using System;

namespace CtrlInvest.ImportHistorical
{
    /// <summary>
    /// The 'Context' class
    /// </summary>
    public class Context
    {
        Strategy strategy;
        // Constructor
        public Context(Strategy strategy)
        {
            this.strategy = strategy;
        }
        public void ImportHistoricalByDates()
        {
            strategy.DownloadDataFromYahoo("TAEE11.SA", 1194912000, 1634256000);
        }
    }
}