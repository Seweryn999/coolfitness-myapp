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
    name: "",
    email: "",
    phoneNumber: "",
    height: "",
    weight: "",
    age: "",
    gymExperience: "",
    muscleGroups: "",
  });

  const [notification, setNotification] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const validateFormData = (data) => {
    if (!data.name || data.name.trim().length < 3) {
      return "The name must contain at least 3 characters.";
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
      return "Please provide a valid email address.";
    }

    if (!/^\d{9,}$/.test(data.phoneNumber)) {
      return "Please provide a valid phone number (at least 9 digits).";
    }

    const height = parseInt(data.height, 10);
    if (isNaN(height) || height < 150 || height > 250) {
      return "Height must be a number between 150-250 cm.";
    }

    const weight = parseInt(data.weight, 10);
    if (isNaN(weight) || weight < 30 || weight > 300) {
      return "Weight must be a number between 30-300 kg.";
    }

    const age = parseInt(data.age, 10);
    if (isNaN(age) || age < 16 || age > 100) {
      return "Age must be a number between 16-100 years.";
    }

    const gymExperience = parseInt(data.gymExperience, 10);
    if (isNaN(gymExperience) || gymExperience < 0 || gymExperience > 600) {
      return "Gym experience must be a number of months between 0-600.";
    }

    if (
      !["all", "legs", "upper body"].includes(
        data.muscleGroups.trim().toLowerCase()
      )
    ) {
      return "Please provide a valid muscle group (all, legs, upper body).";
    }

    return null;
  };

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
          name: "",
          email: "",
          phoneNumber: "",
          height: "",
          weight: "",
          age: "",
          gymExperience: "",
          muscleGroups: "",
        });
        onSubmit(formData); 
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
        <input
          type="text"
          name="name"
          placeholder="Name"
          value={formData.name}
          onChange={handleChange}
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleChange}
        />
        <input
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          value={formData.phoneNumber}
          onChange={handleChange}
        />
        <input
          type="text"
          name="height"
          placeholder="Height (cm)"
          value={formData.height}
          onChange={handleChange}
        />
        <input
          type="text"
          name="weight"
          placeholder="Weight (kg)"
          value={formData.weight}
          onChange={handleChange}
        />
        <input
          type="text"
          name="age"
          placeholder="Age"
          value={formData.age}
          onChange={handleChange}
        />
        <input
          type="text"
          name="gymExperience"
          placeholder="Gym Experience (months)"
          value={formData.gymExperience}
          onChange={handleChange}
        />
        <input
          type="text"
          name="muscleGroups"
          placeholder="Muscle Groups (all, legs, upper body)"
          value={formData.muscleGroups}
          onChange={handleChange}
        />
        <button type="submit">Submit</button>
      </form>
      {notification && <p className="notification">{notification}</p>}
    </div>
  );
}

export default Form;
