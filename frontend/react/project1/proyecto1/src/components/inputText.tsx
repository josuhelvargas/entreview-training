
import type { ChangeEventHandler } from 'react';

export interface InputNumberProps {
  onChangeHandler: ChangeEventHandler<HTMLInputElement>,
  value: string,
  placeholder: string
}

export const InputText= ({ onChangeHandler, value, placeholder }: InputNumberProps) => {
  return (
    <>
    <label htmlFor={placeholder}>{placeholder}</label>
    <input onChange={onChangeHandler} value={value} 
    placeholder={placeholder} className="input-number" type="text" />
    </>
  )
};

