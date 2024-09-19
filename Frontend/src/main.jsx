import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import { BrowserRouter } from "react-router-dom";
import { ErrorProvider } from "./contexts/ErrorContext.jsx";
import { LoadingProvider } from "./contexts/LoadingContext.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
  <BrowserRouter>
    <ErrorProvider>
      <LoadingProvider>
        <React.StrictMode>
          <App />
        </React.StrictMode>
      </LoadingProvider>
    </ErrorProvider>
  </BrowserRouter>
);
