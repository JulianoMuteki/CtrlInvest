using CtrlInvest.Domain.Entities;
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
        public abstract void DownloadHistoricalToText(string ticker, DateTime dtStart, DateTime dtEnd);
        public abstract IList<HistoricalPrice> ConvertHistoricalToList(Ticket ticket);

    }
}
