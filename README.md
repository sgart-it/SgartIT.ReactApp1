# SgartIT.ReactApp1

Demo app in React function components con supporto di più repositories (in memory, MsSql, MsSql Dapper, MsSql Entity Framework, Sqlite e SharePoint Online)

Versione con **useReducer** e l'uso di **useEffect** (App.tsx) per gestire le chiamate asincrone.

Vedi:
- App.tsx
- reducer
    - AppState.ts
    - AppReducer.ts
    - AppContext.ts

Esempio di template root compoment

```jsx
const App: FC = () => {
  const [state, dispatch] = useReducer(AppReducer, AppInitialState);
  const { text, actionTypeAsync } = state;

  const executeActionTypeAsync = async () => {
    try {
      dispatch({ type: 'loading' });

      const items = await TodoService.search(text);
      dispatch({ type: 'set-todos', values: items ?? [] });

    } catch (err) {
      dispatch({ type: 'set-error', value: err?.toString() });
    } finally {
      dispatch({ type: 'loaded' });
    }
  };

  useEffect(() => {
    console.log('useEffect', actionTypeAsync.toString());
    void executeActionTypeAsync();
  }, [actionTypeAsync]);

  return (
    <BrowserRouter>
      <AppContext.Provider value={{ state, dispatch }}>
        ...
      </AppContext.Provider>
    </BrowserRouter>
  );
```