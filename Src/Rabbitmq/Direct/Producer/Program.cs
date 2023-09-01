using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using System.Text;

const string message = "Test Direct!";


CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: Setting._queueNameDirect,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: Setting._queueNameDirect,
                     basicProperties: null,
                     body: body);
Console.WriteLine($" Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
