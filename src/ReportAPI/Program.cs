using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;
using ReportAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<ReportContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("reportdb"), sqlServer => sqlServer.EnableRetryOnFailure()));

builder.Services.AddControllers();

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(Assembly.GetExecutingAssembly());

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });

    x.AddEntityFrameworkOutbox<ReportContext>(o =>
    {
        o.UseSqlServer();
        o.UseBusOutbox();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapControllers();

await app.SeedDataAsync<ReportContext>();

app.Run();
