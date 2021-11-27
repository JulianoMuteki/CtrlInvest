using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Entities
{
    public class TicketSync: EntityBase
    {        
        public bool IsEnabled { get; set; }
        public DateTime DateStart { get; set; }
        public Guid TickerID { get; set; }
        public Ticket Ticket { get; set; }

        public TicketSync()
        {

        }
    }
}
