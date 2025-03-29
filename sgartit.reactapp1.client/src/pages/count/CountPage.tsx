import { FC, useReducer } from 'react';
import Counter from '../../components/counter/Counter';
import { CounterReducer } from '../../reducer/CounterReducer';
import CouterContext from '../../reducer/CounterContext';
import { Button } from 'primereact/button';

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
        <Counter title="Contatore 2" quantity={2} />
        <hr />
        <Counter title="Contatore 3" quantity={3} />
        <hr />
        <Button onClick={()=> dispatch({ type: "reset" })}>Reset</Button>
      </div>
    </CouterContext.Provider>
  );

}

export default CountPage;