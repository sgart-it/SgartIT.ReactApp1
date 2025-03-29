import React, { Dispatch } from 'react';
import { CountAction, CountState } from './CounterReducer';

type ContextProps = {
  state: CountState;
  dispatch: Dispatch<CountAction>
};

const CouterContext = React.createContext({} as ContextProps);

const useCounterContext = () => React.useContext(CouterContext);

export {
  useCounterContext
}

export default CouterContext;