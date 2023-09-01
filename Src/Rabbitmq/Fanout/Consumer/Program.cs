using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangefanout, ExchangeType.Fanout);
channel.QueueDeclare(Setting._queueNamefanout,
 durable: true,
 exclusive: false,
 autoDelete: false,
 arguments: null);

channel.QueueBind(Setting._queueNamefanout, Setting._exchangefanout, string.Empty);
channel.BasicQos(0, 10, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};

channel.BasicConsume(Setting._queueNamefanout, true, consumer);
Console.WriteLine(" press [enter] to exit.");
Console.ReadLine();




