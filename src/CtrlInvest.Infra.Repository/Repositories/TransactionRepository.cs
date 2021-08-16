using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;

namespace CtrlInvest.Infra.Repository.Repositories
{
    public class TransactionRepository: GenericRepository<FinancialTransaction>, ITransactionRepository
    {
        public TransactionRepository(CtrlInvestContext context)
            : base(context)
        {

        }
    }
}
