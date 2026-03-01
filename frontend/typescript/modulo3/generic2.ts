

function setProperty<T,K extends keyof T>(object:T, key:K, value:T[K]) : void //key es una propiedad valida de T
{
   object[key] = value;
}


interface User {
  id:number, 
  name:string, 
  active:boolean
}

const  user:User = {
  id: 1, 
  name: "Josue",
  active: true
}

// setProperty(user, "name", "Daniela");
// console.log(user);


function pick<T,K extends keyof T> (object:T, keys:K[]) : Pick<T,K>
{
  const result = {} as Pick<T,K>;
  for(const key of keys){
    result[key] = object[key];
  }
  return result; 
}

type AllowedKeys =  "id" | "active"  | "name" ;
const userKeys:AllowedKeys[] = [ "active", "name"] as const; //Es importante indicar los valores que s epueden aceptar par aque el compialdor de ts lo corra
const extracted = pick(user, userKeys);



// 1️⃣ El problema real

// Cuando escribes:

// const userKeys = ["id", "active", "name"];

// TypeScript NO guarda esos valores como literales.

// Internamente lo interpreta como:

// const userKeys: string[]

// No como:

// ("id" | "active" | "name")[]

// Y ahí está el problema.

// Tu función exige:

// K extends keyof T

// Pero string no es lo mismo que "id" | "active" | "name".

// 2️⃣ ¿Qué hace as const realmente?

// Cuando escribes:

// const userKeys = ["id", "active", "name"] as const;

// TypeScript cambia el tipo de esto:

// string[]

// a esto:

// readonly ["id", "active", "name"]

// Es decir:

// Ya no es un arreglo genérico.

// Es un tuple literal exacto.

// Cada posición mantiene su valor literal.

// Eso permite que TypeScript infiera:

// K = "id" | "active" | "name"

Y ahora sí coincide con keyof T.