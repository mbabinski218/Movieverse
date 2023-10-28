import { environment } from './common/environment';
import { GoogleOAuthProvider } from '@react-oauth/google';
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <GoogleOAuthProvider clientId={environment.googleClientId}>
     <React.StrictMode> {/* Elementy są renderowane dwa razy w trybie ścisłym (temu api może być wywoływane dwa razy) */}
      <App />
    </React.StrictMode>
  </GoogleOAuthProvider>
);