
using Common.Model;
using Consumer.Model;
using MassTransit;
using MassTransit.RabbitMqTransport;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var _SettingRabbitMq = new SettingRabbitMq();
builder.Configuration.GetSection(nameof(SettingRabbitMq)).Bind(_SettingRabbitMq);


builder.Services.AddMassTransit(mt => mt.AddMassTransit(x => {
    x.UsingRabbitMq((cntxt, cfg) => {
        cfg.Host(_SettingRabbitMq.Url, "/", c => {
            c.Username(_SettingRabbitMq.UserName);
            c.Password(_SettingRabbitMq.Password);
        });

        cfg.ReceiveEndpoint("Consumerqueue", (c) => {
            c.Consumer<CustomerConsumer>();
        });
    });
}));

builder.Services.AddControllers();
// Add services to the container.

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

