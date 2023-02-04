using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using System;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ITicketAppService : IApplicationServiceBase<Ticket>
    {
        public ICollection<TicketSync> GetAllTicketsSyncs();
        ICollection<TicketSync> GetTicketsSyncs();
        public HistoricalPrice GetLatestHistoricalByTicker(string ticker);
        void SaveHistoricalPricesList(IList<HistoricalPrice> oricalPricesList);
        public Earning GetLastEarningByTicker(Guid ticketID);
        void SaveEarningsList(IList<Earning> earningList);
        public ICollection<Ticket> FindTicketsByText(string textFind);
        public Ticket FindTicketByTicketCode(string textFind);
        public ICollection<HistoricalPrice> GetHistoricalPricesByTicket(string ticketCode);

        public HistoricalPrice GetLastPriceByTicket(string ticketCode);

        public ICollection<Earning> GetEarningsByTicket(string ticketCode);
        void SaveHistoricalPrice(HistoricalPrice historicalPrice);
        void SaveEarning(Earning SaveEarning);

        void SaveTickerSync(TicketSync ticketSync);
        public ICollection<HistoricalPrice> GetHistoricalPricesByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate);
        public ICollection<Earning> GetEarningsByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate);
    }
}
