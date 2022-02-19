
using CtrlInvest.MessageBroker.Common;
using System;

namespace CtrlInvest.Import.Dividends.Services
{
    public interface IEarningService
    {
        event EventHandler<ImportDataFromServerEventArgs> ThresholdReached;
        void DoImportOperation();
    }
}
