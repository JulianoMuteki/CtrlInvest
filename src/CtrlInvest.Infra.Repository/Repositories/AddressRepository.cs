using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;

namespace CtrlInvest.Infra.Repository.Repositories
{
   public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(CtrlInvestContext context)
            : base(context)
        {

        }
    }
}
