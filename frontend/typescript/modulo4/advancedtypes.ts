//Una interseccion es un valor que cimple A y B al mismo tiempo.
type A = { id: number}
type B = { name: string}

type C = A & B;



//diferencia con union 
//puede ser A, O bien , puede ser B
type U = A | B ;




//Caso peligroso (intersección incompatible)
type AW = { id: string };
type BW = { id: number };

type CW = AW & BW;

//Aquí:

// type CW = {
//   id: string & number
// }

// ¿Y qué es string & number?

// 👉 never

// Porque no puede existir algo que sea string Y number.

// Entonces C["id"] es never.



