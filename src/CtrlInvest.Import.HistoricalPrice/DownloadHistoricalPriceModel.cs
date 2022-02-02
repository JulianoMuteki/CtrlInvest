using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class DownloadHistoricalPriceModel
    {
        private object ticker;
        private object dateStart;
        private DateTime date;

        public DownloadHistoricalPriceModel(object ticker, object dateStart, DateTime date)
        {
            this.ticker = ticker;
            this.dateStart = dateStart;
            this.date = date;
        }

        public string TicketCode { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

    }
}
