using CustomerAPI.Data;
using CustomerAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<CustomerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("customerdb"), sqlServer => sqlServer.EnableRetryOnFailure()));

builder.Services.AddControllers();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddMassTransit(x => 
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
