using AutoMapper;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services.Common;
using CtrlInvest.Services.Dtos;
using CtrlInvest.Services.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CtrlInvest.API.StockExchange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockExchangeController : ControllerBase
    {
        private readonly ITicketAppService _ticketAppService;
        public readonly IMapper _mapper;

        public StockExchangeController(ITicketAppService ticketAppService, IMapper mapper)
        {
            _mapper = mapper;
            _ticketAppService = ticketAppService;
        }
      
        //[HttpGet("TicketsAvailable")]       
        //public IEnumerable<TicketDto> Index()
        //{
        //    var tickets = _ticketAppService.GetAll();
        //    var ticketsDtos = _mapper.Map<IList<TicketDto>>(tickets);
        //    return ticketsDtos;
        //}

        [HttpGet("{searchTicket}", Name = "FindTicketByTicketCode")]
        public IEnumerable<TicketDto> FindTicketByTicketCode(string searchTicket)
        {
            var tickets = _ticketAppService.FindTicketsByText(searchTicket);
            var ticketsDtos = _mapper.Map<IList<TicketDto>>(tickets);
            return ticketsDtos;
        }

        [HttpGet("/api/HistoricalPrice/{ticketCode}", Name = "HistoricalPricesByTicket")]
        public IEnumerable<HistoricalPriceDto> GetHistoricalPricesByTicket(string ticketCode)
        {
            var historicalPrices = _ticketAppService.GetHistoricalPricesByTicket(ticketCode);
            var historicalPricesDtos = _mapper.Map<IList<HistoricalPriceDto>>(historicalPrices);
            return historicalPricesDtos;
        }

        [HttpGet("/api/HistoricalPrice/{ticketCode}/{startDate}/{endDate}", Name = "HistoricalPricesByTicketAndDates")]
        public IEnumerable<HistoricalPriceDto> GetHistoricalPricesByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate)
        {
            var historicalPrices = _ticketAppService.GetHistoricalPricesByTicketAndDates(ticketCode, startDate, endDate);
            var historicalPricesDtos = _mapper.Map<IList<HistoricalPriceDto>>(historicalPrices);
            return historicalPricesDtos;
        }

        [HttpGet("/api/Earning/{ticketCode}", Name = "EarningsByTicket")]
        public IEnumerable<EarningDto> GetEarningsByTicket(string ticketCode)
        {
            var earnings = _ticketAppService.GetEarningsByTicket(ticketCode);
            var earningsDtos = _mapper.Map<IList<EarningDto>>(earnings);
            return earningsDtos;
        }

        [HttpGet("/api/Earning/{ticketCode}/{startDate}/{endDate}", Name = "EarningsByTicketAndDates")]
        public IEnumerable<EarningDto> GetEarningsByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate)
        {
            var earnings = _ticketAppService.GetEarningsByTicketAndDates(ticketCode, startDate, endDate);
            var earningsDtos = _mapper.Map<IList<EarningDto>>(earnings);
            return earningsDtos;
        }

        [HttpGet("/api/LastPrice/{ticketCode}", Name = "LastPriceByTicket")]
        public HistoricalPriceDto GetLastPriceByTicket(string ticketCode)
        {
            var historicalPrices = _ticketAppService.GetLastPriceByTicket(ticketCode);
            var historicalPriceDto = _mapper.Map<HistoricalPriceDto>(historicalPrices);
            return historicalPriceDto;
        }

    }
}
