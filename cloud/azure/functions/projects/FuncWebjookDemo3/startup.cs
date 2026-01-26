using FuncWebhookDemo.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FuncWebhookDemo.Startup))]

namespace FuncWebhookDemo;

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    var dbPath = Environment.GetEnvironmentVariable("SqliteDbPath") ?? "webhooks.db";

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlite($"Data Source={dbPath}"));
  }
}
