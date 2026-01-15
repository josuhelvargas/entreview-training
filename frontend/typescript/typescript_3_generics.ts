//  const getFirst<T> (array:T[]) :T | undefined => array[0]; 

 function getFirstf<T> (array:T[]):T | undefined { return array[0]};
 function getId<T extends{ id:string}> (obj:T):string { return obj.id} //limitamos para indicar que el objeto tiene al pripiedad string

 type ElementType<T> = T extends (infer U) [] ? U : T;

 type ApiErrorShape<T> = T extends "validation"
  ? { field: string; message: string }
  : { message: string; code?: string };



  function proccessArray<T> (array: T[], processor: (item:T) => T) : void  {
     return array.forEach(element => processor(element));
  }





type Administrator = {
   isAdministrator:boolean
}

class User<T extends Administrator>{
   
   private items = new Map<string, T>();

   is
}
