import { FC } from "react";
import { useCounterContext } from "../../reducer/CounterContext";

type CounterProps = {
  title: string;
  quantity?: number;
};

const Counter: FC<CounterProps> = ({ title, quantity }) => {
  const { state, dispatch } = useCounterContext();

  const quantityInc = quantity ?? 1;

  return (
    <div>
      <strong>{title}</strong> count: {state.count} &#160;

      <button onClick={() => dispatch({ type: "decrement", quantity: quantityInc })}> - </button>
      <button onClick={() => dispatch({ type: "increment", quantity: quantityInc })}> + </button>

      <span style={{ marginLeft: "1em" }}>Increment: {quantityInc }</span>
    </div>
  );
};

export default Counter;
