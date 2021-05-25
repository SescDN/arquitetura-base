using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using RabbitMQ.Client;
using Stefanini.Tracing;
using System.Collections.Generic;
using System.Text;

namespace Stefanini.Messaging.RabbitMQ.RabbitMQ
{
    public class RabbitMQQueueClient : IQueueClient
    {

        private IConnectionFactory _connectionFactory;
        private string _queueName;
        private readonly ITracer _tracer;
        private readonly ISharedTracing _sharedTracing;

        public RabbitMQQueueClient(IConnectionFactory connectionFactory, string queueName, ITracer tracer, ISharedTracing sharedTracing)
        {
            this._connectionFactory = connectionFactory;
            this._queueName = queueName;
            this._tracer = tracer;
            this._sharedTracing = sharedTracing;
        }

        public IConnection GetConnection()
        {
            return this._connectionFactory.CreateConnection();
        }

        public IModel GetModel(IConnection connection)
        {
            IModel model = connection.CreateModel();

            model.QueueDeclare(queue: this._queueName,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

            return model;
        }

        public void Publish<T>(string exchangeName, string routingKey, T content)
        {
            string serializedContent = JsonConvert.SerializeObject(content, Formatting.Indented);
            using (IConnection con = this.GetConnection())
            using (IModel model = this.GetModel(con))
            {
                using (var scope = _tracer.BuildSpan("RabbitMQ").StartActive(finishSpanOnDispose: true))
                {
                    var span = scope.Span.SetTag(Tags.SpanKind, Tags.SpanKindClient);

                    this._sharedTracing.CorrelationId = new Dictionary<string, string>();
                    _tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(this._sharedTracing.CorrelationId));

                    scope.Span.Log("Enviando dado para a fila.");

                    model.QueueDeclare(queue: this._queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                    IBasicProperties properties = model.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.Headers = new Dictionary<string, object>();
                    properties.Headers.Add("tracing", JsonConvert.SerializeObject(this._sharedTracing.CorrelationId));

                    byte[] payload = Encoding.UTF8.GetBytes(serializedContent);
                    model.BasicPublish(exchangeName, routingKey, properties, payload);
                }
            }
        }

        public string GetQueueName()
        {
            return this._queueName;
        }
    }
}
