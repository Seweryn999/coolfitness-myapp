import React, { useEffect, useState } from "react";

function PlanDisplayer({ formData }) {
  const [plan, setPlan] = useState(null); // Przechowywanie planu
  const [loading, setLoading] = useState(false); // Status ładowania
  const [error, setError] = useState(null); // Obsługa błędów

  useEffect(() => {
    const fetchPlan = async () => {
      try {
        setLoading(true);
        setError(null); // Resetowanie błędów
        setPlan(null); // Resetowanie poprzedniego planu

        console.log("Wysyłanie danych do backendu:", formData);

        // Walidacja danych formularza
        if (
          !formData ||
          !formData.Goal ||
          !formData.Intensity ||
          !formData.Duration
        ) {
          throw new Error(
            "Nieprawidłowe dane formularza. Upewnij się, że wszystkie pola są wypełnione."
          );
        }

        // Wysłanie żądania do backendu
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

        console.log("Status odpowiedzi backendu:", response.status);

        if (!response.ok) {
          const errorMessage = await response.json();
          throw new Error(
            `Błąd HTTP! Status: ${response.status} - ${errorMessage.message}`
          );
        }

        const data = await response.json();
        console.log("Odebrane dane z backendu:", data);
        setPlan(data); // Przechowywanie planu w stanie
      } catch (err) {
        console.error("Błąd podczas pobierania planu:", err);
        setError(err.message || "Nie udało się pobrać planu treningowego.");
      } finally {
        setLoading(false);
      }
    };

    if (formData) {
      fetchPlan();
    }
  }, [formData]);

  if (loading) {
    return <div>Ładowanie...</div>;
  }

  if (error) {
    return <div>Błąd: {error}</div>;
  }

  if (!plan) {
    return <div>Brak planu. Podaj poprawne dane formularza.</div>;
  }

  return (
    <div>
      <h2>Twój Plan Treningowy</h2>
      <div>
        <h3>Cel: {plan.goal}</h3>
        <h3>Intensywność: {plan.intensity}</h3>
        <h3>Czas trwania: {plan.duration} minut</h3>
        <div>
          <h4>Plan treningowy:</h4>
          <pre>{plan.planDetails}</pre>
        </div>
      </div>
    </div>
  );
}

export default PlanDisplayer;
