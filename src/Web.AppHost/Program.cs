var builder = DistributedApplication.CreateBuilder(args);

var mssql = builder.AddSqlServer("mssql").WithDataVolume();

var betdb = mssql.AddDatabase("betdb");
var customerdb = mssql.AddDatabase("customerdb");
var reportdb = mssql.AddDatabase("reportdb");

var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();

var eventstore = builder.AddEventStore("eventstore").WithDataVolume();

builder.AddProject<Projects.BetAPI>("betapi")
       .WithReference(betdb)
       .WithReference(rabbitmq);

builder.AddProject<Projects.CustomerAPI>("customerapi")
       .WithReference(customerdb)
       .WithReference(rabbitmq);

builder.AddProject<Projects.ReportAPI>("reportapi")
       .WithReference(reportdb)
       .WithReference(eventstore)
       .WithReference(rabbitmq);

builder.AddProject<Projects.EventStoringAPI>("eventstoringapi")
       .WithReference(eventstore)
       .WithReference(rabbitmq);

builder.Build().Run();
