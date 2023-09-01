using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangeheader, ExchangeType.Headers);
channel.QueueDeclare(Setting._queueNameheader,
 durable: true,
 exclusive: false,
 autoDelete: false,
 arguments: null);

var header = new Dictionary<string, object> { { "type", "new" } };
channel.QueueBind(Setting._queueNameheader, Setting._exchangeheader, string.Empty, header);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Recive=" + message);
};

channel.BasicConsume(Setting._queueNameheader, true, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();