
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlInvest.Services.StocksExchanges
{
    public interface IHistoricalPriceService : IApplicationServiceBase<HistoricalPrice>
    {
        void SaveInDatabaseOperation(string brokerMessages);
        Task SaveRangeInDatabaseOperation(IList<string> brokerMessages);

        bool ExistByTickedCodeAndDate(string tickedCode, DateTime dateTime);
    }
}
