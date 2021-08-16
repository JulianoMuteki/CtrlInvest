using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Repository;
using CtrlInvest.Infra.Context;
using CtrlInvest.Infra.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Infra.Repository.Repositories
{
   public class ChildTreeRepository : GenericRepository<ChildTree>, IChildTreeRepository
    {
        public ChildTreeRepository(CtrlInvestContext context)
            : base(context)
        {

        }
    }
}
