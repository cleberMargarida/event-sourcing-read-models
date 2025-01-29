using Projects;

var builder = DistributedApplication.CreateBuilder();

builder.AddProject("", new Projects.BetAPI().ProjectPath);
builder.AddProject("", new Projects.CustomerAPI().ProjectPath);
builder.AddProject("", new Projects.ReportAPI().ProjectPath);