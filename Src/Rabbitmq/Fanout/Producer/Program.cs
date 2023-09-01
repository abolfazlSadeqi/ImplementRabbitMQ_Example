using Common;
using Common.Rabbitmq;
using RabbitMQ.Client;
using System.Text;

string message = "Message fanout test with Time=" + DateTime.Now.ToLongTimeString();

CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(Setting._exchangefanout, ExchangeType.Fanout);



var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(Setting._exchangefanout, string.Empty, null, body);

Console.WriteLine($"Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();




