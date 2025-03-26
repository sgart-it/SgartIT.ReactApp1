import { FC, useReducer } from 'react';
import Counter from '../../components/counter/Counter';
import { CounterReducer } from '../../reducer/CounterReducer';
import CouterContext from '../../reducer/CounterContext';

const CountPage: FC = () => {
  const [state, dispatch] = useReducer(CounterReducer, { count: 0 });

  const providerState = {
    state,
    dispatch,
  };


  return (
    <CouterContext.Provider value={providerState}>
      <div>
        <h2>CountPage</h2>

        <hr />
        <Counter title="Contatore 1" />
        <hr />
        <Counter title="Contatore 2" />
        <hr />
        <Counter title="Contatore 3" />
        <hr />
      </div>
    </CouterContext.Provider>
  );

}

export default CountPage;