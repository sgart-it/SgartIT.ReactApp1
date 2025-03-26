import React, { Dispatch } from 'react';
import { CountAction, CountState } from './CounterReducer';

interface IContextProps {
  state: CountState;
  dispatch: Dispatch<CountAction>
}

const CouterContext = React.createContext({} as IContextProps);

export function useCustomContext() {
  return React.useContext(CouterContext);
}

export default CouterContext;