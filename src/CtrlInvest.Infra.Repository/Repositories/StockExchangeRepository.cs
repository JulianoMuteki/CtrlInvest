using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Infra.Repository.Repositories
{
    public class StockExchangeRepository : GenericRepository<Ticket>, IStockExchangeRepository
    {
        public StockExchangeRepository(CtrlInvestContext context)
            : base(context)
        {

        }

        public ICollection<TicketSync> GetTicketsSyncs()
        {
            try
            {
                return _context.Set<TicketSync>()
                                .Include(x => x.Ticket)
                                .ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<StockExchangeRepository>("Unexpected error fetching GetTicketsSyncs", nameof(this.GetTicketsSyncs), ex);
            }
        }
    }
}