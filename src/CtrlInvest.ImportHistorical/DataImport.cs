using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.ImportHistorical
{
    /// <summary>
    /// The 'Strategy' abstract class
    /// </summary>
    public abstract class DataImport
    {
        public abstract void DownloadDataFromYahoo(string ticker, long dtStart, long dtEnd);
        public abstract void SaveHistoricalInDatabase();

    }
}
