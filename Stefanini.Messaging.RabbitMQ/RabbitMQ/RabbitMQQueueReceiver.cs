using Newtonsoft.Json;
using OpenTracing;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Stefanini.Tracing;
using Stefanini.Tracing.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stefanini.Messaging.RabbitMQ.RabbitMQ
{
    public class RabbitMQQueueReceiver<T> : IQueueReceiver<T>
    {
        private IQueueClient _client;
        private IModel model;
        private IConnection con;
        private readonly ITracer _tracer;
        private readonly ISharedTracing _sharedTracing;

        public RabbitMQQueueReceiver(IQueueClient client, ITracer tracer, ISharedTracing sharedTracing)
        {
            this._client = client;
            _tracer = tracer;
            this._sharedTracing = sharedTracing;
        }

        Func<T, Task> _action;

        public void OnReceiver(Func<T, Task> action)
        {
            this._action = action;
        }

        public void Start()
        {
            var retryOnStartupPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(9, retryAttempt =>
                     TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                 );

            retryOnStartupPolicy.Execute(() =>
            {
                con = this._client.GetConnection();
                model = this._client.GetModel(con);

                EventingBasicConsumer consumer = new EventingBasicConsumer(model);
                consumer.Received += async (m, ea) =>
                {
                    System.Console.WriteLine("Processando item da fila.");
                    this._sharedTracing.CorrelationId = JsonConvert.DeserializeObject<IDictionary<string, string>>(Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["tracing"]));

                    using (var scope = TracingExtension.StartServerSpan(_tracer, this._sharedTracing.CorrelationId, "Tracing RabbitMQ"))
                    {
                        scope.Span.Log("Consumindo a fila");
                        T oEvent = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ea.Body.ToArray()));
                        try
                        {
                            await this._action(oEvent);
                            model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            System.Console.WriteLine("Processado item da fila.");
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine("Erro ao processar item da fila. - " + e.Message);
                            model.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                        }
                    }
                };

                model.BasicConsume(queue: this._client.GetQueueName(),
                                        autoAck: false,
                                        consumer: consumer);
            });
        }
    }
}
