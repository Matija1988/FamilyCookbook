import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import { BrowserRouter } from "react-router-dom";
import { ErrorProvider } from "./contexts/ErrorContext.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
  <BrowserRouter>
    <ErrorProvider>
      <React.StrictMode>
        <App />
      </React.StrictMode>
    </ErrorProvider>
  </BrowserRouter>
);
