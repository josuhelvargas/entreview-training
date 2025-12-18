// Como escribir purebas unitarias: 
//1 Renderiza el componente

//2 Obtén un elemento del componente y simula cualquier interacción del usuario.

//3 Escribe una afirmación.


// Qué probar
// En general, sus pruebas deben cubrir los siguientes aspectos de su código:

// Si un componente se renderiza con o sin accesorios

// Cómo se representa un componente con cambios de estado

// Cómo reacciona un componente a las interacciones del usuario


// Qué no probar
// Probar la mayor parte de tu código es importante, pero aquí hay algunas cosas que no necesitas probar:

// Implementación real: No es necesario probar la implementación real de una funcionalidad.Simplemente verifique si el componente funciona correctamente.
// Supongamos que desea ordenar una matriz al hacer clic en un botón.No es necesario probar la lógica de ordenación.Solo verifique si se llamó a la función y si los cambios de estado se representan correctamente.

// Bibliotecas de terceros: si utiliza bibliotecas de terceros como Material UI, no es necesario probarlas; ya deberían estar probadas y comprobadas.



// Mocking 
// Implica crear una replica de una dependnecias que imita el comportamiento real de la dependencia original.
// Utilice Mocking al probar interacciones(para verificar si una función llama a otra con los argumentos esperados).



// Stubbing
// El stubbing remplaza cierta funcionalidad de una dependencia original con respuestas predeterminadas.

// Utilice Stubbing al probar la lógica: proporcione entradas controladas para garantizar resultados consistentes sin depender de dependencias externas.



//ejem1

import { test, expect,vi } from 'vitest';
import { render, screen, fireEvent } from "@testing-library/react";
import { Button } from '../components/Button';

export const sumar =  (a:number, b:number ) => a+b;

test('sumar 1 + 2 = 3', () => {
  const resultado = sumar(1,2);
  expect(resultado).toBe(3);
});


test("calls onClick when button is clicked", () => {
  const handleClick = vi.fn();
  render(<Button handleClick={handleClick} label="Click Me" />);

  const button = screen.getByText(/click me/i);
  fireEvent.click(button);

  expect(handleClick).toHaveBeenCalledTimes(1);
});