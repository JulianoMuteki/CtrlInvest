using CtrlInvest.CrossCutting;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.MessageBroker.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData.Services
{
    public class HistoricalEarningService : IHistoricalEarningService
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly ILogger<HistoricalEarningService> _logger;

        // Constructor
        public HistoricalEarningService(ILogger<HistoricalEarningService> logger, ITicketAppService ticketAppService)
        {
            _logger = logger;
            _ticketAppService = ticketAppService;
        }

        public void SaveInDatabaseOperation(string brokerMessage)
        {
            try
            {
                Earning historicalPrice = ReadBrokerMessage(brokerMessage);
                if (Guid.Empty != historicalPrice.TickerID)
                    _ticketAppService.SaveEarning(historicalPrice);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private Earning ReadBrokerMessage(string brokerMessage)
        {
            Earning earning = new Earning();
            try
            {
                PackageMessage packageMessage = JsonSerialize.JsonDeserializeObject<PackageMessage>(brokerMessage);

                if (packageMessage.isValidMessage())
                {
                    string[] subs = packageMessage.Message.Split('|');
                    DateTime dateWith = DateTime.ParseExact(subs[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime datePayment;
                    DateTime? paymentDate;

                    if (!DateTime.TryParseExact(subs[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datePayment))
                    {
                        paymentDate = null;
                    }
                    else
                    {
                        paymentDate = datePayment;
                    }

                    earning = new Earning()
                    {
                        PaymentDate = paymentDate,
                        DateWith = dateWith,
                        ValueIncome = Double.Parse(subs[1]),
                        Type = subs[2],
                        Quantity = Int32.Parse(subs[4]),
                        TickerID = packageMessage.TicketID
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return earning;
        }
    }
}
