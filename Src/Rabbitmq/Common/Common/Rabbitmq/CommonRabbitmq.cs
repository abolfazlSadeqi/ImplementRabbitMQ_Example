using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rabbitmq;

public class CommonRabbitmq
{
    public IModel CreateModel(IConnection connection) => connection.CreateModel();
   
    public IConnection CreateConnection()
    {
        string HostName = "localhost";
        string UserName = "guest";
        string Password = "guest";
        string VirtualHost = "TestApp";

        var factory = new ConnectionFactory { HostName = HostName, UserName = UserName, Password = Password, VirtualHost = VirtualHost };
        return factory.CreateConnection();
    }
        
}
