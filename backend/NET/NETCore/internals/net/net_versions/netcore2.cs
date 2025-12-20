// //.NET Standard 2.0
// // Mayor compatibilidad con .NET Framework APIs
// // Ahora se pueden usar más de 20,000 APIs adicionales

// using System.Data;
// using System.Drawing; 

// var dataTable = new DataTable();
// dataTable.Columns.Add("Name", typeof(string));
// dataTable.Rows.Add("John");


// // 2. Span<T> y Memory<T>
// // csharp// Manejo eficiente de memoria sin allocations
// public void ProcessData(Span<byte> buffer)
// {
//     // Slice sin copiar datos
//     Span<byte> slice = buffer.Slice(0, 10);
    
//     // Operaciones eficientes
//     for (int i = 0; i < slice.Length; i++)
//     {
//         slice[i] = (byte)(slice[i] * 2);
//     }
// }

// // Uso con arrays
// byte[] data = new byte[100];
// Span<byte> span = data;
// ProcessData(span);
// 3. Ref Structs y Ref Returns
// csharp// Ref struct para alto rendimiento
// public ref struct BufferReader
// {
//     private Span<byte> _buffer;
    
//     public BufferReader(Span<byte> buffer)
//     {
//         _buffer = buffer;
//     }
// }

// // Ref returns para evitar copias public ref int FindMax(int[] numbers)
// {
//     int maxIndex = 0;
//     for (int i = 1; i < numbers.Length; i++)
//     {
//         if (numbers[i] > numbers[maxIndex])
//             maxIndex = i;
//     }
//     return ref numbers[maxIndex];
// }

// // Uso
// int[] nums = { 1, 5, 3, 9, 2 };
// ref int max = ref FindMax(nums);
// max = 100; // Modifica el array original
// 4. ValueTask<T>
// csharp// Alternativa más eficiente a Task<T> para operaciones síncronas
// public async ValueTask<int> GetCachedValueAsync(string key)
// {
//     if (_cache.TryGetValue(key, out int value))
//     {
//         // Retorno síncrono sin allocation de Task
//         return value;
//     }
    
//     // Solo se crea Task si es necesario
//     return await FetchFromDatabaseAsync(key);
// }
// 5. Razor Pages
// csharp// Modelo de programación simplificado para páginas web
// // Pages/Index.cshtml.cs
// public class IndexModel : PageModel
// {
//     [BindProperty]
//     public string Name { get; set; }
    
//     public void OnGet()
//     {
//         // Lógica para GET
//     }
    
//     public IActionResult OnPost()
//     {
//         if (!ModelState.IsValid)
//             return Page();
            
//         // Procesar formulario
//         return RedirectToPage("./Success");
//     }
// }
// 6. Mejoras en ASP.NET Core
// csharp// Startup.cs mejorado
// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Soporte mejorado para antiforgery
//         services.AddAntiforgery(options =>
//         {
//             options.HeaderName = "X-CSRF-TOKEN";
//         });
        
//         // Razor Pages
//         services.AddRazorPages();
        
//         // MVC con compatibilidad 2.0
//         services.AddMvc()
//             .SetCompatibilityVersion(CompatibilityVersion.Version_2_0);
//     }
// }
// 7. IHttpClientFactory
// csharp// Manejo correcto de HttpClient
// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Registro de HttpClient
//         services.AddHttpClient("github", c =>
//         {
//             c.BaseAddress = new Uri("https://api.github.com/");
//             c.DefaultRequestHeaders.Add("Accept", "application/json");
//             c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
//         });
//     }
// }

// // Uso en controlador
// public class HomeController : Controller
// {
//     private readonly IHttpClientFactory _clientFactory;
    
//     public HomeController(IHttpClientFactory clientFactory)
//     {
//         _clientFactory = clientFactory;
//     }
    
//     public async Task<IActionResult> Index()
//     {
//         var client = _clientFactory.CreateClient("github");
//         var response = await client.GetStringAsync("repos/dotnet/core");
//         return Content(response);
//     }
// }
// 8. Globalization Improvements
// csharp// Mejor soporte para internacionalización
// using System.Globalization;

// public void ConfigureCulture()
// {
//     var culture = new CultureInfo("es-MX");
//     CultureInfo.CurrentCulture = culture;
//     CultureInfo.CurrentUICulture = culture;
    
//     decimal price = 1234.56m;
//     Console.WriteLine(price.ToString("C")); // $1,234.56
// }
// 9. Configuration Improvements
// csharp// appsettings.json más flexible
// public class Startup
// {
//     public Startup(IConfiguration configuration)
//     {
//         Configuration = configuration;
//     }
    
//     public IConfiguration Configuration { get; }
    
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Binding fuerte de configuración
//         services.Configure<AppSettings>(
//             Configuration.GetSection("AppSettings"));
//     }
// }

// public class AppSettings
// {
//     public string ConnectionString { get; set; }
//     public int MaxRetries { get; set; }
//     public LogLevel LogLevel { get; set; }
// }
// 10. Mejoras en Entity Framework Core 2.0
// csharp// Owned Types
// public class Order
// {
//     public int Id { get; set; }
//     public Address ShippingAddress { get; set; }
// }

// [Owned]
// public class Address
// {
//     public string Street { get; set; }
//     public string City { get; set; }
//     public string ZipCode { get; set; }
// }

// // DbContext
// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {
//     modelBuilder.Entity<Order>()
//         .OwnsOne(o => o.ShippingAddress);
// }

// // Table Splitting
// modelBuilder.Entity<Order>()
//     .HasOne(o => o.Details)
//     .WithOne(d => d.Order)
//     .HasForeignKey<OrderDetails>(d => d.OrderId);
    
// modelBuilder.Entity<Order>()
//     .ToTable("Orders");
    
// modelBuilder.Entity<OrderDetails>()
//     .ToTable("Orders");
// 11. Logging Improvements
// csharp// Logging más flexible
// public class HomeController : Controller
// {
//     private readonly ILogger<HomeController> _logger;
    
//     public HomeController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }
    
//     public IActionResult Index()
//     {
//         _logger.LogInformation("Index page visited at {Time}", 
//             DateTime.UtcNow);
            
//         _logger.LogWarning("This is a warning with {Data}", 
//             new { Id = 1, Name = "Test" });
            
//         return View();
//     }
// }
// 12. Dependency Injection Enhancements
// csharp// Scopes mejorados
// public void ConfigureServices(IServiceCollection services)
// {
//     // Singleton
//     services.AddSingleton<ISingletonService, SingletonService>();
    
//     // Scoped (por request)
//     services.AddScoped<IScopedService, ScopedService>();
    
//     // Transient (cada vez)
//     services.AddTransient<ITransientService, TransientService>();
    
//     // Factory pattern
//     services.AddTransient<IMyService>(provider =>
//     {
//         var config = provider.GetRequiredService<IConfiguration>();
//         return new MyService(config["ApiKey"]);
//     });
// }
// 13. Performance Improvements
// csharp// ArrayPool para reutilización de arrays
// using System.Buffers;

// public void ProcessLargeData()
// {
//     // Alquilar array del pool
//     var buffer = ArrayPool<byte>.Shared.Rent(1024);
    
//     try
//     {
//         // Usar buffer
//         ProcessBuffer(buffer);
//     }
//     finally
//     {
//         // Devolver al pool
//         ArrayPool<byte>.Shared.Return(buffer);
//     }
// }
// 14. SignalR
// csharp// Hub para comunicación en tiempo real
// public class ChatHub : Hub
// {
//     public async Task SendMessage(string user, string message)
//     {
//         await Clients.All.SendAsync("ReceiveMessage", user, message);
//     }
    
//     public async Task SendToGroup(string groupName, string message)
//     {
//         await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
//     }
// }

// // Startup
// public void ConfigureServices(IServiceCollection services)
// {
//     services.AddSignalR();
// }

// public void Configure(IApplicationBuilder app)
// {
//     app.UseSignalR(routes =>
//     {
//         routes.MapHub<ChatHub>("/chatHub");
//     });
// }
// 15. HTTPS por Defecto
// csharp// Redirección HTTPS automática
// public void Configure(IApplicationBuilder app)
// {
//     app.UseHttpsRedirection();
//     app.UseHsts(); // HTTP Strict Transport Security
// }

// public void ConfigureServices(IServiceCollection services)
// {
//     services.AddHsts(options =>
//     {
//         options.MaxAge = TimeSpan.FromDays(365);
//         options.IncludeSubDomains = true;
//     });
// }