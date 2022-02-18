using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.MessageBroker.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData
{
    public class HistoricalPriceService : IHistoricalPriceService
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly ILogger<HistoricalPriceService> _logger;      

        // Constructor
        public HistoricalPriceService(ILogger<HistoricalPriceService> logger, ITicketAppService ticketAppService)
        {
            _logger = logger;
            _ticketAppService = ticketAppService;
        }

        public void SaveInDatabaseOperation(string brokerMessage)
        {
            try
            {
                HistoricalPrice historicalPrice = ReadBrokerMessage(brokerMessage);
                if (!string.IsNullOrEmpty(historicalPrice.TickerCode))
                    _ticketAppService.SaveHistoricalPrice(historicalPrice);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private HistoricalPrice ReadBrokerMessage(string brokerMessage)
        {
            HistoricalPrice history = new HistoricalPrice();
            try
            {
                PackageMessage packageMessage = JsonSerialize.JsonDeserializeObject<PackageMessage>(brokerMessage);

                if (packageMessage.isValidMessage())
                {
                    string[] subs = packageMessage.Message.Split(',');

                    history = new HistoricalPrice()
                    {
                        TickerCode = packageMessage.TicketCode,
                        Date = Convert.ToDateTime(subs[0]),
                        Open = double.Parse(subs[1]),
                        High = double.Parse(subs[2]),
                        Low = double.Parse(subs[3]),
                        Close = double.Parse(subs[4]),
                        AdjClose = double.Parse(subs[5]),
                        Volume = Convert.ToInt32(subs[6]),
                        TickerID = packageMessage.TicketID
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return history;
        }
    }
}
