using CtrlInvest.Domain.Entities.FinancialClassification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface IGrandChildTreeAppService: IApplicationServiceBase<GrandChildTree>
    {
        public ICollection<GrandChildTree> GetListByParentID(Guid parentID);
    }
}
