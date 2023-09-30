import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import 'bootstrap/dist/css/bootstrap.min.css';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode> {/* Elementy są renderowane dwa razy w trybie ścisłym (temu api może być wywoływane dwa razy) */}
    <App />
  </React.StrictMode>
);