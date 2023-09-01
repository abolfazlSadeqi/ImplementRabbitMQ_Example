using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using System.Text;


string message = "Message Topic test with Time=" + DateTime.Now.ToLongTimeString();



CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangetopic, ExchangeType.Topic, arguments: null);

var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(Setting._exchangetopic, Setting._bindingKeytopic_add, null, body);

Console.WriteLine($"Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();