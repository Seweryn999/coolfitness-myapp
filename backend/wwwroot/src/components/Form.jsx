import React from "react";

function Form() {
  return (
    <div>
      <p className="formparagraph">
        Twój personaly trener stworzy plan treningowy specjalnie dla ciebie.
      </p>
      <p className="formtask">Wypełnij ankiete: </p>
      <input type="text" placeholder="Imię" />
      <p></p>
      <input type="text" placeholder="Wzrost" />
      <p></p>
      <input type="text" placeholder="Waga ciała" />
      <p></p>
      <input type="text" placeholder="Wiek" />
      <p></p>
      <input type="text" placeholder="Staż na siłowni (miesiące)" />
    </div>
  );
}

export default Form;
