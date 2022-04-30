using AutoMapper;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services.Common;
using CtrlInvest.Services.Dtos;
using CtrlInvest.Services.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CtrlInvest.API.StockExchange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketAppService _ticketAppService;
        public readonly IMapper _mapper;

        public TicketController(ITicketAppService ticketAppService, IMapper mapper)
        {
            _mapper = mapper;
            _ticketAppService = ticketAppService;
        }

        // GET: TicketController
        [HttpGet]
        public IEnumerable<TicketDto> Index()
        {
            var tickets = _ticketAppService.GetAll();
            var ticketsDtos = _mapper.Map<IList<TicketDto>>(tickets);
            return ticketsDtos;
        }

        // GET: TicketController
        [HttpGet("{textFind}", Name = "FindTicketByTicketCode")]
        public IEnumerable<TicketDto> FindTicketByTicketCode(string textFind)
        {
            var tickets = _ticketAppService.FindTicketByTicketCode(textFind);
            var ticketsDtos = _mapper.Map<IList<TicketDto>>(tickets);
            return ticketsDtos;
        }

        [HttpGet("/earning/{ticketCode}", Name = "GetEarningsByTicket")]
        public IEnumerable<EarningDto> GetEarningsByTicket(string ticketCode)
        {
            var tickets = _ticketAppService.GetEarningsByTicket(ticketCode).Select(earning => earning.AsEarningDto());
            return tickets;
        }

        [HttpGet("/historicalPrice/{ticketCode}", Name = "GetHistoricalPricesByTicket")]
        public IEnumerable<HistoricalPriceDto> GetHistoricalPricesByTicket(string ticketCode)
        {
            var historicalPrices = _ticketAppService.GetHistoricalPricesByTicket(ticketCode).Select(historicalPrice => historicalPrice.AsHistoricalPriceDto()).ToList();
            return historicalPrices;
        }

    }
}
