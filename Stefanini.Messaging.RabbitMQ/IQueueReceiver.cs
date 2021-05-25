using System;
using System.Threading.Tasks;

namespace Stefanini.Messaging.RabbitMQ
{
    public interface IQueueReceiver<T>
    {
        void OnReceiver(Func<T, Task> action);
        void Start();
    }
}
