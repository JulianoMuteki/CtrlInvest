using AutoMapper;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Identity;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Security.Permission;
using CtrlInvest.Services.Dtos;
using CtrlInvest.Services.Email;
using CtrlInvest.Services.ViewModel;
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
    public class StockManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ITicketAppService _ticketAppService;
        public readonly IMapper _mapper;

        public StockManagementController(ILogger<AccountController> logger, IConfiguration configuration,
            ITicketAppService ticketAppService, IMapper mapper)
        {
            _configuration = configuration;
            _ticketAppService = ticketAppService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("AddTickerSyncData")]
        public async Task<IActionResult> AddTickerSyncData([FromBody] TicketSyncDto ticketSyncDto)
        {
            if (ModelState.IsValid)
            {
                var ticket = _ticketAppService.FindTicketByTicketCode(ticketSyncDto.Ticker);

                var earningsDtos = _mapper.Map<TicketSync>(ticketSyncDto);
                earningsDtos.Ticket = ticket;
                earningsDtos.TickerID = ticket.Id;

                _ticketAppService.SaveTickerSync(earningsDtos);
            }
            else
                return BadRequest(ModelState);

            return Ok("TickerSync saved"); // passtoken
        }
    }
}
