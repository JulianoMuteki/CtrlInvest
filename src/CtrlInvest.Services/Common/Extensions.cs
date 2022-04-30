using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Services.Dtos;

namespace CtrlInvest.Services.Common
{
    public static class Extensions
    {
        //public static TicketDto AsTicketDto(this Ticket ticket)
        //{
        //    return new TicketDto
        //    {
        //        Ticker = ticket.Ticker,
        //        Name = ticket.Name,
        //        Exchange = ticket.Exchange,
        //        Country = ticket.Country,
        //        Currency = ticket.Currency
        //    };
        //}

        public static HistoricalPriceDto AsHistoricalPriceDto(this HistoricalPrice historicalPrice)
        {
            return new HistoricalPriceDto
            {
                TickerCode = historicalPrice.TickerCode,
                AdjClose = historicalPrice.AdjClose,
                Date = historicalPrice.Date,
                Close = historicalPrice.Close,
                High = historicalPrice.High,
                Low = historicalPrice.Low,
                Open = historicalPrice.Open,
                Volume = historicalPrice.Volume
            };
        }

        public static EarningDto AsEarningDto(this Earning earning)
        {
            return new EarningDto
            {
                DateWith = earning.DateWith,
                PaymentDate = earning.PaymentDate,
                Quantity = earning.Quantity,
                Type = earning.Type,
                ValueIncome = earning.ValueIncome
            };
        }
    }
}
