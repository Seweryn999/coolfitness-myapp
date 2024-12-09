import React, { useState } from "react";
import Form from "./components/Form";
import PlanDisplayer from "./components/PlanDisplayer";
import "./components/styles/App.css"; // Importujemy styl

const App = () => {
  const [formData, setFormData] = useState(null);

  const handleFormSubmit = (data) => {
    setFormData(data);
  };

  return (
    <div className="app-container">
      <header className="app-header">
        <h1 className="app-title">Welcome to CoolFitness</h1>
        <h2 className="app-subtitle">
          CoolFitness is your new personal trainer
        </h2>
        <h2 className="app-subtitle">
          Just fill in the form and go hit that gains
        </h2>
      </header>
      <main>
        {!formData ? (
          <Form onSubmit={handleFormSubmit} />
        ) : (
          <PlanDisplayer formData={formData} />
        )}
      </main>
    </div>
  );
};

export default App;
