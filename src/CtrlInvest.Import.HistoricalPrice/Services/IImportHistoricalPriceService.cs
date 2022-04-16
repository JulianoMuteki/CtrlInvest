using CtrlInvest.MessageBroker.Common;
using System;

namespace CtrlInvest.Import.HistoricalPrice
{
    public interface IImportHistoricalPriceService
    {
        event EventHandler<ImportDataFromServerEventArgs> ThresholdReached;
        void DoImportOperation();
    }
}
