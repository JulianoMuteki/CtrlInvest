﻿
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.MessageBroker
{
    public interface IMessageBrokerService : IDisposable
    {
        void SetQueueChannel(string queueName);
        void DoSendMessageOperation(string message);
        event EventHandler<string> ProcessCompleted;
        void DoReceiveMessageOperation();
        bool isExpireTimeToReceiveMessage();
        bool ChannelConnectionIsOpen();

    }
}
