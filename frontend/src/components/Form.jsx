import React, { useState } from "react";
import { getDatabase, ref, push } from "firebase/database";
import { initializeApp } from "firebase/app";
import "./styles/Form.css"; // Import stylów

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
    Name: "",
    Gender: "",
    Goal: "",
    Intensity: "",
    Duration: "",
    ExperienceLevel: "",
  });

  const [notification, setNotification] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const validateFormData = (data) => {
    if (
      !data.Name ||
      !data.Gender ||
      !data.Goal ||
      !data.Intensity ||
      !data.Duration ||
      !data.ExperienceLevel
    ) {
      return "Please fill in all the fields.";
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
        setNotification("Thank you for signing up! Here is your plan.");
        onSubmit(formData);
      })
      .catch((error) => {
        setNotification("Something went wrong. Please try again.");
        console.error(error);
      });
  };

  return (
    <div className="form-container">
      <p className="form-intro">
        Your personal trainer will create a workout plan just for you.
      </p>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="Name">Name:</label>
          <input
            id="Name"
            type="text"
            name="Name"
            placeholder="Your Name"
            value={formData.Name}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="Gender">Gender:</label>
          <select
            id="Gender"
            name="Gender"
            value={formData.Gender}
            onChange={handleChange}
          >
            <option value="">Select Gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="ExperienceLevel">Experience Level:</label>
          <select
            id="ExperienceLevel"
            name="ExperienceLevel"
            value={formData.ExperienceLevel}
            onChange={handleChange}
          >
            <option value="">Select Level</option>
            <option value="Beginner">Beginner</option>
            <option value="Intermediate">Intermediate</option>
            <option value="Advanced">Advanced</option>
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="Goal">Goal:</label>
          <input
            id="Goal"
            type="text"
            name="Goal"
            placeholder="Weight loss, Muscle Gain"
            value={formData.Goal}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="Intensity">Muscle Group</label>
          <input
            id="Intensity"
            type="text"
            name="Intensity"
            placeholder="Full body, Upper body, Legs"
            value={formData.Intensity}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
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
        <button className="submit-button" type="submit">
          Submit
        </button>
      </form>
      {notification && <p className="notification">{notification}</p>}
    </div>
  );
}

export default Form;
