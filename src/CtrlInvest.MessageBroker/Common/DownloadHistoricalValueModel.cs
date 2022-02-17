using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.MessageBroker.Common
{
    public class DownloadHistoricalValueModel<T> where T : class
    {
        public DownloadHistoricalValueModel (T ticket, DateTime dateStart, DateTime date)
        {
            this.ticket = ticket;
            this.DateStart = dateStart;
            this.DateEnd = date;
        }

        public T ticket { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
