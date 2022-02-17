
using CtrlInvest.MessageBroker.Common;
using System;

namespace CtrlInvest.Import.Dividends.Services
{
    public interface IEarningService
    {
        event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
        void DoImportOperation();
    }
}
