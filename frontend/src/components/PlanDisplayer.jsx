import React, { useEffect, useState } from "react";

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

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h2>Welcome, {formData.Name}! Here are your workout plans:</h2>
      {plans.map((plan, index) => (
        <div key={index}>
          <h3>Plan {index + 1}</h3>
          {plan.plan.map((workout, workoutIndex) => (
            <div key={workoutIndex}>
              <h4>Workout {workout.workout}</h4>
              <ul>
                {workout.exercises.map((exercise, idx) => (
                  <li key={idx}>
                    {exercise.name}: {exercise.reps || exercise.duration} |{" "}
                    {exercise.sets} sets
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
