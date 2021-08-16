using CtrlInvest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface IBankAppService : IApplicationServiceBase<Bank>
    {
        ICollection<Bank> GetAllBanks();
    }
}
