


// Ejemplo (Startup / Configure)
public void Configure(IApplicationBuilder app)
{
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapGet("/health", async ctx =>
        {
            await ctx.Response.WriteAsync("OK");
        });
    });
}

// Explicación:

// UseRouting() calcula cuál endpoint debe ejecutarse.

// UseEndpoints() “ejecuta” el endpoint elegido.

// AuthZ debe ir entre UseRouting() y UseEndpoints() para que aplique por endpoint.




// JSON nativo rápido: System.Text.Json
// Qué mejora: serialización más rápida y sin depender de Newtonsoft por default (aunque Newtonsoft sigue siendo opción).
// Ejemplo

//using System.Text.Json; <-sin newtonsoft

var payload = new { Name = "Josue", Age = 33 };
string json = JsonSerializer.Serialize(payload);

var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);


// Explicación:

// UseRouting() calcula cuál endpoint debe ejecutarse.

// UseEndpoints() “ejecuta” el endpoint elegido.

// AuthZ debe ir entre UseRouting() y UseEndpoints() para que aplique por endpoint.





/////////------> 3) gRPC soportado oficialmente en ASP.NET Core
// /// Explicación:

// gRPC usa HTTP/2.

// Útil cuando controlas cliente/servidor y quieres menor overhead que JSON/REST.
// /// 
/// 
/// 
/// 
/// 



// 4) Worker Service: template oficial para procesos background

// Qué mejora: patrón estándar para ETLs, schedulers, consumers de colas, etc., con DI + logging + config unificados.

// Ejemplo (Worker)
// using Microsoft.Extensions.Hosting;

public sealed class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // trabajo recurrente
            await Task.Delay(1000, stoppingToken);
        }
    }
}


// Explicación:

// BackgroundService ya trae el ciclo de vida correcto.

// Puedes inyectar servicios, DbContext, HttpClient, etc.


// 5) Desktop en .NET Core: WPF y WinForms (Windows)

// Qué mejora: posibilidad real de migrar apps de escritorio desde .NET Framework.

// Ejemplo (csproj)
// <PropertyGroup>
//   <TargetFramework>netcoreapp3.1</TargetFramework>
//   <UseWPF>true</UseWPF>
// </PropertyGroup>


// Explicación:

// Antes de 3.0 esto no existía en Core.

// Abrió la puerta a modernizar desktop sin quedarte en Framework.