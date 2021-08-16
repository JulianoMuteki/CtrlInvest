using CtrlInvest.Domain.Entities.FinancialClassification;
using CtrlInvest.Domain.Interfaces.Base;
using System.Collections.Generic;

namespace CtrlInvest.Domain.Interfaces.Repository
{
    public interface IParentTreeRepository : IGenericRepository<ParentTree>
    {
        ICollection<ParentTree> GetAll_WithChildrem();
    }
}
