using CtrlInvest.Domain.Entities.Aggregates;
using CtrlInvest.Domain.Identity;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Security;
using CtrlInvest.Security.Permission;
using CtrlInvest.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlInvest.API.StockExchange.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ICustomEmailSender _customEmailSender;
        private readonly IIdentityCustomService _identityCustomService;
        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<ApplicationRole> roleManager,
           IIdentityCustomService identityCustomService,
           ILogger<AccountController> logger, IConfiguration configuration,
           ICustomEmailSender customEmailSender)
        {
            _identityCustomService = identityCustomService;
            _customEmailSender = customEmailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Errors = "Email already in use",
                        Success = false
                    });
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    try
                    {
                        var newuser = await _userManager.FindByEmailAsync(model.Email);
                        var codeEmailConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(newuser);

                        await _customEmailSender.SendEmailAsync(newuser.Email, "Ctrl.Invest Register Token", codeEmailConfirmation);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new
                        {
                            Errors = $"Error: {ex.Message}",
                            Success = false
                        });
                    }
                }
                else
                    return BadRequest(new
                    {
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
            }
            else
                return BadRequest(ModelState);

            return Ok("Your validation code was sended to youe e-mail."); // passtoken
        }

        [HttpPost("ValidateRegister")]
        public async Task<IActionResult> ValidateRegister([FromBody] LoginViewModel model, string validationCode)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return BadRequest($"The {model.Email} was not found");
                }

                var result = await _userManager.ConfirmEmailAsync(user, validationCode);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleAuthorize.Client.ToString());
                    return Ok($"Congratulations {user}, your code was validaded. Now you can do login!");
                }

                return BadRequest("Error to validate token. Try use ReSendTokenEmail.");
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser == null)
                {
                    return BadRequest(new
                    {
                        Errors = $"The {model.Email} was not found",
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);
                if (!isCorrect)
                {
                    return BadRequest(new
                    {
                        Errors = "Fail validade your account",
                        Success = false
                    });
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var jwtId = Guid.NewGuid();
                    var claims = await IdentityUserHelper.GetClaimsByUser(existingUser, _roleManager, _userManager);
                    UserToken userToken = JwtSecurityTokenCustom.GenerateToken(model.Email, jwtId , _configuration["Jwt:key"],
                                                             _configuration["TokenConfiguration:ExpireHours"],
                                                             _configuration["TokenConfiguration:Issuer"],
                                                             _configuration["TokenConfiguration:Audience"],
                                                             claims);
                    UserTokenHistory userTokenHistory = new UserTokenHistory();
                    userTokenHistory.SetUserToken(existingUser.Id, jwtId, userToken.Expiration, userToken.Token);

                    _identityCustomService.Add(userTokenHistory);
                    return Ok(new
                    {
                        State = "You are logged",
                        Token = userToken
                    });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return BadRequest(new
                    {
                        Errors = "User account locked out.",
                        Success = false
                    });
                }
            }
            _logger.LogWarning("Invalid login attempt.");
            return BadRequest(new
            {
                Errors = "Invalid login attempt.",
                Success = false
            });
        }

        [HttpPost("ReSendTokenEmail")]
        public async Task<IActionResult> ReSendTokenEmail([FromBody] LoginViewModel model)
        {
            string jwtToken = string.Empty;

            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    _logger.LogInformation("User created a new account with password.");
                    try
                    {
                        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                        await _customEmailSender.SendEmailAsync(existingUser.Email, "Ctrl.Invest Register Token", emailConfirmationToken);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
                else
                    return BadRequest("User not found");
            }
            else
                return BadRequest(ModelState);

            return Ok("Your validation code was sended to youe e-mail."); // passtoken
        }

    }
}
