using Microsoft.AspNetCore.Identity;
using System;

namespace CtrlInvest.Domain.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
