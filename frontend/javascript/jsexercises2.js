// Global Execution Context

// Function Execution Context

// Creation phase

// Execution phase

// Variable Environment

// Outer Environment Reference

// This binding



//I

// 🧠 Cómo funciona realmente(internals)

// JavaScript ejecuta en dos fases:

// Creation Phase(Memory Allocation)

// Execution Phase

// En la fase de creación:

// Se registran variables

// Se registran funciones

// Se asignan referencias

// Se inicializan dependiendo del tipo de declaración

// Eso es lo que se conoce como hoisting.




//II
// 🧠 Lo que realmente ocurre(nivel engine)

// Durante la creación del execution context:

// Se crea un Variable Environment

// Se crea un Lexical Environment

// Se registran bindings

// Se asignan referencias

// Se preparan closures


console.log(a); //undefined (la variable no ha sido definida)
var a = 10;

function test() {
  console.log("inside test");
}

console.log(test); //(Funtion:test) indica que se trata de una funcion



//2
var a = 1;

function foo() {
  console.log(a);//undefined
}

var a = 2;
foo();//2 utilia el valor de la varibae como golobal , pro tanto lo que vale afuera a tamben se toam dentro de la funcion.




var a = 1;

function b() {
  var a = 2;
  c();//2
}

function c() {
  console.log(a);//1
}

b();//1 El contexto activo es c , el cual toma a la variable a.





//Bloque 3 Creation Phase.