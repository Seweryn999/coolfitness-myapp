import React, { useEffect, useState } from "react";

function PlanDisplayer({ formData }) {
  const [plan, setPlan] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPlan = async () => {
      try {
        const response = await fetch(
          "http://localhost:5000/api/plan/generate",
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(formData),
          }
        );

        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        setPlan(data);
      } catch (err) {
        console.error("Error fetching plan:", err);
        setError("Failed to fetch the training plan.");
      } finally {
        setLoading(false);
      }
    };

    if (formData) {
      fetchPlan();
    }
  }, [formData]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!plan) {
    return <div>No plan available.</div>;
  }

  return (
    <div>
      <h2>Your Training Plan</h2>
      <div>
        <h3>Goal: {plan.goal}</h3>
        <h3>Intensity: {plan.intensity}</h3>
        <h3>Duration: {plan.duration} minutes</h3>
        <p>{plan.planDetails}</p>
      </div>
    </div>
  );
}

export default PlanDisplayer;
