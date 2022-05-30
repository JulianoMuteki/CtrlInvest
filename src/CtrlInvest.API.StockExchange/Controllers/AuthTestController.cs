using CtrlInvest.Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CtrlInvest.API.StockExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthTestController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public AuthTestController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("Authorized")]        
        public IActionResult Authorized()
        {
            return Ok("Yes, you are Authorized");
        }

        [HttpGet("RolesByUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RolesByUser(string emailUser)
        {
            var existingUser = await _userManager.FindByEmailAsync(emailUser);
            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                return Ok(roles);
            }

            return BadRequest(new
            {
                Errors = "User not found",
                Success = false
            });
        }
    }
}

