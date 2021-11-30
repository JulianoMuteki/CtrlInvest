using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using System;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ITicketAppService : IApplicationServiceBase<Ticket>
    {
        public ICollection<TicketSync> GetAllTicketsSyncs();
        public HistoricalPrice GetLatestHistoricalByTicker(string ticker);
        void SaveHistoricalPricesList(IList<HistoricalPrice> oricalPricesList);

        public Earning GetLastEarningByTicker(Guid ticketID);
        void SaveEarningsList(IList<Earning> earningList);
    }
}
