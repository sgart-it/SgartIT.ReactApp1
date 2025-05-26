import { AppInitialState, AppState } from "../reducer/AppState";

export type StoreAction = {
  type: 'search-todo' | 'loading' | 'loaded' | 'new-todo' | 'delete-todo';
};

const appState: AppState = AppInitialState;
const subscribers = new Set(); // Set to store callback functions

const getSnapshot = (): AppState => {
  return appState;
};

const subscribe = (callback: () => void): (() => void) => {
  subscribers.add(callback);
  return () => subscribers.delete(callback);
};

const useAppStore = (): [Theme, (newTheme: Theme) => void] => {
  const theme = useSyncExternalStore(subscribe, getSnapshot);

  const setTheme = (newTheme: Theme) => {
    localStorage.setItem(THEME_STORAGE_KEY, newTheme);
    window.dispatchEvent(new Event("storage"));
  };

  return [theme, setTheme];
};

export default useAppStore;
