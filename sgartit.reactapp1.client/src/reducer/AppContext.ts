import React, { Dispatch } from 'react';
import { AppAction } from './AppReducer';
import { AppState } from "./AppState";

type AppProps = {
  state: AppState;
  dispatch: Dispatch<AppAction>
};

const AppContext = React.createContext({} as AppProps);

const useAppContext = () => React.useContext(AppContext);

export {
  AppContext,
  useAppContext
};
