
using System;

namespace CtrlInvest.Import.HistoricalPrice
{
    public interface IMessageBroker: IDisposable
    {
        void Init();

        void SendMessageToRabbitMQ(string message);
    }
}
