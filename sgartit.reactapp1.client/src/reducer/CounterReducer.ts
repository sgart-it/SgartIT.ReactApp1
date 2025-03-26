
export type CountAction = {
  type: 'increment' | 'decrement';
  quantity: number;
};

export type CountState = {
  count: number;
};

export const CounterReducer = (state: CountState, action: CountAction) => {
  const { type, quantity } = action;

  switch (type) {
    case "increment":
      return {
        ...state,
        count: state.count + quantity,
      };
    case "decrement":
      return {
        ...state,
        count: state.count - quantity,
      };
    default:
      return state;
  }
};
