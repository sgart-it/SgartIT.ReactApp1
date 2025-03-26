import { FC } from 'react';
import './App.css';
import { BrowserRouter } from 'react-router-dom';
import Router from './Router/Router';
import MainNavigation from './Router/MainNavigation';

const App: FC = () => {

  return (
    <BrowserRouter>
      <div className="App">
        <h1>SgartIT.ReactApp1</h1>
        <header>
          <MainNavigation />
        </header>
        <main>
          <Router />
        </main>
      </div>
    </BrowserRouter>
  );

}

export default App;