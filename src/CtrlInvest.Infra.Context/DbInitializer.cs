using CtrlInvest.Domain.Identity;
using CtrlInvest.Security.Permission;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CtrlInvest.Infra.Context
{
    public class DbInitializer
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                SeedData(userManager, roleManager);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("juliano.pestili@outlook.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "juliano.pestili@outlook.com";
                user.Email = "juliano.pestili@outlook.com";
                user.PhoneNumber = "(19) 99999-8888";
                user.FirstName = "Juliano";
                user.LastName = "Pestili";

                IdentityResult result = userManager.CreateAsync(user, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    result = userManager.AddToRoleAsync(user, RoleAuthorize.Admin.ToString()).Result;
                }
            }
        }

        public void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(RoleAuthorize)))
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    ApplicationRole role = new ApplicationRole();
                    role.Name = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}