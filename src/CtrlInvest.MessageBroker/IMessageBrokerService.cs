
using RabbitMQ.Client;
using System;

namespace CtrlInvest.MessageBroker
{
    public interface IMessageBrokerService: IDisposable
    {        
        void DoReceiveOperation();
        void DoSendMessageOperation(string message);
        event EventHandler ProcessCompleted;
    }
}
