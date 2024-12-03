import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

// Upewnij się, że element o id "root" istnieje w DOM
const rootElement = document.getElementById("root");
if (rootElement) {
  ReactDOM.createRoot(rootElement).render(<App />);
} else {
  console.error('Element o id "root" nie został znaleziony w DOM');
}
