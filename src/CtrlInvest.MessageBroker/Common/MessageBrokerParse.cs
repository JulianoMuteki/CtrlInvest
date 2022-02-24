using CtrlInvest.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace CtrlInvest.MessageBroker.Common
{
    public class MessageBrokerParse
    {
        public static IList<PackageMessage> ConvertStringToList(string historicalPriceList, string ticket, Guid id)
        {
            IList<PackageMessage> packageMessages = new List<PackageMessage>();
            try
            {
               // _logger.LogInformation("Start send messega to Broker");
                using var sr = new StringReader(historicalPriceList);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    PackageMessage packageMessage = new PackageMessage()
                    {
                        Message = line,
                        TicketCode = ticket,
                        TicketID = id
                    };

                    //string message = JsonSerialize.JsonSerializer<PackageMessage>(packageMessage);
                    // _logger.LogInformation($"Send to Broker message: {message}");
                    packageMessages.Add(packageMessage);
                }

                return packageMessages;
            }
            catch (Exception ex)
            {
                throw ex;
              //  _logger.LogError(ex.Message);
            }
        }
    }
}
