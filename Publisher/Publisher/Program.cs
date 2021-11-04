using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = "Hello World";
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;


                while (true)
                {
                    channel.BasicPublish(exchange: "",
                     routingKey: "task_queue",
                     basicProperties: properties,
                     body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                    Thread.Sleep(3000);
                }

            }
        }
    }
}
