import React, { useEffect, useState } from "react";
import "./styles/PlanDisplayer.css";

function PlanDisplayer({ formData }) {
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        setLoading(true);
        const response = await fetch(
          "http://localhost:5000/api/plan/generate",
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              Goal: formData.Goal,
              Intensity: formData.Intensity,
              Duration: parseInt(formData.Duration, 10), // Ensure numeric value
            }),
          }
        );

        if (!response.ok) {
          const errorMessage = await response.text();
          throw new Error(`Failed to fetch plans: ${errorMessage}`);
        }

        const data = await response.json();
        setPlans(data); // Assuming the backend returns the correct JSON structure
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
      {plans.length > 0 ? (
        plans.map((plan, index) => (
          <div key={index} className="plan-card">
            <h3 className="plan-title">Plan {index + 1}</h3>
            {Array.isArray(plan.plan) && plan.plan.length > 0 ? (
              plan.plan.map((workout, workoutIndex) => (
                <div key={workoutIndex} className="workout-day">
                  <h4 className="workout-title">Workout {workout.workout}</h4>
                  {Array.isArray(workout.exercises) &&
                  workout.exercises.length > 0 ? (
                    <ul className="exercise-list">
                      {workout.exercises.map((exercise, idx) => (
                        <li key={idx} className="exercise-item">
                          <span className="exercise-name">
                            {exercise.name}:
                          </span>{" "}
                          {exercise.reps || exercise.duration} |{" "}
                          <span className="exercise-sets">
                            {exercise.sets} sets
                          </span>
                        </li>
                      ))}
                    </ul>
                  ) : (
                    <p>No exercises found for this workout.</p>
                  )}
                </div>
              ))
            ) : (
              <p>No workout data available for this plan.</p>
            )}
          </div>
        ))
      ) : (
        <p>No plans available.</p>
      )}
    </div>
  );
}

export default PlanDisplayer;
