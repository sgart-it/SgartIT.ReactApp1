import { FC } from "react";
import { useCounterContext } from "../../reducer/CounterContext";

type CounterProps = {
  title: string;
};

const Counter: FC<CounterProps> = ({ title }) => {
  const { state, dispatch } = useCounterContext();

  return (
    <div>
      <strong>{title}</strong> count: {state.count} &#160;

      <button onClick={() => dispatch({ type: "decrement", quantity: 1 })}> - </button>
      <button onClick={() => dispatch({ type: "increment", quantity: 1 })}> + </button>
    </div>
  );
};

export default Counter;
