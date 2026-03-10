import { add } from '../utils/math';
import { vi,describe, it, expect } from 'vitest';

// npx vitest math <- command
interface Result {
    message: string, 
    code: number
}

it('adds 2 numbers properly',()=>{
    expect(add(20,30)).toBe(50);
})

 


describe('using basic mocks to create a vi.functions', ()=> {
    it('call a function with a specific parameter',()=>{
        const mockedFunctionA =  vi.fn();
        mockedFunctionA('a');
        expect(mockedFunctionA).toBeCalledWith('a');
    });


    it('a function returns specific value', ()=>{
        const objectExample = {message: "success", code: 200};
        const mockedFunctionB = vi.fn(() => objectExample);
        mockedFunctionB.mockReturnValue(objectExample);
        //expect(mockedFunctionB()).toBe(objectExample);
    });

    
});


