import React, { useEffect, useState } from "react";
import "./styles/PlanDisplayer.css"; // Dodajemy import stylów

function PlanDisplayer({ formData }) {
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        setLoading(true);
        const response = await fetch(
<<<<<<< HEAD
          "http://localhost:5000/api/plan/generate",
=======
          "https://localhost:5000/api/plan/generate",
>>>>>>> 030072258cd07e2fcf4ddae6ace2e53f1040193d
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(formData),
          }
        );

        if (!response.ok) {
          throw new Error("Failed to fetch plans.");
        }

        const data = await response.json();
        setPlans(data.plans);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    if (formData) fetchPlans();
  }, [formData]);

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (
    <div className="container">
      <h2 className="welcome">
        Welcome, <span className="username">{formData.Name}</span>! Here are
        your workout plans:
      </h2>
      {plans.map((plan, index) => (
        <div key={index} className="plan-card">
          <h3 className="plan-title">Plan {index + 1}</h3>
          {plan.plan.map((workout, workoutIndex) => (
            <div key={workoutIndex} className="workout-day">
              <h4 className="workout-title">Workout {workout.workout}</h4>
              <ul className="exercise-list">
                {workout.exercises.map((exercise, idx) => (
                  <li key={idx} className="exercise-item">
                    <span className="exercise-name">{exercise.name}:</span>{" "}
                    {exercise.reps || exercise.duration} |{" "}
                    <span className="exercise-sets">{exercise.sets} sets</span>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      ))}
    </div>
  );
}

export default PlanDisplayer;
