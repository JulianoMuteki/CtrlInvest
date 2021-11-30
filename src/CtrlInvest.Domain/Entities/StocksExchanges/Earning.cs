using CtrlInvest.Domain.Common;
using System;

namespace CtrlInvest.Domain.Entities.StocksExchanges
{
    public class Earning : EntityBase
    {
        public DateTime DateWith { get; set; }
        public double ValueIncome { get; set; }
        public string Type { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int Quantity { get; set; }
        public Guid TickerID { get; set; }
        public Ticket Ticket { get; set; }

        public Earning()
        {

        }
    }
}
