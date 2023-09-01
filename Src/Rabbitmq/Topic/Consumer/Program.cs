using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;



CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangetopic, ExchangeType.Topic);
channel.QueueDeclare(Setting._queueNametopic,
 durable: true,
 exclusive: false,
 autoDelete: false,
 arguments: null);

channel.QueueBind(Setting._queueNametopic, Setting._exchangetopic, Setting._bindingKeytopic_all );

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received {message}");
};

channel.BasicConsume(Setting._queueNametopic, true, consumer);
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
