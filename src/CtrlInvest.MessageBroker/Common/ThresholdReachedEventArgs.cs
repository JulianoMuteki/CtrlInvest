using CtrlInvest.Domain.Entities;
using System;

namespace CtrlInvest.MessageBroker.Common
{
    public class ThresholdReachedEventArgs : EventArgs
    {
        public string HistoricalEarningList { get; set; }
        public Ticket Ticket { get; set; }
    }
}
