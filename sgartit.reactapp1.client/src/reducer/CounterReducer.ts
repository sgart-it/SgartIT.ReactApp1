
export type CountAction = {
  type: 'increment' | 'decrement' | 'reset';
  quantity?: number;
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
        count: state.count + (quantity ?? 1)
      };
    case "decrement":
      if (state.count - (quantity ?? 1) < 0) {
        return {
          ...state,
          count: 0
        };
      }
      return {
        ...state,
        count: state.count  - (quantity ?? 1)
      };
    case "reset":
      return {
        ...state,
        count: 0
      };
    default:
      return state;
  }
};
