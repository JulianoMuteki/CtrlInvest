using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;

namespace CtrlInvest.MessageBroker
{
    public interface IRabbitFactoryConnection : IPooledObjectPolicy<IModel>, IDisposable
    {
    }
}
