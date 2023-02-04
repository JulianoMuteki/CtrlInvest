using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Base;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Interfaces.Repository
{
    public interface IStockExchangeRepository : IGenericRepository<Ticket>
    {
        ICollection<TicketSync> GetTicketsSyncs();
    }
}
