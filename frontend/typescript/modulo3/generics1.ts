/*
Los genericos existen por que hay 3 problemas 
1.Duplicidad de funcione spor tipo haciendo lo mismo 
2.Al usar any como tipo se pierde seguridad por que 
3.usar unknown es seguro pero incomodo, luego se tiene que andar casteando

Aqui entran los genericos: mismo algoritmo para cualquoier tipo conservando el tipo ( tanto entrada como salida)

JavaScript es:
🔹 Dinámico
🔹 Débilmente tipado
🔹 Tipado en runtime
Eso significa que los tipos:
Se determinan en ejecución :(
Pueden cambiar :()


JavaScript usa IEEE-754 double precision.
⚠ Problema clásico:
0.1 + 0.2 // 0.30000000000000004

luego los strings son inmutables.

undefined es variable declarada pero no asignada
null es ausencia intencional de valor(asignaod a nada) 

⚠ Curiosidad histórica:
typeof null // "object"



6️⃣ Symbol (ES6)
Identificadores únicos.
const id = Symbol("id");
Útil para:
Claves privadas
Metaprogramación

7️⃣ BigInt (ES2020)
Para números gigantes fuera del límite seguro.
const big = 9007199264740991n;
Number.MAX_SAFE_INTEGER

Object
Todo lo que no es primitivo es objeto.

Incluye:
Arrays
Functions
Dates
Maps
Sets
RegExp
Clases
etc.


🚀 Conexión con TypeScript
TypeScript:

No cambia los tipos de JS
Solo agrega sistema de tipos en compile-time
Los tipos desaparecen en runtime



Preguntas interresantes: 
1.Cómo V8 representa internamente los tipos
2.Cómo funciona el sistema de coerción paso a paso
3.Cómo funcionan los Hidden Classes
4.cómo TypeScript modela estos tipos con unions e intersections
*/



//Ejemplo esto escal ahorrible: 
function getFirstString(arr:string[]) : string {
    return arr[0];
}

function getFirstNumber(arr:number[]) :number {
  return arr[0];
}


//en genericos:
// Los genéricos NO existen en runtime.
// Solo existen en el compilador.

function getFirst<T>(array:T[]):T | undefined { //T, Z indican tipos (tipo parametro ) Number, String etc //Entonces es polimorfimso parametrico.
  return array[0];
}

// con genericos pares
function getPair<T,Z>(t:T, z:Z): [T,Z] | undefined {
  return [t,z];
}
const p = getPair(120, "120");
function map<T, R>(arr: T[], fn: (x: T) => R): R[] {
  return arr.map(fn);
}










function getProperty<T,K extends keyof T>(object:T, property:K): T[K] { //debes indicar que la proipiedad no slo  puede ser un string sino limitarlo a aque SOLAMNENTE puede se runa prpoiedad del tipo T y el tipo de retorno justo indica eso tambien
  return object[property]
}



// 🧠 Recordatorio rápido

// Si tienes:

// type User = {
//   id: number;
//   name: string;
//   active: boolean;
// };

// Entonces:

// keyof User
// // "id" | "name" | "active"
