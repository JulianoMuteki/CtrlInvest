using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Entities
{
    public class Ticket : EntityBase
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }

        public ICollection<HistoricalDate> HistoricalDates { get; set; }
        public ICollection<TicketSync> TicketSyncs { get; set; }
        private Ticket()
        {
            this.HistoricalDates = new HashSet<HistoricalDate>();
            this.TicketSyncs = new HashSet<TicketSync>(1);
    }
    }
}
