using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
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
                    channel.QueueDeclare(queue: "Hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    for(int i =0; i < 15000; i++)
                    {
                        Entities.Pessoa pes = new Entities.Pessoa()
                        {
                            Idade = new Random().Next(),
                            Nome = "Moretti"
                        };

                        var body = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(pes));

                        channel.BasicPublish("", "Hello", true, null, body);
                        Console.WriteLine("Enviou {0}", body);
                        System.Threading.Thread.Sleep(3000);
                    }

                    Console.WriteLine("[x] Mensagem enviada!");
                }
            }

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
