using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Dtos
{
    public class TicketDto
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
    }
}
