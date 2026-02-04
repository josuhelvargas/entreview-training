using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);//lee base config se appsettings, DI

builder.ConfigureFunctionsWebApplication();//monta http pipeline

builder.Services
    .AddApplicationInsightsTelemetryWorkerService() //temlemetria
    .ConfigureFunctionsApplicationInsights();//config re correlaciond e requests con appinsights

builder.Build().Run();
