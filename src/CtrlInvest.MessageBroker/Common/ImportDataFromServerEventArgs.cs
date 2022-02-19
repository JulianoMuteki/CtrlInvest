using CtrlInvest.Domain.Entities;
using System;

namespace CtrlInvest.MessageBroker.Common
{
    public class ImportDataFromServerEventArgs : EventArgs
    {
        public string HistoricalDataList { get; set; }
        public Ticket Ticket { get; set; }
    }
}
