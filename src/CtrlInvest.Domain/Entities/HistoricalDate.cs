using CtrlInvest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Entities
{
    public class HistoricalDate : ValueObject<HistoricalDate>
    {
        
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public int Volume { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return this.GetType().GetProperties().Select(propInfo => propInfo.GetValue(this, null));
        }

        public string TickerCode { get; set; }

        public Guid TickerID { get; set; }
        public Ticket Ticket { get; set; }
        //public Guid ParentTreeID { get; private set; }
        //public ParentTree ParentTree { get; private set; }
    }
}
