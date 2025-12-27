


var names = new List<string> { "Alice", "", "Charlie", "" };
string getRandomName()
{
  var random = new Random();
  int index = random.Next(names.Count);
  return names[index];
}
string someRandomName = getRandomName() ?? "Default Name";

string lastName = names[^1];

string clasificarEdad(int Edad) =>  //cuidar muy bien que no se traslapen las ocdicionales 
  Edad switch
  {
    < 0 => "Edad no valida",
    >= 0 and < 18 => "Menor de edad",
    >= 18 and < 65 => "Mayor de edad",
    >= 65 and < 150 => "Tercera edad",
    _ => "Edad no válida"
  };

Console.WriteLine(clasificarEdad(70));

var numeros = new[] { 10, 20, 30, 40, 50, 60 };
var despuesDelsegundo = numeros[2..];
var ultimosdos = numeros[^2..];
Console.WriteLine($"ultimos dos numeros: {string.Join(", ", ultimosdos)}");

async IAsyncEnumerable<int> GenerarNumerosDeFormaAsyncDelay() //tenemos un metodo asyncrono que devuelve una secuencia de enteros
{
  for (int i = 0; i <= 5; i++)
  {
    await Task.Delay(1000);
    yield return i; //yield lo dispone el dato tan pronto como este disponible
  }
}

await foreach (var numero in GenerarNumerosDeFormaAsyncDelay())  //iterar sobre datos asyncronos tan pront esten disponibles.
{
  Console.WriteLine(numero);
}




// Parte 5: Pregunta abierta(5 puntos)
// 16.Explica con un ejemplo práctico cuándo usarías IAsyncEnumerable<T> en lugar de Task<List<T>> y cuáles son las ventajas.
// Respuesta modelo:
// Usaría IAsyncEnumerable<T> cuando necesito procesar resultados conforme van llegando, sin esperar a que todos estén disponibles.
// Ejemplo: Consulta paginada a base de datos
// csharp// ❌ Mal - Espera a cargar TODOS los registros en memoria
 async Task<List<Usuario>> ObtenerUsuariosAsync()
{
  var usuarios = new List<Usuario>();
  // Carga 100,000 usuarios en memoria
  await foreach (var usuario in _db.Usuarios)
  {
    usuarios.Add(usuario);
  }
  return usuarios; // Consume mucha memoria
}

// // ✓ Bien - Procesa de a poco
 async IAsyncEnumerable<Usuario> ObtenerUsuariosStreamAsync()
{
  await foreach (var usuario in _db.Usuarios)
  {
    yield return usuario; // Lo devuelve inmediatamente
  }
}

// // Uso
await foreach (var usuario in ObtenerUsuariosStreamAsync())
{
  // Proceso cada usuario conforme llega
  await EnviarEmailAsync(usuario);
}
// Ventajas:

// Menor consumo de memoria(no carga todo a la vez)
// Mejor tiempo de respuesta (empieza a procesar inmediatamente)
// Ideal para grandes volúmenes de datos
// Permite cancelación más granular