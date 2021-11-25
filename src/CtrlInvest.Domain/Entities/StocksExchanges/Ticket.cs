using CtrlInvest.Domain.Common;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Entities
{
    public class Ticket : EntityBase
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }

        public ICollection<HistoricalPrice> HistoricalPrices { get; set; }
        public ICollection<TicketSync> TicketSyncs { get; set; }
        private Ticket()
        {
            this.HistoricalPrices = new HashSet<HistoricalPrice>();
            this.TicketSyncs = new HashSet<TicketSync>(1);
    }
    }
}
