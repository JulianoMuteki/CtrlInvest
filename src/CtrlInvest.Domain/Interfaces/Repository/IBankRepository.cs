using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Repository
{
    public interface IBankRepository : IGenericRepository<Bank>
    {
        ICollection<Bank> GetAllBanks();
    }
}
