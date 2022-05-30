using System.ComponentModel.DataAnnotations;

namespace CtrlInvest.Security.Permission
{
    public enum RoleAuthorize
    {
        [Display(GroupName = "Manager", Name = "Admin", Description = "System admin")]
        Admin,

        [Display(GroupName = "User", Name = "User", Description = "View all system")]
        User,

        [Display(GroupName = "User", Name = "Client", Description = "Just client")]
        Client
    }
}
