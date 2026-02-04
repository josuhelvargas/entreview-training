import { applyMiddleware, configureStore, type Reducer  } from "@reduxjs/toolkit";
import inventoryReducer from "../features/inventory/inventorySlice";


// const preloadedState = {
//   inventory :[],
//   auth:{
//     user: {},
//     token: {}
//   }
// }

export const store = configureStore({
  reducer: {
    inventory: inventoryReducer
  }, 
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({
    serializableCheck:false,
  }),
  devTools:  import.meta.env.MODE !== 'production', 
  // preloadedState,
  //enhancers: 
})


