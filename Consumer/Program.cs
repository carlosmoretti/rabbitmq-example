using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var con = factory.CreateConnection())
            {
                using (var channel = con.CreateModel())
                {
                    channel.QueueDeclare("Hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) => {
                        var body = ea.Body;
                        Entities.Pessoa pes = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.Pessoa>(Encoding.UTF8.GetString(body));
                        Console.WriteLine("[x] Received {0}", string.Format("{0} | {1}", pes.Nome, pes.Idade));
                    };

                    channel.BasicConsume(queue: "Hello", autoAck: true, consumer: consumer);

                    Console.WriteLine("Press [Enter] to exit");
                    Console.Read();
                }
            }
        }
    }
}
