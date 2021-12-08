using CtrlInvest.Domain.Entities;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Common
{
    public static class Extensions
    {
        public static TicketDto AsTicketDto(this Ticket ticket)
        {
            return new TicketDto
            {
                Ticker = ticket.Ticker,
                Name = ticket.Name,
                Exchange = ticket.Exchange,
                Country = ticket.Country,
                Currency = ticket.Currency
            };
        }
    }
}
