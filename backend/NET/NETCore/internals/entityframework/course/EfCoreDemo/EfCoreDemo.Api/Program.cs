using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration; //config clave-valor Iconfiguratin  .AddConfiguration(); por default toma el appsettings
Console.WriteLine(config);


// builder.Environment.ApplicationName

// builder.Host   //app completa
// Configuras logging
// Integras Azure
// Agregas background services
// Cambias el ciclo de vida
// Configuras contenedores externos
//builder.Host.UseContentRoot()

//builder.Metrics

// Medir requests por segundo

// Medir latencia

// Medir uso de memoria

// Medir exceptions

// Exportar a Prometheus

// Exportar a Azure Monitor

// Exportar a Grafana



//builder.Services //dependencias y di en tiempo de ejecucion seran resualtas
// Singleton	Toda la app
// Scoped	Por request
// Transient	Cada vez que se pide



//builder.WebHost.UseStartup<Startup>(); //servidor http
// Configuras Kestrel

// Cambias puertos

// Configuras certificados

// Configuras IIS

// builder.Services.AddDbContext<AppDbContext>(opt =>
// {
//     opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build(); //webapp builder is built


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();

