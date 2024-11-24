import React, { useState } from "react";
import { getDatabase, ref, push } from "firebase/database";
import { initializeApp } from "firebase/app";

const firebaseConfig = {
  authDomain: "coolfitness-f1486.firebaseapp.com",
  databaseURL: "https://coolfitness-f1486-default-rtdb.firebaseio.com/",
  projectId: "coolfitness-f1486",
  storageBucket: "coolfitness.appspot.com",
  messagingSenderId: "1061755816855",
  appId: "YOUR_APP_ID",
};

const app = initializeApp(firebaseConfig);
const database = getDatabase(app);

function Form() {
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

  const handleSubmit = () => {
    const entriesRef = ref(database, "Entries");
    push(entriesRef, formData)
      .then(() => {
        setNotification("Thank you for signing up! We will be in touch soon.");
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
      })
      .catch((error) => {
        setNotification("Something went wrong. Please try again.");
        console.error(error);
      });
  };

  return (
    <div>
      <p className="formparagraph">
        Twój personalny trener stworzy plan treningowy specjalnie dla ciebie.
      </p>
      <p className="formtask">Wypełnij ankietę:</p>
      <input
        type="text"
        name="name"
        placeholder="Imię"
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
        placeholder="Numer Telefonu"
        value={formData.phoneNumber}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="height"
        placeholder="Wzrost"
        value={formData.height}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="weight"
        placeholder="Waga ciała"
        value={formData.weight}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="age"
        placeholder="Wiek"
        value={formData.age}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="gymExperience"
        placeholder="Staż na siłowni (miesiące)"
        value={formData.gymExperience}
        onChange={handleChange}
      />
      <p></p>
      <input
        type="text"
        name="muscleGroups"
        placeholder="(all, leggs, upper body)"
        value={formData.muscleGroups}
        onChange={handleChange}
      />
      <p></p>
      <button onClick={handleSubmit}>Submit</button>
      {/* Display notification */}
      {notification && (
        <p style={{ color: "green", marginTop: "10px" }}>{notification}</p>
      )}
    </div>
  );
}

export default Form;
