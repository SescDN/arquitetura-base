using System;
using System.Text;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using RabbitMQ.Client;

namespace Stefanini.Log.Target
{
    [Target("RabbitMQ")]
    public class RabbitMQTarget : TargetWithLayout
    {
        public RabbitMQTarget()
        {
            this.HostName = "localhost";
            this.Port = 5672;
            this.Exchange = "";
            this.RoutingKey = "";
            this.VirtualHost = "";
        }

        public string HostName { get; set; }

        [RequiredParameter]
        public string UserName { get; set; }

        [RequiredParameter]
        public string Password { get; set; }

        public int Port { get; set; }

        public string Exchange { get; set; }

        [RequiredParameter]
        public string RoutingKey { get; set; }

        public string VirtualHost { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);

            var factory = new ConnectionFactory() { HostName = this.HostName, VirtualHost = this.VirtualHost, Port = this.Port, UserName = this.UserName, Password = this.Password };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(logMessage);

                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;

                    channel.BasicPublish(exchange: this.Exchange,
                                         routingKey: this.RoutingKey,
                                         basicProperties: null,
                                         body: body);
                }
            }
        }
    }
}
