using BetAPI.Data;
using BetAPI.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<BetContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("betdb"), sqlServer => sqlServer.EnableRetryOnFailure()));

builder.Services.AddControllers();

builder.Services.AddScoped<IBetService, BetService>();

builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
