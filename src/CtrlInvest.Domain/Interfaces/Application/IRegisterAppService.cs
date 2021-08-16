using CtrlInvest.Domain.Entities.FinancialClassification;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface IRegisterAppService
    {
        public void AddTree(string treeID, string titleTree, string description, string tag);

        ICollection<ParentTree> GetAll();
    }
}
