using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class DownloadHistoricalPriceModel
    {
        public DownloadHistoricalPriceModel(string ticketCode, DateTime dateStart, DateTime date)
        {
            this.TicketCode = ticketCode;
            this.DateStart = dateStart;
            this.DateEnd = date;
        }

        public string TicketCode { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

    }
}
