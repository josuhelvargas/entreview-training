//Features dotnet core 2
//se logro retrocompatibilidad con apis previas de .net framework: ejem ADO.NET




using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using System.Drawing;
using System.Drawing.Imaging;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//anotaciones para campos ([Required, SrtingLength, EmailAddress, Range(18,100) edad DisplayName("Fecha de nacimiento ")])

using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

using System.Configuration;


using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;


using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;

using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;
using System.IO.Compression;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;




int[] nums = { 1, 5, 3, 9, 2 };
ref int max = ref UsarRefReturns.FindMax(nums);
max = 100; // Modifica el array original

var color = Color.FromArgb(255, 100, 50, 25);
var point = new Point(10, 20);
var size = new Size(100, 200);
var rectangle = new Rectangle(point, size);



var XmlDocument = new XmlDocument();
XmlDocument.LoadXml("<root><element>Josue</element></root>");
var elementValue = XmlDocument.SelectSingleNode("/root/element")?.InnerText;
if (elementValue != null)
{
  Console.WriteLine($"XmlDocument Element Value: {elementValue}");
}

// ConfigurationManager (necesita paquete adicional)
// Install-Package System.Configuration.ConfigurationManager

//var connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"]?.ConnectionString; 

//criptographic algorithms

// MD5
// using (var md5 = MD5.Create())
// {
//   var hash = md5.ComputeHash(Encoding.UTF8.GetBytes("password"));
//   var hashString = BitConverter.ToString(hash).Replace("-", "");
// }

// // SHA256
// using (var sha256 = SHA256.Create())
// {
//   var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes("password"));
// }

// // RSA
// using (var rsa = RSA.Create())
// {
//   var publicKey = rsa.ExportParameters(false);
//   var privateKey = rsa.ExportParameters(true);
// }

// // AES
// using (var aes = Aes.Create())
// {
//   aes.Key = GenerateKey();
//   aes.IV = GenerateIV();

//   using (var encryptor = aes.CreateEncryptor())
//   {
//     // Encriptar datos
//   }
// }


Span<byte> buffer = stackalloc byte[256];
Span<byte> slicedData = buffer.Slice(0, 128);
slicedData.Fill(0xFF);
foreach (var b in slicedData)
{
  Console.Write($"{b:X2} ");
}

Memory<byte> memoryBuffer = new byte[512];
memoryBuffer.GetType().GetProperty("Length");





public ref struct BufferReader
{
  private Span<byte> _buffer;

  public BufferReader(Span<byte> buffer)
  {
    _buffer = buffer;
  }
}

// Ref returns para evitar copias
public class UsarRefReturns
{
  public ref int FindMax(int[] numbers)
  {
    int maxIndex = 0;
    for (int i = 1; i < numbers.Length; i++)
    {
      if (numbers[i] > numbers[maxIndex])
        maxIndex = i;
    }
    return ref numbers[maxIndex];
  }

}
// Uso

public class CacheExample
{
  private readonly Dictionary<string, int> _cache = new Dictionary<string, int>();
  public async ValueTask<int> GetCachedValueAsync(string key)
  {
    if (_cache.TryGetValue(key, out int value))
    {
      // Retorno síncrono sin allocation de Task
      return value;
    }

    // Solo se crea Task si es necesario
    return await FetchFromDatabaseAsync(key);
  }
}

//Muestra un ejemplo practico de como utilizar Span y Memory en una coslucion concreta y por que no otro tipo de estructura. 


//Muestra un ejemplo practico de como utilizar Ref Structs para alto redmimiento.Se rleaciona con el hecho de tener grandes cantidades de datos en memoria en listas por ejemplo? objetos en forma de listas grandes?

//Explica por que valuetask es mas eficiente que Task en operaciones sincronas. Da un ejemplo practico y real de su usu.


//Que es con exactitud IActionResult en ASP.NET Core y como se utiliza para manejar respuestas HTTP en controladores web. Proporciona un ejemplo de codigo que demuestre su uso en una aplicacion web real.

//que es lo que hace:  services.AddAntiforgery(options =>
// {
//   options.HeaderName = "X-CSRF-TOKEN";
// });

//dame mas detalles dde como debe estar estrucurrado el stratup (configureServices y configure)

//como se utiliza el onbjeto cultureinfo y 
// CultureInfo.CurrentCulture = culture;
// CultureInfo.CurrentUICulture = culture;

//Como se utiliza PageModel?

//Como funciona IConfiguration 

//explica la difernecia de los scopes singletons scodped, trqnsient y transient con provider.


//explica como funciona signalR
//explica cmo funciona usehttpredirection /usehsts
public class IndexModel : PageModel
{
  [BindProperty]
  public string Name { get; set; }

  public void OnGet()
  {
    // Lógica para GET
  }

  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
      return Page();

    // Procesar formulario
    return RedirectToPage("./Success");
  }
}


// Startup.cs mejorado
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    // Soporte mejorado para antiforgery
    services.AddAntiforgery(options =>
    {
      options.HeaderName = "X-CSRF-TOKEN";
    });

    // Razor Pages
    services.AddRazorPages();

    // MVC con compatibilidad 2.0
    services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_0);
  }
}


public class CultureUsage
{
  public void ConfigureCulture()
  {
    var culture = new CultureInfo("es-MX");
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;

    decimal price = 1234.56m;
    Console.WriteLine(price.ToString("C")); // $1,234.56
  }
  public void ShowDateInUsCulture(DateTime date)
  {
    var usCulture = new CultureInfo("en-US");
    string formattedDate = date.ToString("D", usCulture);
    Console.WriteLine($"Date in US format: {formattedDate}");
  }
}