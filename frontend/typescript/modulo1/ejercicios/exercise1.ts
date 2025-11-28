// Declara una variable status: "success" | "error" | "loading" y asígnale cada valor en diferentes líneas.

let statuss : "success" | "error" | "loading" ;
statuss = "success";
statuss = "error";
statuss = "loading";

// Escribe una función:

// function printLength(value: string | string[]): void {
//  if(typeof(value) == 'string'){
//   console.log(`string is + ${value.length}`);
//  }
//   if(Array.isArray(value)) {
//   console.log(`array length is ${value.length()}`);
//  }
//  }
function printLength(value: string | string[]): void {
  if(typeof(value) == 'string'){
   console.log(`string is + ${value.length}`);
  }
   if(Array.isArray(value)) {
   console.log(`array length is ${value.length}`);
  }
}

// Si value es string, imprime value.length.

// Si es array de string, imprime value.length(cantidad de elementos).

// Usa typeof y / o Array.isArray.

// Define tipos:

//type Square = { kind: "square"; size: number };
//type Circle = { kind: "circle"; radius: number };

interface Square {
  kind: "square";
  size: number;
}
interface Circle {
  kind: "circle";
  radius: number;
}


// Y una función:

// function getArea(shape: Square | Circle): number {
//   // usa un narrowing por shape.kind
// }
function getArea(shape: Square | Circle ): number | undefined {
  if(shape.kind == 'square') {
    return shape.size * shape.size;
  }
  if(shape.kind == 'circle') {
    return Math.PI * shape.radius * shape.radius;
  }
}



//para ver desde la consola con node y correr el archivo typescript: 
// npm install--save - dev typescript //instalacion local 
//instlaacion global ( no es necesario )
//npm install -g typescript
// tsc - v
// tsc hola.ts //compilar
//node hola.js //ejecutar