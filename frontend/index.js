import React from "react";
import ReactDOM from "react-dom";
import App from "./src/App";
import "./index.css"; // Jeśli chcesz stylować globalnie

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById("root")
);
