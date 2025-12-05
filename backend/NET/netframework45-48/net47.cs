🔵 .NET Framework 4.7 / 4.7.1 / 4.7.2 — Resumen de mejoras clave
1️⃣ High DPI mejorado (High DPI Awareness v2)

En .NET Framework 4.7+ se hicieron actualizaciones profundas para mejorar la experiencia en pantallas de alta resolución (4K, 5K, escalado de Windows a 150%, 200%, etc.).

🔍 Problema antes de .NET 4.7

Windows Forms y WPF tenían artefactos de escalado.

Controles borrosos.

Distorsión en fuentes.

Layout incorrecto al mover la ventana entre monitores con diferentes escalados DPI.

⭐ ¿Qué introduce .NET 4.7+?

✔ Soporte para High DPI v2 en WinForms.
✔ Mejor escalado de fuentes y controles automáticamente.
✔ Mejor soporte al cambiar de monitor con distinto DPI (per-monitor DPI awareness).
✔ Mejor integración con la API moderna de Windows 10 para DPI.

🔧 Activación (WinForms)

En app.config:

<configuration>
  <windowsSettings>
    <dpiAwareness>PerMonitorV2</dpiAwareness>
  </windowsSettings>
</configuration>


O vía manifiesto:

<dpiAware>true/pm</dpiAware>


Esto habilita que el formulario se escale correctamente dependiendo del monitor actual.

2️⃣ Compatibilidad con .NET Standard 2.0

Esta es probablemente la feature más importante de .NET Framework 4.7.1.

🔍 ¿Qué es .NET Standard?

Es un conjunto de APIs comunes entre:

.NET Framework

.NET Core

Xamarin

Mono

Unity

.NET Standard 2.0 fue la versión que unificó 32k APIs, haciendo que librerías modernas pudieran funcionar en .NET Framework nuevamente.

⭐ ¿Qué habilitó .NET Framework 4.7.1?

✔ Puede referenciar librerías compiladas para .NET Standard 2.0.
✔ Permite compartir código entre:

.NET Framework (apps legacy)

.NET Core / .NET 5+

Xamarin apps

Microservicios en .NET moderna

✔ Permite migrar aplicaciones grandes paso a paso sin romper compatibilidad.

🧩 Ejemplo práctico:

Una librería común:

// MyLibrary.csproj
<TargetFramework>netstandard2.0</TargetFramework>


Puede ser usada en:

.NET Framework 4.7.1+

.NET Core 2.0+

.NET 6, 7, 8, 9...

Esto fue clave para reducir la fragmentación del ecosistema.

3️⃣ GC mejorado (latencia, LOH, rendimiento)

En .NET Framework 4.7+ se integraron varias mejoras de GC que luego serían bases para el GC moderno de .NET Core.

⭐ Mejoras introducidas:
✔ Mejoras en el Server GC

Mejor balance entre heaps por core.

Mejor gestión de threads del GC.

Reducción de pausas largas en colecciones Gen2.

✔ Mejoras en el Background GC

Background GC más eficiente en escenarios de alta memoria.

Mejor recolección concurrente.

✔ Fragmentación reducida (LOH improvements)

Aunque el LOH compacting aparece fuerte en .NET Framework 4.8, en 4.7 ya se introducen mejoras para:

Menor fragmentación.

Mejor manejo de objetos > 85 KB.

✔ Mejoras internas en el algoritmo de "dynamic adjustment"

El GC adapta su agresividad según patrones reales de memoria.

Resultado práctico:

Menos pausas en UI.

Mejor rendimiento en aplicaciones de servidor ASP.NET.

Menor uso de memoria en aplicaciones que cargan muchos objetos grandes.

🔵 Resumen Ejecutivo (para entrevistas / documentación)
Versión	Área	Mejora	Impacto
4.7	WinForms/WPF	High DPI v2	Apps más nítidas, escalado correcto en 4K y monitores múltiples.
4.7.1	BCL	Compatibilidad .NET Standard 2.0	Sharing de librerías modernas, migración más simple a .NET Core.
4.7.2	GC	Mejoras en server GC y background GC	Menos pausas, menor latencia, mejor rendimiento en servidores.
🔵 ¿Quieres más profundidad?

Puedo generar:

✔ Ejemplos de código específicos de .NET Standard 2.0 usados desde .NET Framework.
✔ Cómo escribir una librería que funcione en .NET Framework 4.7.2 y .NET 8.
✔ Diagramas de High DPI Awareness.
✔ Explicación visual del GC mejorado en 4.7+.