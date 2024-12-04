import React, { useEffect, useState } from "react";

function PlanDisplayer({ formData }) {
  const [plan, setPlan] = useState(null); // Przechowywanie planu
  const [loading, setLoading] = useState(false); // Status ładowania
  const [error, setError] = useState(null); // Obsługa błędów

  useEffect(() => {
    const fetchPlan = async () => {
      setLoading(true);
      setError(null); // Resetowanie błędów
      setPlan(null); // Resetowanie poprzedniego planu

      console.log("Wysyłanie danych do backendu:", formData);

      try {
        if (!formData || Object.keys(formData).length === 0) {
          throw new Error("Nieprawidłowe dane formularza.");
        }

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
        console.log("Odpowiedź: ", response);

        const textResponse = await response.text();
        console.log("Odpowiedź tekstowa z backendu:", textResponse);

        if (!response.ok) {
          const errorMessage = JSON.parse(textResponse);
          console.error("Błąd odpowiedzi: ", errorMessage.message);
          throw new Error(
            `Błąd HTTP! Status: ${response.status} - ${errorMessage.message}`
          );
        }

        let data;
        try {
          data = JSON.parse(textResponse);
        } catch (error) {
          console.error("Błąd parsowania odpowiedzi JSON:", error);
          throw new Error("Odpowiedź nie jest poprawnym JSON-em.");
        }

        console.log("Odebrane dane z backendu:", data);

        setPlan(data);
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
        <h3>Cel: {plan.Goal}</h3>
        <h3>Intensywność: {plan.Intensity}</h3>
        <h3>Czas trwania: {plan.Duration} minut</h3>
        <p>{plan.PlanDetails}</p>
      </div>
    </div>
  );
}

export default PlanDisplayer; // Upewnij się, że tutaj jest `export default`
