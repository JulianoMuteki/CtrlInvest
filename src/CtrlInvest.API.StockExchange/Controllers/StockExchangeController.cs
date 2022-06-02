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

        [HttpGet("{textFind}", Name = "FindTicketByTicketCode")]
        public IEnumerable<TicketDto> FindTicketByTicketCode(string searchTicket)
        {
            var tickets = _ticketAppService.FindTicketByTicketCode(searchTicket);
            var ticketsDtos = _mapper.Map<IList<TicketDto>>(tickets);
            return ticketsDtos;
        }

        [HttpGet("/api/HistoricalPrice/{ticketCode}", Name = "HistoricalPricesByTicket")]
        public IEnumerable<HistoricalPriceDto> GetHistoricalPricesByTicket(string ticketCode)
        {
            var historicalPrices = _ticketAppService.GetHistoricalPricesByTicket(ticketCode).Select(historicalPrice => historicalPrice.AsHistoricalPriceDto()).ToList();
            return historicalPrices;
        }

        [HttpGet("/api/HistoricalPrice/{ticketCode}/{startDate}/{endDate}", Name = "HistoricalPricesByTicketAndDates")]
        public IEnumerable<EarningDto> GetHistoricalPricesByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet("/api/Earning/{ticketCode}", Name = "EarningsByTicket")]
        public IEnumerable<EarningDto> GetEarningsByTicket(string ticketCode)
        {
            var tickets = _ticketAppService.GetEarningsByTicket(ticketCode).Select(earning => earning.AsEarningDto());
            return tickets;
        }

        [HttpGet("/api/Earning/{ticketCode}/{startDate}/{endDate}", Name = "EarningsByTicketAndDates")]
        public IEnumerable<EarningDto> GetEarningsByTicketAndDates(string ticketCode, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

    }
}
