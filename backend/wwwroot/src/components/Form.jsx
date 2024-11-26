// Import what we need
import React, { useState } from "react";
import { getDatabase, ref, push } from "firebase/database";
import { initializeApp } from "firebase/app";

// Firebase setup
const firebaseConfig = {
  authDomain: "coolfitness-f1486.firebaseapp.com",
  databaseURL: "https://coolfitness-f1486-default-rtdb.firebaseio.com/",
  projectId: "coolfitness-f1486",
  storageBucket: "coolfitness-f1486.appspot.com",
  messagingSenderId: "1061755816855",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const database = getDatabase(app);

// This is our form component
function Form() {
  // Keeps track of what the user types
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

  // Handles messages we show the user (like errors or success)
  const [notification, setNotification] = useState("");

  // Update the form state when something is typed
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  // Make sure the data is valid before doing anything with it
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

    return null; // Everything checks out
  };

  // When the user submits the form
  const handleSubmit = () => {
    const validationError = validateFormData(formData);
    if (validationError) {
      setNotification(validationError); // Show them what went wrong
      return;
    }

    // Send the data to Firebase
    const entriesRef = ref(database, "Entries");
    push(entriesRef, formData)
      .then(() => {
        setNotification("Thank you for signing up! We’ll be in touch soon."); // All good
        setFormData({
          name: "",
          email: "",
          phoneNumber: "",
          height: "",
          weight: "",
          age: "",
          gymExperience: "",
          muscleGroups: "",
        }); // Reset the form
      })
      .catch((error) => {
        setNotification("Something went wrong. Please try again."); // Uh-oh
        console.error(error); // Log the error for debugging
      });
  };

  // The form UI
  return (
    <div>
      <p className="formparagraph">
        Your personal trainer will create a workout plan just for you.
      </p>
      <p className="formtask">Please fill out the form:</p>
      <input
        type="text"
        name="name"
        placeholder="Name"
        value={formData.name}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="email"
        name="email"
        placeholder="Email"
        value={formData.email}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="phoneNumber"
        placeholder="Phone Number"
        value={formData.phoneNumber}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="height"
        placeholder="Height (cm)"
        value={formData.height}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="weight"
        placeholder="Weight (kg)"
        value={formData.weight}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="age"
        placeholder="Age"
        value={formData.age}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="gymExperience"
        placeholder="Gym Experience (months)"
        value={formData.gymExperience}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="muscleGroups"
        placeholder="Muscle Groups (all, legs, upper body)"
        value={formData.muscleGroups}
        onChange={handleChange}
      />
      <p></p>
      <button onClick={handleSubmit}>Submit</button>
      {notification && (
        <p style={{ color: "green", marginTop: "10px" }}>{notification}</p>
      )}
    </div>
  );
}

export default Form;
