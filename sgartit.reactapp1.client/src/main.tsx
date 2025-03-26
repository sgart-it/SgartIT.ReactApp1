import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// thema PrimeReact
import 'primereact/resources/themes/bootstrap4-dark-blue/theme.css';
import 'primeicons/primeicons.css';
import locale_it from 'primelocale/it.json';
import './index.css'

import App from './App.tsx'
import { addLocale, APIOptions, LocaleOptions, PrimeReactProvider } from 'primereact/api'

addLocale('it', locale_it.it as LocaleOptions);

// https://primereact.org/configuration/
const providerOptions: Partial<APIOptions> = {
  appendTo: 'self',
  cssTransition: true,
  locale: 'it'
};

//locale('it');

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <PrimeReactProvider value={providerOptions}>
      <App />
    </PrimeReactProvider>
  </StrictMode>,
)
