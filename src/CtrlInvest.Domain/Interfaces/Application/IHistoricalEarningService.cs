
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using System.Collections.Generic;

namespace CtrlInvest.Services.StocksExchanges
{
    public interface IHistoricalEarningService : IApplicationServiceBase<Earning>
    {
        void SaveInDatabaseOperation(string message);
        void SaveRangeInDatabaseOperation(List<string> requestChunck);
    }
}
