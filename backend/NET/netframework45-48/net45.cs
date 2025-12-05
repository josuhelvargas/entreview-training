//.NET Framework 4.5
//C# 5.0


using System.Net.Http;
using System.Threading.Tasks;

public class Downloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    // .NET Framework 4.5 – C# 5
    public async Task<string> DownloadDataAsync(string url)
    {
        // No bloquea el hilo, pero se lee como código síncrono.
        string content = await _httpClient.GetStringAsync(url);

        // Aquí ya tenemos el resultado.
        return content;
    }
}


using System.Net.Http;
using System.Net.Http.Headers;

public static class HttpClientFactory
{
    // Patrón típico pre-IHttpClientFactory (pero reutilizando HttpClient)
    public static HttpClient Create(string token)
    {
        var client = new HttpClient();

        client.BaseAddress = new Uri("https://api.example.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }
}


// configuración en app.config / web.config
<?xml version="1.0"?>
<configuration>
  <runtime>
    <!-- Server GC para apps de servidor (IIS / servicios Windows) -->
    <gcServer enabled="true" />

    <!-- GC concurrent/background habilitado (workstation) -->
    <gcConcurrent enabled="true" />
  </runtime>
</configuration>


// gcServer enabled="true" → más threads para GC, mejor para servidores con varios núcleos.

// gcConcurrent enabled="true" → permite que el GC de fondo trabaje mientras la app sigue



public async Task<int> ProcessAndDownloadAsync(string url, int n)
{
    // CPU-bound work in ThreadPool
    int calc = await Task.Run(() =>
    {
        int sum = 0;
        for (int i = 0; i < n; i++)
        {
            sum += i;
        }
        return sum;
    });

    // I/O bound con async/await
    string data = await new Downloader().DownloadDataAsync(url);

    return calc + data.Length;
}
// En .NET 4.5, el algoritmo de “hill climbing” del ThreadPool decide mejor cuándo crear más threads, por lo que este tipo de código escala mucho mejor que en 4.0.



using System.Net.Http;
using System.Threading.Tasks;

public class WeatherService
{
    private static readonly HttpClient _http = new HttpClient
    {
        BaseAddress = new Uri("https://api.weather.com/")
    };

    public async Task<string> GetForecastAsync(string city)
    {
        // GET api/forecast?city={city}
        var response = await _http.GetAsync("api/forecast?city=" + city);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}


// resumen: ini-resumen para entrevista (versión con código en mente)

// async/await (C# 5, .NET 4.5)
// → Escribes código asíncrono legible, sobre Task, apoyado por el compilador con state machines. Ejemplo: HttpClient.GetStringAsync, FileStream.CopyToAsync, WinForms/WPF async handlers.

// ThreadPool mejorado
// → Introduce un algoritmo de “hill climbing” que escala mejor con Task.Run y TPL; tu código Task.Run(...); await ... aprovecha ese motor sin que tengas que configurarlo.

// HttpClient
// → API orientada a Task, reutilizable y extensible, ideal para microservicios y llamadas REST: await client.GetAsync, await response.Content.ReadAsStringAsync().

// Background GC mejorado
// → Menos pausas, GC concurrente/full de Gen2 con interrupciones más pequeñas; configurable con <gcServer> y <gcConcurrent>, y ajustable con GCSettings.LatencyMode.