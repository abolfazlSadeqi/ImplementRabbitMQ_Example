
Implement RabbitMQ in an ASP.NET Core Step by step (With and without MassTransit)

## RabbitMQ
   1.is an open source message broker software
   
   2.Developed with Erlang
   
   3.It can also be used to implement a publish/subscribe pattern
   
   4.Available for all popular programming languages.
   

## Basic Functionalities

|Title|desc|
|--|--|
|Queue| is a buffer that stores messages.|	 
|Consumer |is a user application that receives messages|
|Publisher|is a user application that sends messages|	 
|Virtual Host|is used to separate the applications that are using RabbitMQ.	|
|Exchange Types |receiving messages from Producers and pushing them on the Queue depending on the rules defined in the Exchange type. 1.Direct 2.Fanout 3.Topic 4.Headers	
|Bindings |connection between queue and exchange.	 |


## Exchange Types

###	direct

A direct exchange delivers messages to queues based on the message routing key. A direct exchange is ideal for the unicast routing of messages. They can be used for multicast routing as well.

###	fanout

A fanout exchange routes messages to all of the queues that are bound to it and the routing key is ignored.Fanout exchanges are ideal for the broadcast routing of messages.

###	Topic

 route messages to one or many queues based on matching between a message routing key and the pattern that was used to bind a queue to an exchange. The topic exchange type is often used to implement various publish/subscribe pattern variations. Topic exchanges are commonly used for the multicast routing of messages.

### Headers

is designed for routing on multiple attributes that are more easily expressed as message headers than a routing key.Instead, the attributes used for routing are taken from the headers attribute. A message is considered matching if the value of the header equals the value specified upon binding.

## Default configuration

|Title|value|
|--|--|
|Endpoint address|	http://localhost|
|Port|	15672|
|username	|guest|
|Password	|guest|
|Windows Service Name|	RabbitMQ|


## Implementation Step by Step without use MassTransit

### Steps

1.	Install NuGet Package(s) into your Project
   
|Package|	It must be installed in the project|
|--|--|
|RabbitMQ.Client|	Producer,Consumer|

2.	Adding codes to connect to Rabbitmq
   
a)	Develop the base Method

 ```

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
```

b)use base Method

```

CommonRabbitmq commonRabbitmq = new CommonRabbitmq();

using var connection = commonRabbitmq.CreateConnection();
using var channel = connection.CreateModel();
```

3.	Add the code related to exchange
   
a)direct exchange

add code in Producer project(for publish Message)

```

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
```

add code in consumer project(for publish Message)

```

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
                     
```

b) Fanout exchange

add code in Producer project(for publish Message)

```
channel.ExchangeDeclare(Setting._exchangefanout, ExchangeType.Fanout);

var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(Setting._exchangefanout, string.Empty, null, body);

Console.WriteLine($"Sent {message}");
```

add code in consumer project(for publish Message)

```

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
```

c) Headers exchange

add code in Producer project(for publish Message)

```

channel.ExchangeDeclare(Setting._exchangeheader, ExchangeType.Headers, arguments: null);

var body = Encoding.UTF8.GetBytes(message);


var properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object> { { "type", "new" } };
channel.BasicPublish(Setting._exchangeheader, string.Empty, properties, body);
```

add code in consumer project(for publish Message)

```

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
```

d) Topic exchange

add code in Producer project(for publish Message)

```

channel.ExchangeDeclare(Setting._exchangetopic, ExchangeType.Topic, arguments: null);

var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(Setting._exchangetopic, Setting._bindingKeytopic_add, null, body);
add code in consumer project(for publish Message)
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
```


## Implementation Step by Step with MassTransit

### MassTransit
MassTransit is free software/open-source .NET-based Enterprise Service Bus software that helps .NET developers route messages over RabbitMQ, Azure Service Bus, SQS, and ActiveMQ service busses


### Steps

1.	Install NuGet Package(s) into your Project

|Package|	It must be installed in the project|
|--|--|
|MassTransit	Producer,Consumer|
|MassTransit.AspNetCore	|Producer|
|MassTransit.RabbitMQ	|Producer,Consumer|



2. Adding SettingRabbitMq in appsettings.json

```

"SettingRabbitMq": {
    "Url": "localhost",
    "UserName": "guest",
    "Password": "guest"
  }

```

3.Create Class Config

```

public class SettingRabbitMq
{
    public string Url { get; set; } 
    public string UserName { get; set; } 
    public string Password { get; set; } 
}

```


4.use config in program file

```

var _SettingRabbitMq = new SettingRabbitMq();
builder.Configuration.GetSection(nameof(SettingRabbitMq)).Bind(_SettingRabbitMq);

```

5.You can add and config RabbitMq 

```

builder.Services.AddMassTransit(mt => mt.AddMassTransit(x => {
    x.UsingRabbitMq((cntxt, cfg) => {
        cfg.Host(_SettingRabbitMq.Url, "/", c => {
            c.Username(_SettingRabbitMq.UserName);
            c.Password(_SettingRabbitMq.Password);
        });
    });
}));

```

6.add code in Producer project(for publish Message)

```
     await _publish.Publish<Customer>(new
        {
            Id = 1,
            FirstName = "test"+Guid.NewGuid().ToString(),
            LastName = "Test",
            age = 20
        });

```

7.add code in consumer project(for publish Message)

a)develop CustomerConsumer to receive message

```

public class CustomerConsumer : IConsumer<Customer>
{
    public async Task Consume(ConsumeContext<Customer> context)
    {
        var value = context.Message.LastName +" " + context.Message.FirstName;
      
    }
}

```

b) Introducing the consumer queue to MassTransit by registering the following code in the program file

```
        cfg.ReceiveEndpoint("Consumerqueue", (c) => {
            c.Consumer<CustomerConsumer>();
        });

```







