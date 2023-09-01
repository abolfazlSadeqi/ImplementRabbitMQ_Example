using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: Setting._queueNameDirect,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received {message}");
};
channel.BasicConsume(queue: Setting._queueNameDirect,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
