import React, { useState } from "react";
import Form from "./components/Form";
import PlanDisplayer from "./components/PlanDisplayer";

const App = () => {
  const [formData, setFormData] = useState(null);

  const handleFormSubmit = (data) => {
    setFormData(data);
  };

  return (
    <div>
      <h1>Welcome to CoolFitness</h1>
      {!formData ? (
        <Form onSubmit={handleFormSubmit} />
      ) : (
        <PlanDisplayer formData={formData} />
      )}
    </div>
  );
};

export default App;
