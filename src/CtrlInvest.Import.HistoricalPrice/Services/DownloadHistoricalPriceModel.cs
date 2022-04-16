using CtrlInvest.Domain.Entities;
using System;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class DownloadHistoricalPriceModel
    {
        public DownloadHistoricalPriceModel(Ticket ticket, DateTime dateStart, DateTime date)
        {
            this.ticket = ticket;
            this.DateStart = dateStart;
            this.DateEnd = date;
        }

        public Ticket ticket { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

    }
}
