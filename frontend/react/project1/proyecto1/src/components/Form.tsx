
import { InputText } from './inputText'
import { InputNumber } from './InputNumber'
import { useState, type ChangeEvent } from 'react'
import isNumber from '../utils/utils';
//setState
//https://react.dev/reference/react/useState#:~:text=las%20actualizaciones%20de%20estado%20por%20lotes.
//https://react.dev/reference/react/useState#my-initializer-or-updater-function-runs-twice
export const Form = () => {
  const [formTextValue, setFormTextValue] = useState<string>("");
  const [formNumberValue, setFormNumberValue] = useState<number>(0);

  //El orden en el que se ejcutan es el siguiente:(acutualizacion de estado por lotes (batch))
  //se prieoriza la ejecucion de eventhanlders, luego s eejcutan las fx tipo Setxxx() 
  //eventos (clicks)

//La documentacion indica que esto no es buena idea, no debe haver setstate anidados en  bucles/ condicionales anidados.
const onChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
      isNumber(e.target.value)  
      ? setFormNumberValue(Number(e.target.value))  
      : setFormTextValue(e.target.value);
}

  return (
    <form>
            <InputText onChangeHandler={onChangeHandler} value={formTextValue} placeholder={} label={} />
            <InputNumber onChangeHandler={onChangeHandler} value={formNumberValue} placeholder={} label={} />
    </form>
  )
}


//ejem: 
// ✅ Replace state with a new object
// setForm({
//   ...form,
//   firstName: 'Taylor'
// });


