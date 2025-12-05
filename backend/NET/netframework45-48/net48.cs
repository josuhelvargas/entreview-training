🔵 .NET Framework 4.8 / 4.8.1 — Mejoras principales
1️⃣ Mejoras en RyuJIT (JIT Compiler)

.NET Framework 4.8 incorpora una versión más madura y moderna de RyuJIT, beneficiándose directamente de avances introducidos en .NET Core.

⭐ ¿Qué cambia realmente?

✔ Mejor rendimiento en el código generado
El JIT produce código nativo más eficiente, especialmente para:

Bucles intensivos

SIMD (System.Numerics)

Operaciones matemáticas

Código con branching complejo

✔ Optimizaciones heredadas de .NET Core 2.x/3.x
Aunque .NET Framework permanece congelado en cuanto a evolución, RyuJIT sigue recibiendo refinamientos:

Mejor enlazado de métodos (inlining)

Mejor propagación de constantes

Eliminación de código muerto

Reducciones en asignaciones temporales (mejor IL-to-native translation)

✔ Menos “tiered JIT stalls”
Aunque no tiene Tiered Compilation completa como .NET Core, sí reduce pausas iniciales para métodos calientes.

2️⃣ LOH Compacting (Large Object Heap Compacting) mejorado

El LOH (Large Object Heap) es una parte del GC donde se guardan objetos mayores de ~85 KB, como:

Grandes arreglos (byte[], float[], etc.)

Buffers

Imágenes

Serializaciones grandes

Históricamente, el LOH no se compactaba, lo que causaba:

Fragmentación severa

Crecimiento sostenido del uso de memoria

Problemas en aplicaciones de larga ejecución (ej. servidores)

⭐ .NET Framework 4.8 introduce LOH compacting controlado

✔ Compactación selectiva del LOH, bajo ciertas condiciones.
✔ Reduce fragmentación en aplicativos que asignan y liberan grandes objetos.
✔ Manejo más eficiente de montos de memoria sostenidos.

🔧 Configuración

Puedes habilitar compactación manual:

GCSettings.LargeObjectHeapCompactionMode =
    GCLargeObjectHeapCompactionMode.CompactOnce;

GC.Collect();


Esto compacta LOH una vez durante el próximo Gen2 GC.

🧠 Beneficio real

Menos OutOfMemoryExceptions en aplicaciones con alto churn de buffers.

Mejor rendimiento en servidores y procesos que corren por muchas horas/días.

Menor fragmentación = uso más estable de memoria.

3️⃣ Accesibilidad (A11y) mejorada

.NET 4.8 y 4.8.1 incluyen una oleada de mejoras para accesibilidad en:

Windows Forms

WPF

Controls legacy

Narradores (Narrator/UIA)

Contraste alto

⭐ Cambios destacados

✔ Mejor soporte para UI Automation
✔ Roles y patrones controlables por herramientas de accesibilidad
✔ Mejoras para:

DataGridView

ComboBox

Button

ListView

CheckBox

RadioButton

MonthCalendar

✔ Lectura más precisa en Narrator
✔ Mejor diferenciación en High Contrast Themes
✔ Focus visuals más claros para usuarios con discapacidad visual
✔ Fixes en Keyboard Navigation (Tab/Shift+Tab)

En general, .NET 4.8 es la versión más alineada con los requisitos de accesibilidad de Windows 10/11.

4️⃣ Soporte para ARM64 (en 4.8.1)

.NET Framework 4.8.1 es la primera versión que ofrece soporte más completo para Windows en ARM64, incluyendo:

⭐ Soporte nativo para ARM64 en:

WPF

WinForms

WCF

BCL

JIT (RyuJIT adaptado a ARM64)

¿Por qué es importante?

Windows 11 está entrando fuerte en ARM64 (Surface Pro X, laptops nuevas).

Permite ejecutar aplicaciones .NET Framework en estos dispositivos sin emulación x86.

Mejora eficiencia energética y rendimiento en arquitecturas ARM.

🔵 Resumen Ejecutivo (para entrevistas o documentación)
Feature	4.8	4.8.1	Impacto
RyuJIT mejorado	✔	✔	Código nativo más eficiente, menor latencia, mejores optimizaciones (SIMD, branching, inlining).
LOH Compacting	✔(Mucho mejor)	✔	Reduce fragmentación de memoria, ideal para apps con objetos grandes.
Accesibilidad mejorada	✔	✔	Mejor Navegación, UI Automation, lectura por Narrator, High Contrast.
Soporte ARM64	❌	✔	Las apps .NET Framework pueden correr nativas en Windows ARM64.