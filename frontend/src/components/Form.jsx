import React, { useState } from "react";
import { getDatabase, ref, push } from "firebase/database";
import { initializeApp } from "firebase/app";
import "./styles/Form.css";

const firebaseConfig = {
  authDomain: "coolfitness-f1486.firebaseapp.com",
  databaseURL: "https://coolfitness-f1486-default-rtdb.firebaseio.com/",
  projectId: "coolfitness-f1486",
  storageBucket: "coolfitness-f1486.appspot.com",
  messagingSenderId: "1061755816855",
};

const app = initializeApp(firebaseConfig);
const database = getDatabase(app);

function Form({ onSubmit }) {
  const [formData, setFormData] = useState({
    Goal: "", // Cel planu
    Intensity: "", // Intensywność (np. "upper body", "all")
    Duration: "", // Czas trwania planu w minutach
  });

  const [notification, setNotification] = useState("");

  // Obsługuje zmiany w formularzu
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  // Walidacja formularza
  const validateFormData = (data) => {
    if (!data.Goal || !data.Intensity || !data.Duration) {
      return "Please fill in all the fields.";
    }
    return null;
  };

  // Obsługuje wysyłanie formularza
  const handleSubmit = (e) => {
    e.preventDefault();

    const validationError = validateFormData(formData);
    if (validationError) {
      setNotification(validationError);
      return;
    }

    const entriesRef = ref(database, "Entries");
    push(entriesRef, formData)
      .then(() => {
        setNotification("Thank you for signing up! We’ll be in touch soon.");
        setFormData({
          Goal: "",
          Intensity: "",
          Duration: "",
        });
        onSubmit(formData); // Przekazanie danych do PlanDisplayer
      })
      .catch((error) => {
        setNotification("Something went wrong. Please try again.");
        console.error(error);
      });
  };

  return (
    <div className="form-container">
      <p>Your personal trainer will create a workout plan just for you.</p>
      <p>Please fill out the form:</p>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="Goal">Goal (e.g., weight loss, muscle gain):</label>
          <input
            id="Goal"
            type="text"
            name="Goal"
            placeholder="Goal"
            value={formData.Goal}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="Intensity">
            Intensity (e.g., upper body, full body):
          </label>
          <input
            id="Intensity"
            type="text"
            name="Intensity"
            placeholder="Intensity"
            value={formData.Intensity}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="Duration">Duration (in minutes):</label>
          <input
            id="Duration"
            type="number"
            name="Duration"
            placeholder="Duration"
            value={formData.Duration}
            onChange={handleChange}
          />
        </div>
        <button type="submit">Submit</button>
      </form>
      {notification && <p className="notification">{notification}</p>}
    </div>
  );
}

export default Form;
