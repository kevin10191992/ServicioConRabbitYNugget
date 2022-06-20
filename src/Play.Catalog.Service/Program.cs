using MassTransit;
using Microsoft.Extensions.Configuration;
using Play.Catalog.Service.Config;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
string ServiceName = "Play.Catalog.Service";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(c =>
{
    c.AddConsumers(Assembly.GetEntryAssembly());

    c.UsingRabbitMq((context, configurator) =>
    {
        RabbitConfig conf = builder.Configuration.GetSection("RabbitConfig").Get<RabbitConfig>();

        configurator.Host(conf.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(ServiceName, false));
        configurator.UseMessageRetry(op =>
        {
            op.Interval(3, TimeSpan.FromSeconds(5));
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
