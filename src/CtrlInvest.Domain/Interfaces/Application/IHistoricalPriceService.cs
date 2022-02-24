
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;

namespace CtrlInvest.Services.StocksExchanges
{
    public interface IHistoricalPriceService : IApplicationServiceBase<HistoricalPrice>
    {
        void SaveInDatabaseOperation(string message);
    }
}
