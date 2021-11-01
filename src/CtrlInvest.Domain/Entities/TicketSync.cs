using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Entities
{
    public class TicketSync : ValueObject<TicketSync>
    {
        public Guid TicketSyncID { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime DateStart { get; set; }

        public Guid TickerID { get; set; }
        public Ticket Ticket { get; set; }
        private TicketSync()
        {

        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            return this.GetType().GetProperties().Select(propInfo => propInfo.GetValue(this, null));
        }
    }
}
