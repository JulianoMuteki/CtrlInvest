using CtrlInvest.Domain.Entities;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ITicketAppService : IApplicationServiceBase<Ticket>
    {
        public ICollection<TicketSync> GetAllTicketsSyncs();
        public HistoricalPrice GetLatestHistoricalByTicker(string ticker);
        void SaveHistoricalDateList(IList<HistoricalPrice> historicalsList);
    }
}
