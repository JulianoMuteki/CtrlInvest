using CtrlInvest.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ITicketAppService : IApplicationServiceBase<Ticket>
    {
        // get ticketsync


        public ICollection<TicketSync> GetAllTicketsSyncs();
    }
}
