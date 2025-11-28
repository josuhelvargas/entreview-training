


import type { ChangeEventHandler } from 'react';

export interface InputNumberProps {
  onChangeHandler: ChangeEventHandler<HTMLInputElement>,
  value: number,
  placeholder: string
}

export const InputNumber = ({ onChangeHandler, value, placeholder }: InputNumberProps) => {
  return (
    <>
      <label htmlFor={placeholder}>{placeholder}</label>
      <input onChange={onChangeHandler} value={value}
        placeholder={placeholder} className="input-number" type="number" />
    </>
  )
};


