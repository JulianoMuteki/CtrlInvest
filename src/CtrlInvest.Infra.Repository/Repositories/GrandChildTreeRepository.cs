using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;

namespace CtrlInvest.Infra.Repository.Repositories
{
    public class GrandChildTreeRepository : GenericRepository<GrandChildTree>, IGrandChildTreeRepository
    {
        public GrandChildTreeRepository(CtrlInvestContext context)
            : base(context)
        {

        }
    }
}
