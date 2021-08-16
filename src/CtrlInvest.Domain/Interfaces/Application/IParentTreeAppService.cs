using CtrlInvest.Domain.Entities.FinancialClassification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface IParentTreeAppService: IApplicationServiceBase<ParentTree>
    {
        ICollection<ParentTree> GetAll_WithChildrem();
    }
}
