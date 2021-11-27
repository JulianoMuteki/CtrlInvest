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
    public interface DataImport<T> where T : class
    {
        public void DownloadHistoricalToText(string ticker, DateTime dtStart, DateTime dtEnd);
        public IList<T> ConvertHistoricalToList(Ticket ticket);

    }
}
