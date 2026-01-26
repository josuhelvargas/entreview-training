using System;

namespace FuncWebhookDemo.Data;

public class WebhookEvent
{
  public int Id { get; set; }

  // Idempotencia: usaremos el delivery id de GitHub como unique key lógica
  public string DeliveryId { get; set; } = default!;

  // La propiedad DeliveryId empieza como null, pero el ! evita que el compilador te muestre advertencias de que podría ser nula.Es una forma de decir: “sé que esto se inicializa como null, pero confío en que luego se le asignará un valor válido antes de usarse.”
  public string EventType { get; set; } = default!;
  public string PayloadJson { get; set; } = default!; //En C#, default para un string es null.

  //El operador ! (null-forgiving operator) le dice al compilador que ignore las advertencias de nulabilidad (es decir, que confíe en que no será null en tiempo 
  //de ejecución, aunque se inicialice como tal).
  public DateTime ReceivedUtc { get; set; } = DateTime.UtcNow;
}
