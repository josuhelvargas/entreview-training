// export function processMessage(message: string, logger: (msg: string) => void) {
//   logger(message);
// }
import { processMessage } from "../utils/utils";
import { vi, describe, expect, it } from "vitest";


describe('process message', ()=>{
    it('register or make log function work ' , ()=>{
     const fnProcessLog = vi.fn();
     processMessage('hola', fnProcessLog);
     expect(fnProcessLog).toBeCalledWith('hola');
    });
});

// Ejercicio 2 — función con retorno  (buscar asi en chatgpt)