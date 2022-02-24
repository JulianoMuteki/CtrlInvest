
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;

namespace CtrlInvest.Services.StocksExchanges
{
    public interface IHistoricalEarningService : IApplicationServiceBase<Earning>
    {
        void SaveInDatabaseOperation(string message);
    }
}
