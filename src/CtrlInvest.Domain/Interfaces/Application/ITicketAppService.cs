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
        public ICollection<Ticket> FindTicketByTicketCode(string textFind);
        public ICollection<HistoricalPrice> GetHistoricalPricesByTicket(string ticketCode);

        public HistoricalPrice GetLastPriceByTicket(string ticketCode);

        public ICollection<Earning> GetEarningsByTicket(string ticketCode);
        void SaveHistoricalPrice(HistoricalPrice historicalPrice);
        void SaveEarning(Earning SaveEarning);
        public ICollection<HistoricalPrice> GetHistoricalPricesByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate);
        public ICollection<Earning> GetEarningsByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate);
    }
}
