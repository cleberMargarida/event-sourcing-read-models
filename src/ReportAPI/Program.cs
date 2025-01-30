using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Consumers;
using ReportAPI.Data;
using ReportAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<ReportContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("reportdb"), sqlServer => sqlServer.EnableRetryOnFailure()));

builder.Services.AddControllers();

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CustomerCreatedConsumer>();
    x.AddConsumer<BetSettledConsumer>(x => x.ConcurrentMessageLimit = 1);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

await app.SeedDataAsync<ReportContext>();

app.Run();
