using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using System.Text;

string message = "Message header test with Time=" + DateTime.Now.ToLongTimeString();


CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangeheader, ExchangeType.Headers, arguments: null);

var body = Encoding.UTF8.GetBytes(message);


var properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object> { { "type", "new" } };
channel.BasicPublish(Setting._exchangeheader, string.Empty, properties, body);


Console.WriteLine($"Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();