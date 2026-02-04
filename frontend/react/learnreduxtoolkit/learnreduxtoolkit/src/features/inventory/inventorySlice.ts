import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction, CreateSliceOptions } from "@reduxjs/toolkit";
import { act } from "react";


 export interface Product {
  id:number,
  sku: string, 
  name:string,
  department:number, 
  group:string, 
  other:string,
  discount:number,
  isDiscounted:number,
}

const initialState : Product[] = [];

const options:CreateSliceOptions = {
  name: "Inventory",
  initialState: initialState,
  reducers: {
    
    addProduct(state, action:PayloadAction<Product>){
        state.push(action.payload);
    },

    deleteProductById(state,action:PayloadAction<number>) { 
        return state.filter( (product: Product) => product.id !== action.payload);
    }
    
  }
}






const inventorySlice = createSlice(options);
export const { addProduct, deleteProduct } = inventorySlice.actions;
export default inventorySlice.reducer;