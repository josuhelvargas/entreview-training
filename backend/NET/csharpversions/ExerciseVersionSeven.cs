
//pattern matching: 

using System.Runtime.CompilerServices;

executeValidationAccordingToType(42);

executePatternMatchingOnArray(new object[] { 1, 2, "" });

int.TryParse("342", out var numeroParseado); //se pueden crear variables out inline en lugar de tenerlas que declarar arriba. (parseos)
Console.WriteLine($"Número parseado: {numeroParseado}");



var usuarios = new Dictionary<int, string>   //out vars enseleccion de diccionarios.
{
    { 1, "Juan" },
    { 2, "María" }
};

if (usuarios.TryGetValue(1, out var nombre))  //trygetvalue
{
  Console.WriteLine($"Usuario encontrado: {nombre}");
}

bool ValidarYExtraerEmail(string texto, out string email, out string dominio)
{
  // email y dominio NO tienen valor inicial (debes asignarles uno)
  // DEBES asignarles un valor antes de salir del método
  // Estos valores SALEN del método hacia quien lo llamó

  email = string.Empty;  // OBLIGATORIO asignar
  dominio = string.Empty; // OBLIGATORIO asignar

  if (string.IsNullOrEmpty(texto) || !texto.Contains("@"))
    return false;

  var partes = texto.Split('@');
  email = texto;      // Modificas el valor que SALE
  dominio = partes[1]; // Modificas el valor que SALE
  return true;
}

// Uso:
if (ValidarYExtraerEmail("usuario@ejemplo.com", out var email, out var dominio)) //se declaran como parametros pero salen del metodo en su scope por tanto sirven para devolver multiples valores. (que de hecho deberia poderse mejro con  tuplas)
{
  Console.WriteLine($"Email: {email}");
  Console.WriteLine($"Dominio: {dominio}");
}


//is funciona para tipo y para valores puedes usar en el pattern matchin is, and , or  (literalmente  no || && lo que lo hace mas legible) atambien puede usars is not null / is null 
void executeValidationAccordingToType(object input)
{
  if (input is int number) { Console.WriteLine($"Input is an integer: {number}"); }
  else if (input is string text) { Console.WriteLine($"Input is a string: {text}"); }
  else if (input is null) { Console.WriteLine("Input is null"); }
  else { Console.WriteLine("Input is of an unknown type"); }
}

//tambien podemos hacer pattern matchin para validar arrays : 

void executePatternMatchingOnArray(object[] myArray)
{
  switch (myArray) //se tiene que seguir una estructura asi por qu eno regresa nada.
  {
    case [1, 2, 3]:
      Console.WriteLine("Array contains exactly 1, 2, 3");
      break;

    case [var first, var second, var third] when first is int && second is int && third is int:
      Console.WriteLine($"Array contains three integers: {first}, {second}, {third}");
      break;

    case [var first, .. var rest]:
      Console.WriteLine($"Array starts with {first} and has {rest.Length} more elements");
      break;

    default:
      Console.WriteLine("Array does not match any pattern");
      break;
  }
}

string ValidarDatos(object dato)
{
  return dato switch
  {
    null => "El dato es nulo",
    string s when s.Length == 0 => "String vacío",
    string s when s.Length > 100 => "String demasiado largo",
    string s => $"String válido: {s}",
    int n when n < 0 => "Número negativo",
    int n when n is >= 0 and <= 100 => "Número en rango válido",
    int n => $"Número fuera de rango: {n}",
    _ => "Tipo no soportado"
  };
}




//funciones dentro de otras funciones 
//1.Cuando necesitas una función auxiliar que SOLO tiene sentido dentro de otra función:
//2.Para evitar repetir código dentro de un mismo método:
//3.Para mejorar la legibilidad del código al encapsular lógica específica.(validaciones compartidas)
//4.Para poder utilizar funciones recursivas sin exponerlas al ámbito global.
//!!!No usarlas si el metodo padre s evuelv edemasiado largo o complejo









/////Dado que out var ( al final son valriables que se marcan como parametro sepero en realidad son variables de retorno  vs tuplas que son multiples valores de retorno)  pueden ser usadas para devolver multiples valores  se pueden usar para evitar crear clases o estructuras para devolver multiples valores.Cada opcion apra deveolver mutiples valores deve ser cosiderada con cuidado dependiendo del contexto y la complejidad de los datos a devolver: ejem: 
/// 
/// Usa out cuando:
//1.Estás siguiendo un patrón establecido de .NET (TryParse, TryGet, etc.):
//csharp// ✓ BIEN - Sigue la convención de .NET
bool TryObtenerUsuario(int id, out Usuario usuario)
{
  usuario = _repositorio.BuscarPorId(id);
  return usuario != null;
}

// Uso familiar para cualquier programador C#:
if (TryObtenerUsuario(1, out var usuario))
{
  Console.WriteLine(usuario.Nombre);
}
//2.El método devuelve un bool de éxito/fallo Y un valor:
//csharp// ✓ Patrón Try- muy común y reconocible
bool TryParseCoordinate(string input, out double latitude, out double longitude)
{
  latitude = longitude = 0;

  var parts = input.Split(',');
  if (parts.Length != 2) return false;

  return double.TryParse(parts[0], out latitude) &&
         double.TryParse(parts[1], out longitude);
}

// Uso:
if (TryParseCoordinate("19.4326, -99.1332", out var lat, out var lon))
{
  Console.WriteLine($"Lat: {lat}, Lon: {lon}");
}



//3.Necesitas modificar una biblioteca existente o mantener compatibilidad:
//csharp// Si ya existe código usando out, mantén la consistencia
bool TryConnect(string server, out Connection connection)
{
  // ...
}



//4.Trabajas con código legacy o frameworks antiguos:
//csharp// Muchas APIs antiguas usan out
Dictionary.TryGetValue(key, out var value);
int.TryParse(str, out var number);
//Usa Tuples cuando:
//1.Devuelves datos relacionados sin sentido de "éxito/fallo":
// ✓ MEJOR con tuple - Solo devuelves datos
(string nombre, string apellido, int edad) ObtenerDatosPersona(int id)
{
  var persona = _repositorio.Buscar(id);
  return (persona.Nombre, persona.Apellido, persona.Edad);
}

// Uso limpio y claro:
var (nombre, apellido, edad) = ObtenerDatosPersona(1);
Console.WriteLine($"{nombre} {apellido}, {edad} años");



//2.Devuelves resultados de cálculos o transformaciones:
//csharp// ✓ MEJOR con tuple - Es una transformación, no una validación
(decimal subtotal, decimal impuestos, decimal total) CalcularPrecioFinal(decimal precio)
{
  var subtotal = precio;
  var impuestos = precio * 0.16m;
  var total = subtotal + impuestos;

  return (subtotal, impuestos, total);
}

// Uso:
var (subtotal, impuestos, total) = CalcularPrecioFinal(100);
Console.WriteLine($"Total: ${total}");







//3.Método SIEMPRE devuelve valores válidos:
//csharp// ✓ MEJOR con tuple - No hay caso de fallo
 (int minimo, int maximo, double promedio) AnalizarNumeros(int[] numeros)
{
  return (numeros.Min(), numeros.Max(), numeros.Average());
}

var (min, max, prom) = AnalizarNumeros(new[] { 1, 5, 10, 15 });






//4.Trabajas con código moderno y quieres claridad:
//csharp// ✓ Tuple es más declarativo y moderno
 (bool encontrado, Usuario usuario, string mensaje) BuscarUsuario(string email)
{
  var usuario = _repositorio.BuscarPorEmail(email);

  if (usuario == null)
    return (false, null, "Usuario no encontrado");

  if (!usuario.Activo)
    return (false, usuario, "Usuario inactivo");

  return (true, usuario, "Usuario encontrado");
}

// Uso super claro:
var (encontrado, usuario, mensaje) = BuscarUsuario("test@mail.com");
if (encontrado)
{
  Console.WriteLine($"Bienvenido {usuario.Nombre}");
}
else
{
  Console.WriteLine(mensaje);
}





//5.Necesitas nombres descriptivos en los valores de retorno:
//csharp// ✓ Los nombres hacen el código autodocumentado
(int totalVentas, decimal montoTotal, DateTime ultimaVenta) ObtenerEstadisticas(int clienteId)
{
  // ...
  return (ventas.Count, ventas.Sum(v => v.Monto), ventas.Max(v => v.Fecha));
}

// Los nombres aparecen en intellisense y son super claros:
var stats = ObtenerEstadisticas(123);
Console.WriteLine($"Ventas: {stats.totalVentas}");
Console.WriteLine($"Monto: ${stats.montoTotal}");
// Comparación directa:
// ❌ out - Sintaxis más verbosa para casos que no son Try-
bool ObtenerDimensiones(Rectangulo rect, out int ancho, out int alto)
{
  ancho = rect.Ancho;
  alto = rect.Alto;
  return true; // ¿Por qué un bool si siempre es true?
}

if (ObtenerDimensiones(miRect, out var ancho, out var alto))
{
  Console.WriteLine($"{ancho} x {alto}");
}


// ✓ Tuple - Más directo y claro
(int ancho, int alto) ObtenerDimensiones(Rectangulo rect)
{
  return (rect.Ancho, rect.Alto);
}

var (ancho, alto) = ObtenerDimensiones(miRect);
Console.WriteLine($"{ancho} x {alto}");
//Casos mixtos(pueden funcionar ambos):
// Con out (patrón Try-)
bool TryDividir(int a, int b, out decimal resultado)
{
  resultado = 0;
  if (b == 0) return false;

  resultado = (decimal)a / b;
  return true;
}

// Con tuple (más moderno)
(bool exito, decimal resultado) Dividir(int a, int b)
{
  if (b == 0) return (false, 0);
  return (true, (decimal)a / b);
}


// ## Regla de oro:
// ```
// ┌─────────────────────────────────────────────────┐
// │ ¿El método sigue el patrón Try-                │
// │ (bool éxito + valores)?                         │
// │                                                 │
// │ SÍ → Usa OUT                                   │
// │ NO → Usa TUPLE                                 │
// └─────────────────────────────────────────────────┘

// ┌─────────────────────────────────────────────────┐
// │ ¿Devuelves solo datos sin concepto             │
// │ de éxito/fallo?                                │
// │                                                 │
// │ SÍ → Usa TUPLE                                 │
// └─────────────────────────────────────────────────┘
// Recomendación moderna:
// Para código nuevo, prefiere Tuples a menos que específicamente necesites el patrón Try- para validaciones. Los tuples son:

// Más legibles
// Más modernos
// Más declarativos
// Menos verbosos
// Soportan nombres descriptivos

// Código moderno y limpio
(bool esValido, string email, string dominio, string error) ValidarEmail(string texto)
{
  if (string.IsNullOrEmpty(texto))
    return (false, "", "", "El texto está vacío");

  if (!texto.Contains("@"))
    return (false, "", "", "No contiene @");

  var partes = texto.Split('@');
  return (true, texto, partes[1], "");
}

// Uso super claro:
var validacion = ValidarEmail("test@mail.com");
if (validacion.esValido)
{
  Console.WriteLine($"Email: {validacion.email}");
  Console.WriteLine($"Dominio: {validacion.dominio}");
}
else
{
  Console.WriteLine($"Error: {validacion.error}");
}






//Utilizando cifras: 

int binary1 = 0b0001; //0b inicio
int binary2 = 0b0010;


int large = 1_000_000; //separador de cifras con _ para mejorar legibilidad.