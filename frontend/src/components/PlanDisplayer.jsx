import React, { useEffect, useState } from "react";

function PlanDisplayer({ formData }) {
  const [plan, setPlan] = useState(null); // Przechowywanie planu
  const [loading, setLoading] = useState(false); // Status ładowania
  const [error, setError] = useState(null); // Obsługa błędów

  // Efekt odpowiadający za pobranie planu
  useEffect(() => {
    const fetchPlan = async () => {
      setLoading(true);
      setError(null); // Resetowanie błędów
      setPlan(null); // Resetowanie poprzedniego planu

      console.log("Wysyłanie danych do backendu:", formData);

      try {
        // Walidacja formData
        if (!formData || Object.keys(formData).length === 0) {
          throw new Error("Nieprawidłowe dane formularza.");
        }

        // Wysyłanie żądania do backendu
        const response = await fetch(
          "http://localhost:5000/api/plan/generate", // Endpoint backendu
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json", // Typ danych
            },
            body: JSON.stringify(formData), // Przekazanie danych w żądaniu
          }
        );

        console.log("Status odpowiedzi backendu:", response.status);

        // Obsługa niepowodzeń w odpowiedzi
        if (!response.ok) {
          throw new Error(`Błąd HTTP! Status: ${response.status}`);
        }

        // Parsowanie odpowiedzi
        const data = await response.json();
        console.log("Odebrane dane z backendu:", data);

        // Ustawienie odebranego planu
        setPlan(data);
      } catch (err) {
        console.error("Błąd podczas pobierania planu:", err);
        setError(err.message || "Nie udało się pobrać planu treningowego.");
      } finally {
        setLoading(false); // Wyłączenie ładowania
      }
    };

    // Wywołanie funkcji, jeśli dostępne są dane formularza
    if (formData) {
      fetchPlan();
    }
  }, [formData]);

  // Renderowanie, gdy trwa ładowanie
  if (loading) {
    return <div>Ładowanie...</div>;
  }

  // Renderowanie w przypadku błędu
  if (error) {
    return <div>Błąd: {error}</div>;
  }

  // Renderowanie, gdy plan nie jest dostępny
  if (!plan) {
    return <div>Brak planu. Podaj poprawne dane formularza.</div>;
  }

  // Renderowanie odebranego planu treningowego
  return (
    <div>
      <h2>Twój Plan Treningowy</h2>
      <div>
        <h3>Cel: {plan.goal}</h3>
        <h3>Intensywność: {plan.intensity}</h3>
        <h3>Czas trwania: {plan.duration} minut</h3>
        <p>{plan.planDetails}</p>
      </div>
    </div>
  );
}

export default PlanDisplayer;
