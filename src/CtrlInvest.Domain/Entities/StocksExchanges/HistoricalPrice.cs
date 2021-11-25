using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Entities
{
    public class HistoricalPrice : EntityBase
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public int Volume { get; set; }

        public string TickerCode { get; set; }

        public Guid TickerID { get; set; }
        public Ticket Ticket { get; set; }

        public HistoricalPrice()
        {

        }
    }
}
