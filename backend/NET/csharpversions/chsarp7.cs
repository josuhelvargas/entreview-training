


// 1. Tuplas Modernas (x, y)
// Antes (C# 6):
// csharp// Opción 1: Crear una clase completa (overkill)
public class ResultadoValidacion
{
    public bool EsValido { get; set; }
    public string Mensaje { get; set; }
}

public ResultadoValidacion ValidarUsuario(string email, string password)
{
    if (string.IsNullOrEmpty(email))
        return new ResultadoValidacion { EsValido = false, Mensaje = "Email vacío" };
    
    if (password.Length < 8)
        return new ResultadoValidacion { EsValido = false, Mensaje = "Password corto" };
    
    return new ResultadoValidacion { EsValido = true, Mensaje = "OK" };
}

// Opción 2: Usar Tuple (feo y sin nombres)
public Tuple<bool, string> ValidarUsuario2(string email, string password)
{
    // Acceso con .Item1, .Item2 - horrible
    return Tuple.Create(true, "OK");
}

////////////////////// Después (C# 7):
// csharp// ⭐ Tuplas con nombres descriptivos
public (bool esValido, string mensaje) ValidarUsuario(string email, string password)
{
    if (string.IsNullOrEmpty(email))
        return (false, "Email vacío");
    
    if (password.Length < 8)
        return (false, "Password debe tener al menos 8 caracteres");
    
    return (true, "Usuario válido");
}

// Uso limpio
var resultado = ValidarUsuario("juan@mail.com", "12345");
if (resultado.esValido)
{
    Console.WriteLine($"✓ {resultado.mensaje}");
}

// Deconstrucción (muy útil!)
var (esValido, mensaje) = ValidarUsuario("juan@mail.com", "password123");
if (!esValido)
{
    Console.WriteLine($"Error: {mensaje}");
}

// Casos realistas múltiples
public (int total, decimal promedio, decimal maximo) AnalisisVentas(List<decimal> ventas)
{
    return (
        total: ventas.Count,
        promedio: ventas.Average(),
        maximo: ventas.Max()
    );
}

// Múltiples valores de retorno en APIs
public (string token, DateTime expiracion, string refreshToken) GenerarTokens(Usuario usuario)
{
    var token = GenerateJWT(usuario);
    var expiracion = DateTime.Now.AddHours(2);
    var refreshToken = Guid.NewGuid().ToString();
    
    return (token, expiracion, refreshToken);
}

// Uso práctico
var (token, expira, refresh) = GenerarTokens(usuarioActual);
Response.Headers.Add("Authorization", $"Bearer {token}");

// Tuplas en LINQ
var estadisticas = empleados
    .Select(e => (
        nombre: e.Nombre,
        edad: e.Edad,
        antiguedad: DateTime.Now.Year - e.FechaIngreso.Year
    ))
    .Where(x => x.antiguedad > 5)
    .ToList();

// Diccionarios con tuplas como key
var cache = new Dictionary<(string userId, string resource), object>();
cache[(userId: "123", resource: "profile")] = datosUsuario;














//2. Pattern Matching (is, switch)
//Antes (C# 6):
// csharp
public decimal CalcularDescuento(object cliente)
{
    if (cliente is ClienteVIP)
    {
        ClienteVIP vip = (ClienteVIP)cliente; // ❌ Cast manual
        return vip.NivelDescuento * 100;
    }
    else if (cliente is ClienteRegular)
    {
        ClienteRegular regular = (ClienteRegular)cliente; // ❌ Cast manual
        return regular.AñosMiembro > 2 ? 10 : 5;
    }
    
    return 0;
}

// Switch tradicional limitado
public string ObtenerTipoAnimal(int tipo)
{
    switch (tipo)
    {
        case 1:
            return "Perro";
        case 2:
            return "Gato";
        default:
            return "Desconocido";
    }
}


////////////////////// Después (C# 7):
//csharp// ⭐ Pattern matching con 'is' - cast automático
public decimal CalcularDescuento(object cliente)
{
    if (cliente is ClienteVIP vip)  // ✅ Declara y castea en una línea
    {
        return vip.NivelDescuento * 100;
    }
    else if (cliente is ClienteRegular regular)
    {
        return regular.AñosMiembro > 2 ? 10 : 5;
    }
    else if (cliente is ClienteNuevo nuevo && nuevo.PrimeraCompra)
    {
        return 15; // Descuento de bienvenida
    }
    
    return 0;
}

// ⭐ Switch con patterns
public string ProcesarMensaje(object mensaje)
{
    switch (mensaje)
    {
        case string texto:
            return $"Texto recibido: {texto}";
        
        case int numero when numero > 0:
            return $"Número positivo: {numero}";
        
        case int numero when numero < 0:
            return $"Número negativo: {numero}";
        
        case DateTime fecha:
            return $"Fecha: {fecha:dd/MM/yyyy}";
        
        case null:
            return "Mensaje nulo";
        
        default:
            return "Tipo desconocido";
    }
}

// Caso realista: procesar respuestas de API
public void ProcesarRespuesta(object respuesta)
{
    switch (respuesta)
    {
        case HttpResponseMessage http when http.IsSuccessStatusCode:
            Console.WriteLine($"✓ Éxito: {http.StatusCode}");
            break;
        
        case HttpResponseMessage http when (int)http.StatusCode >= 400:
            Console.WriteLine($"✗ Error: {http.StatusCode}");
            break;
        
        case Exception ex when ex is TimeoutException:
            Console.WriteLine("⏱ Timeout - reintentando...");
            break;
        
        case Exception ex:
            Console.WriteLine($"✗ Error: {ex.Message}");
            break;
        
        default:
            Console.WriteLine("Respuesta desconocida");
            break;
    }
}

// Pattern matching en validaciones
public bool EsValido(object valor)
{
    return valor switch
    {
        string s when s.Length > 0 => true,
        int n when n > 0 => true,
        decimal d when d > 0m => true,
        _ => false
    };
}

// Caso realista: logging con types
public void Log(object data)  //muy util manejo de un generico u objeto de distitno tipo
{
    switch (data)
    {
        case Usuario usuario:
            Console.WriteLine($"[Usuario] {usuario.Nombre} - {usuario.Email}");
            break;
        
        case Producto producto when producto.Stock < 10:
            Console.WriteLine($"[Producto] ⚠ Stock bajo: {producto.Nombre}");
            break;
        
        case Producto producto:
            Console.WriteLine($"[Producto] {producto.Nombre} - Stock: {producto.Stock}");
            break;
        
        case List<string> lista:
            Console.WriteLine($"[Lista] {lista.Count} elementos");
            break;
        
        case var obj when obj != null:
            Console.WriteLine($"[Objeto] Tipo: {obj.GetType().Name}");
            break;
        
        case null:
            Console.WriteLine("[Null] Objeto nulo");
            break;
    }
}














// 3. Local Functions (Funciones Locales)
// Antes (C# 6):
// csharp
public class CalculadoraImpuestos
{
    // ❌ Método helper privado accesible en toda la clase
    private decimal CalcularISR(decimal salario)
    {
        if (salario <= 10000) return salario * 0.05m;
        if (salario <= 50000) return salario * 0.15m;
        return salario * 0.30m;
    }

    public decimal CalcularSalarioNeto(decimal salarioBruto)
    {
        var isr = CalcularISR(salarioBruto);
        var imss = salarioBruto * 0.03m;
        return salarioBruto - isr - imss;
    }
}
// Después (C# 7):
// csharp
public class CalculadoraImpuestos
{
    public decimal CalcularSalarioNeto(decimal salarioBruto)
    {
        // ⭐ Función local - solo visible aquí
        decimal CalcularISR(decimal salario)
        {
            if (salario <= 10000) return salario * 0.05m;
            if (salario <= 50000) return salario * 0.15m;
            return salario * 0.30m;
        }
        
        decimal CalcularIMSS(decimal salario) => salario * 0.03m;
        
        var isr = CalcularISR(salarioBruto);
        var imss = CalcularIMSS(salarioBruto);
        return salarioBruto - isr - imss;
    }
}

// Caso realista: validaciones complejas
public (bool esValido, List<string> errores) ValidarRegistro(Usuario usuario)
{
    var errores = new List<string>();
    
    // ⭐ Funciones locales para validaciones
    bool ValidarEmail()
    {
        if (string.IsNullOrEmpty(usuario.Email))
        {
            errores.Add("Email es requerido");
            return false;
        }
        if (!usuario.Email.Contains("@"))
        {
            errores.Add("Email inválido");
            return false;
        }
        return true;
    }
    
    bool ValidarPassword()
    {
        if (usuario.Password?.Length < 8)
        {
            errores.Add("Password debe tener al menos 8 caracteres");
            return false;
        }
        if (!usuario.Password.Any(char.IsDigit))
        {
            errores.Add("Password debe contener al menos un número");
            return false;
        }
        return true;
    }
    
    bool ValidarEdad()
    {
        if (usuario.Edad < 18)
        {
            errores.Add("Debes ser mayor de 18 años");
            return false;
        }
        return true;
    }
    
    // Ejecutar todas las validaciones
    bool emailValido = ValidarEmail();
    bool passwordValido = ValidarPassword();
    bool edadValida = ValidarEdad();
    
    return (emailValido && passwordValido && edadValida, errores);
}

// Recursión con funciones locales
public int Fibonacci(int n)
{
    // ⭐ Helper recursivo local
    int FibRecursivo(int num, int a, int b)
    {
        if (num == 0) return a;
        if (num == 1) return b;
        return FibRecursivo(num - 1, b, a + b);
    }
    
    return FibRecursivo(n, 0, 1);
}

// Procesamiento con múltiples pasos
public List<Producto> ProcesarInventario(List<Producto> productos)
{
    // Funciones locales para cada paso
    bool TieneStock(Producto p) => p.Stock > 0;
    bool EsActivo(Producto p) => p.Activo;
    decimal AplicarDescuento(decimal precio) => precio * 0.9m;
    
    return productos
        .Where(TieneStock)
        .Where(EsActivo)
        .Select(p => new Producto 
        { 
            Nombre = p.Nombre,
            Precio = AplicarDescuento(p.Precio),
            Stock = p.Stock
        })
        .ToList();
}












// 4. Out Variables con out var
// Antes (C# 6):
// csharp// ❌ Declarar variable antes
int resultado;
if (int.TryParse("123", out resultado))
{
    Console.WriteLine(resultado);
}

DateTime fecha;
if (DateTime.TryParse("2024-01-15", out fecha))
{
    Console.WriteLine(fecha);
}

// Diccionarios
string valor;
if (diccionario.TryGetValue("clave", out valor))
{
    Console.WriteLine(valor);
}


////////////////////// Después (C# 7):
// csharp// ⭐ Declaración inline con out var
if (int.TryParse("123", out var resultado))
{
    Console.WriteLine(resultado);
}

if (DateTime.TryParse("2024-01-15", out var fecha))
{
    Console.WriteLine(fecha.ToString("dd/MM/yyyy"));
}

// Caso realista: parsing de query strings
public void ProcesarParametros(Dictionary<string, string> parametros)
{
    if (parametros.TryGetValue("id", out var idString) && 
        int.TryParse(idString, out var id))
    {
        Console.WriteLine($"ID procesado: {id}");
    }
    
    if (parametros.TryGetValue("fecha", out var fechaString) &&
        DateTime.TryParse(fechaString, out var fecha))
    {
        Console.WriteLine($"Fecha: {fecha:dd/MM/yyyy}");
    }
}

// Validación de entrada de usuario
public decimal? ObtenerMonto(string input)
{
    if (decimal.TryParse(input, out var monto) && monto > 0)
    {
        return monto;
    }
    return null;
}

// Pattern matching + out var
public void ProcesarJSON(string json)
{
    if (JObject.Parse(json).TryGetValue("userId", out var userIdToken) &&
        int.TryParse(userIdToken.ToString(), out var userId))
    {
        Console.WriteLine($"Usuario ID: {userId}");
    }
}

// Múltiples out vars en una línea
if (int.TryParse(txtEdad.Text, out var edad) &&
    decimal.TryParse(txtSalario.Text, out var salario))
{
    Console.WriteLine($"Edad: {edad}, Salario: {salario:C}");
}














// 5. Ref Returns & Ref Locals (Performance)
// Concepto (C# 7):
// csharp// ⭐ ref returns - devuelve REFERENCIA, no copia
public class Inventario
{
    private int[] _stock = new int[1000];
    
 
    public void Ejemplo()
    {
        // ref local - trabaja directamente con la referencia
        ref int stockProducto = ref ObtenerStock(10);
        
        // Modificar la referencia modifica el array original
        stockProducto += 50; // _stock[10] ahora tiene 50 más
        
        Console.WriteLine(_stock[10]); // Cambió directamente
    }
}

// Caso realista: búsqueda y modificación en arrays grandes
public class GestorJugadores
{
    private Jugador[] _jugadores = new Jugador[10000];
    
    public ref Jugador BuscarJugador(int id)
    {
        for (int i = 0; i < _jugadores.Length; i++)
        {
            if (_jugadores[i].Id == id)
            {
                return ref _jugadores[i]; // Devuelve referencia
            }
        }
        throw new Exception("No encontrado");
    }
    
    public void ActualizarPuntaje(int jugadorId, int puntos)
    {
        ref Jugador jugador = ref BuscarJugador(jugadorId);
        jugador.Puntaje += puntos; // Modifica directamente el array
    }
}

// Performance: evitar copias de structs grandes
public struct Matriz3D
{
    public double X, Y, Z;
    public double[,] Datos; // Grande
}

public class ProcesadorMatrices
{
    private Matriz3D[] _matrices = new Matriz3D[1000];
    
    public ref Matriz3D ObtenerMatriz(int indice)
    {
        return ref _matrices[indice]; // Sin copia
    }
    
    public void Procesar()
    {
        ref Matriz3D matriz = ref ObtenerMatriz(5);
        matriz.X = 100; // Modifica directamente sin copiar struct
    }
}












// 6. Binary Literals y Digit Separators
// Antes (C# 6):
//csharp// ❌ Difícil de leer
int permisos = 255;
int mascara = 16777216;
long numeroGrande = 1000000000;


// Después (C# 7):
//csharp// ⭐ Binary literals (0b)
int permisoLectura    = 0b0001;  // 1
int permisoEscritura  = 0b0010;  // 2
int permisoEjecucion  = 0b0100;  // 4
int permisoAdmin      = 0b1000;  // 8

int todosPermisos = 0b1111;      // 15
int permisoRW = permisoLectura | permisoEscritura; // 0b0011

// ⭐ Digit separators (_) - legibilidad
int unMillon = 1_000_000;
long poblacionMundial = 8_000_000_000;
decimal salarioAnual = 450_000.50m;

// Combinados
int flags = 0b0001_0010_0100_1000;  // Grupos de 4 bits

// Casos realistas
public class ConfiguracionPermisos
{
    // Flags con binary literals
    public const int LEER     = 0b0000_0001;  // 1
    public const int ESCRIBIR = 0b0000_0010;  // 2
    public const int ELIMINAR = 0b0000_0100;  // 4
    public const int COMPARTIR = 0b0000_1000; // 8
    public const int ADMIN    = 0b1111_1111;  // 255
    
    public bool TienePermiso(int permisos, int permiso)
    {
        return (permisos & permiso) == permiso;
    }
}

// Uso
var config = new ConfiguracionPermisos();
int misPermisos = ConfiguracionPermisos.LEER | ConfiguracionPermisos.ESCRIBIR;

if (config.TienePermiso(misPermisos, ConfiguracionPermisos.LEER))
{
    Console.WriteLine("✓ Puede leer");
}

// Números grandes legibles
public class Constantes
{
    public const int TIMEOUT_MS = 30_000;              // 30 segundos
    public const long MAX_FILE_SIZE = 100_000_000;     // 100 MB
    public const decimal PRESUPUESTO = 5_000_000.00m;  // 5 millones
    
    // Máscaras de bits
    public const int MASCARA_RED = 0b1111_1111_0000_0000; // 255.0
    public const int IP_LOCALHOST = 0b0111_1111_0000_0000_0000_0000_0000_0001; // 127.0.0.1
}

// Colores RGB
public class Color
{
    public const int ROJO   = 0b1111_1111_0000_0000_0000_0000; // #FF0000
    public const int VERDE  = 0b0000_0000_1111_1111_0000_0000; // #00FF00
    public const int AZUL   = 0b0000_0000_0000_0000_1111_1111; // #0000FF
    public const int BLANCO = 0b1111_1111_1111_1111_1111_1111; // #FFFFFF
}