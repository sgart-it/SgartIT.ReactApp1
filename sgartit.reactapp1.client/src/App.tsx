import { FC, useEffect, useReducer } from 'react';
import './App.css';
import { BrowserRouter } from 'react-router-dom';
import Router from './router/Router';
import MainNavigation from './router/MainNavigation';
import { AppContext } from './reducer/AppContext';
import { ACT, AppInitialState } from "./reducer/AppState";
import { AppReducer } from './reducer/AppReducer';
import { TodoService } from './services/todoService';


const App: FC = () => {
  const [state, dispatch] = useReducer(AppReducer, AppInitialState);
  const { text, actionTypeAsync } = state;


  const executeActionTypeAsync = async (firstLoad?: boolean): Promise<void> => {
    try {
      dispatch({ type: 'loading' });

      if (actionTypeAsync === ACT.SearchTodoAsync || firstLoad === true) {
        const items = await TodoService.search(text);
        dispatch({ type: 'set-todos', values: items ?? [] });

      } else if (actionTypeAsync === ACT.DeleteTodoAsync) {
        if (state?.selectedTodo) {
          await TodoService.remove(state.selectedTodo?.id);
          const items = await TodoService.search(text);
          dispatch({ type: 'set-todos', values: items ?? [] });
        }

      } else if (actionTypeAsync === ACT.SaveTodoAsync) {
        if (state?.saveTodo) {
          await TodoService.save(state.saveTodo);
          const items = await TodoService.search(text);
          dispatch({ type: 'set-todos', values: items ?? [] });
        }

      }
    } catch (err) {
      dispatch({ type: 'set-error', value: err?.toString() });
    } finally {
      dispatch({ type: 'loaded' });
    }
  };
  useEffect(() => {
    //console.log('useEffect', actionTypeAsync);
    void executeActionTypeAsync(true);
  }, []);

  useEffect(() => {
    //console.log('useEffect', actionTypeAsync);
    void executeActionTypeAsync();
  }, [actionTypeAsync]);

  return (
    <BrowserRouter>
      <AppContext.Provider value={{ state, dispatch }}>
        <div className="App">
          <h1>SgartIT.ReactApp1</h1>
          <header>
            <MainNavigation />
          </header>
          <main>
            <Router />
          </main>
        </div>
      </AppContext.Provider>
    </BrowserRouter>
  );
}

export default App;