
//1. Interpolated Strings ($"")

string nombre = "Carlos";
int edad = 28;
decimal salario = 45000.50m;

// Concatenación horrible
string mensaje1 = "Nombre: " + nombre + ", Edad: " + edad + ", Salario: $" + salario;

// String.Format verboso
string mensaje2 = string.Format("Nombre: {0}, Edad: {1}, Salario: ${2}", nombre, edad, salario);



//2.
string nombre = "Carlos";
int edad = 28;
decimal salario = 45000.50m;

// Mucho más limpio y legible
string mensaje = $"Nombre: {nombre}, Edad: {edad}, Salario: ${salario:N2}";

// Con expresiones
string info = $"En 5 años tendrás {edad + 5} años";

// Logs realistas
Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Usuario {nombre} inició sesión");



///////////////////////////////////////////////////////////////////////////////
//Null conditional operator (?.)

public class Empleado
{
    public string Nombre { get; set; }
    public Direccion Direccion { get; set; }
}

public class Direccion
{
    public string Ciudad { get; set; }
}

// Código defensivo horrible
string ciudad = null;
if (empleado != null)
{
    if (empleado.Direccion != null)
    {
        ciudad = empleado.Direccion.Ciudad;
    }
}

// Llamadas a métodos
int? longitud = null;
if (empleado != null && empleado.Nombre != null)
{
    longitud = empleado.Nombre.Length;
}





/* con c#6 */

// Acceso a propiedades anidadas
string ciudad = empleado?.Direccion?.Ciudad;

// Llamadas a métodos
int? longitud = empleado?.Nombre?.Length;

// Invocación de eventos (muy útil!)
public event EventHandler CambioDetectado;

// Antes: if (CambioDetectado != null) CambioDetectado(this, EventArgs.Empty);
// Ahora:
CambioDetectado?.Invoke(this, EventArgs.Empty);

// Indexadores
var primerItem = listaEmpleados?[0]?.Nombre;

// En APIs realistas
public decimal ObtenerDescuento(Cliente cliente)
{
    return cliente?.Membresia?.NivelDescuento ?? 0m;
}






///////////////////////////////////////////////////////////////////////////////
// 3. Expression-Bodied Members
// Antes (C# 5):
// csharp
public class Producto
{
    private decimal _precio;
    private int _cantidad;

    public decimal Total
    {
        get { return _precio * _cantidad; }
    }

    public string ObtenerDescripcion()
    {
        return $"Producto: {_precio:C} x {_cantidad}";
    }

    public override string ToString()
    {
        return $"{_precio:C}";
    }
}


// Después (C# 6):
// csharp
public class Producto
{
    private decimal _precio;
    private int _cantidad;

    // Properties con expresión
    public decimal Total => _precio * _cantidad;
    public bool EsCaro => _precio > 1000;
    public string Categoria => _precio > 500 ? "Premium" : "Estándar";

    // Métodos con expresión
    public string ObtenerDescripcion() => $"Producto: {_precio:C} x {_cantidad}";
    
    public decimal CalcularImpuesto() => Total * 0.16m;

    // ToString, Equals, etc.
    public override string ToString() => $"{_precio:C}";
}

// Caso realista: cálculos rápidos
public class CarritoCompras
{
    private List<Producto> _productos;

    public decimal Subtotal => _productos.Sum(p => p.Total);
    public decimal Impuestos => Subtotal * 0.16m;
    public decimal Total => Subtotal + Impuestos;
    public int CantidadItems => _productos.Count;
    public bool EstaVacio => _productos.Count == 0;
}


// 4. Using Static
// Antes (C# 5):
// csharpusing System;
using System.Math;

public class Calculadora
{
    public double CalcularDistancia(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    public void MostrarResultado(double resultado)
    {
        Console.WriteLine($"Resultado: {resultado}");
        Console.WriteLine($"Redondeado: {Math.Round(resultado, 2)}");
        Console.WriteLine($"Máximo con 100: {Math.Max(resultado, 100)}");
    }
}


// Después (C# 6):
// csharpusing System;
using static System.Math;        // ⭐ Ya no escribes Math.
using static System.Console;     // ⭐ Ya no escribes Console.

public class Calculadora
{
    public double CalcularDistancia(double x1, double y1, double x2, double y2)
    {
        // Mucho más limpio
        return Sqrt(Pow(x2 - x1, 2) + Pow(y2 - y1, 2));
    }

    public void MostrarResultado(double resultado)
    {
        WriteLine($"Resultado: {resultado}");           // Sin Console.
        WriteLine($"Redondeado: {Round(resultado, 2)}"); // Sin Math.
        WriteLine($"Máximo con 100: {Max(resultado, 100)}");
    }

    public double CalcularArea(double radio)
    {
        return PI * Pow(radio, 2);  // PI directamente, sin Math.PI
    }
}

// Caso realista: validaciones
using static System.String;

public class Validador
{
    public bool EsValido(string texto)
    {
        return !IsNullOrWhiteSpace(texto);  // Sin String.IsNullOrWhiteSpace
    }
}



/////////////////////////////////////////////////////////////////////////////

public class Usuario
{
    public string Nombre { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; }
    public List<string> Roles { get; set; }

    public Usuario()
    {
        FechaRegistro = DateTime.Now;
        Activo = true;
        Roles = new List<string>();
    }
}
// Después (C# 6):
// csharp
public class Usuario
{
    // ⭐ Inicialización directa, sin constructor
    public string Nombre { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool Activo { get; set; } = true;
    public List<string> Roles { get; set; } = new List<string>();
    
    // Getter-only properties (inmutables)
    public Guid Id { get; } = Guid.NewGuid();
    public string Version { get; } = "1.0.0";
}

// Caso realista: configuración
public class AppConfig
{
    public int TimeoutSegundos { get; set; } = 30;
    public int MaxReintentos { get; set; } = 3;
    public string Ambiente { get; set; } = "Desarrollo";
    public bool LogHabilitado { get; set; } = true;
    public List<string> ServidoresPermitidos { get; set; } = new List<string> 
    { 
        "localhost", 
        "server1.com" 
    };
}




6. nameof()
// Antes (C# 5):
// csharp
public class Empleado
{
    private string _nombre;

    public string Nombre
    {
        get { return _nombre; }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Nombre no puede estar vacío", "Nombre"); // ❌ String literal
            
            _nombre = value;
            OnPropertyChanged("Nombre"); // ❌ String literal - error prone!
        }
    }

    public void GuardarLog()
    {
        Logger.Info("Método GuardarLog ejecutado"); // ❌ Si renombras, se rompe
    }
}
// Después (C# 6):
// csharp
public class Empleado
{
    private string _nombre;

    public string Nombre
    {
        get { return _nombre; }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(
                    "Nombre no puede estar vacío", 
                    nameof(Nombre)); // ✅ Refactoring-safe!
            
            _nombre = value;
            OnPropertyChanged(nameof(Nombre)); // ✅ Si renombras, se actualiza automáticamente
        }
    }

    public void GuardarLog()
    {
        Logger.Info($"Método {nameof(GuardarLog)} ejecutado"); // ✅ Type-safe
    }

    public void ValidarDatos(string email, int edad)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email));
        
        if (edad < 0)
            throw new ArgumentException($"{nameof(edad)} debe ser mayor a 0");
    }
}

// Caso realista: logs estructurados
public class ServicioUsuarios
{
    private readonly ILogger _logger;

    public async Task<Usuario> ObtenerUsuario(int id)
    {
        _logger.LogInformation(
            $"Ejecutando {nameof(ObtenerUsuario)} con ID: {id}");
        
        try
        {
            var usuario = await _repositorio.GetByIdAsync(id);
            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                $"Error en {nameof(ObtenerUsuario)}, parámetro: {nameof(id)} = {id}");
            throw;
        }
    }
}