using CtrlInvest.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Domain.Interfaces.Application
{
    public interface ISecurityApplicationService
    {
        IList<ApplicationUser> GetAllUsers();
    }
}
