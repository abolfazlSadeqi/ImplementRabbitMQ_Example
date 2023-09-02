using Common.Model;
using MassTransit;
using Newtonsoft.Json;

namespace Consumer.Model;


public class CustomerConsumer : IConsumer<Customer>
{
    public async Task Consume(ConsumeContext<Customer> context)
    {
        var value = context.Message.LastName +" " + context.Message.FirstName;
      
    }
}
