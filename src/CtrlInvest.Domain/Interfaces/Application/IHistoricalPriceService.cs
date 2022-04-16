
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using System;
using System.Collections.Generic;

namespace CtrlInvest.Services.StocksExchanges
{
    public interface IHistoricalPriceService : IApplicationServiceBase<HistoricalPrice>
    {
        void SaveInDatabaseOperation(string brokerMessages);
        void SaveRangeInDatabaseOperation(IList<string> brokerMessages);

        bool ExistByTickedCodeAndDate(string tickedCode, DateTime dateTime);
    }
}
