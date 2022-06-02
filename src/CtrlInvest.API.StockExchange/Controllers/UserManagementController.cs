using CtrlInvest.Domain.Identity;
using CtrlInvest.Security.Permission;
using CtrlInvest.Services.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlInvest.API.StockExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public UserManagementController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("Users")]
        [Authorize(Roles = "Admin")]
        public IActionResult Get()
        {
            var users = _userManager.Users.ToList()
                                                       .Select(user => new
                                                       {
                                                           User = user.UserName
                                                       }).ToList();
            return Ok(users);
        }

        [HttpDelete("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var rolesForUser = await _userManager.GetRolesAsync(user);

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }

                await _userManager.DeleteAsync(user);
                return Ok("Deleteded Succeeded");
            }

            return BadRequest("Error in Delete");
        }

        [HttpPut("ChangePassword")]
        [AuthorizeRoles(RoleAuthorize.Client)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser == null)
                {
                    return BadRequest(new
                    {
                        Errors = "Email already in use",
                        Success = false
                    });
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    return Ok("Changeded Succeeded");
                }
                else
                    return BadRequest(changePasswordResult.Errors);
            }
            else
                return BadRequest(ModelState);

            return Ok("Your validation code was sended to youe e-mail."); // passtoken
        }
    }
}
