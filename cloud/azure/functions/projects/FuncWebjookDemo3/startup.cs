using FuncWebhookDemo.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FuncWebhookDemo.Startup))] //“Cuando inicies el host de Functions, ejecuta esta clase Startup para registrar dependencias.”

namespace FuncWebhookDemo;

public class Startup : FunctionsStartup  //👉 Configurar el contenedor de dependencias (DI) 👉 Antes de que se ejecute cualquier Function
{
  public override void Configure(IFunctionsHostBuilder builder) //aranca encuentra functionstartup, registras servicios , host se termina de cosntruir, host ejecuta funciones
  {
    var dbPath = Environment.GetEnvironmentVariable("SqliteDbPath") ?? "webhooks.db";

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlite($"Data Source={dbPath}")); //Tus Functions pueden recibir AppDbContext por constructor (si son clases) o servicios que lo usen
  }
}
