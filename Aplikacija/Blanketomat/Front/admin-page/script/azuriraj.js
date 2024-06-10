document.querySelectorAll('form[id^="update"]').forEach(form => {
  form.addEventListener('submit', function (event) {
      event.preventDefault();

      let id, data, url;

      if (form.id === 'updateProfessorForm') 
      {
          id = document.getElementById('updateProfessorId').value;
          data = {
              Ime: document.getElementById('updateName').value,
              Prezime: document.getElementById('updateSurname').value,
              Email: document.getElementById('updateEmail').value,
              Password: document.getElementById('updatePassword').value
          };
          url = `http://localhost:5246/Profesor/${id}`;
      } 
      else if (form.id === 'updateStudentForm') 
      {
          id = document.getElementById('updateStudentId').value;
          data = {
              Ime: document.getElementById('updateStudentName').value,
              Prezime: document.getElementById('updateStudentSurname').value,
              Email: document.getElementById('updateStudentEmail').value,
              Password: document.getElementById('updateStudentPassword').value
          };
          url = `http://localhost:5246/api/Student/${id}`;
      } 
      else if (form.id === 'updateAssistantForm') 
      {
          id = document.getElementById('updateAssistantId').value;
          data = {
              Ime: document.getElementById('updateAssistantName').value,
              Prezime: document.getElementById('updateAssistantSurname').value,
              Email: document.getElementById('updateAssistantEmail').value,
              Password: document.getElementById('updateAssistantPassword').value
          };
          url = `http://localhost:5246/Asistent/${id}`;
      }

      fetch(url, {
          method: 'PUT',
          headers: {
              'Content-Type': 'application/json'
          },
          body: JSON.stringify(data)
      })
      .then(response => {
          if (!response.ok) {
              return response.text().then(text => { throw new Error(text) });
          }
          return response.json();
      })
      .then(data => {
          console.log('Success:', data);
          alert('Uspešno ažurirano!');
      })
      .catch((error) => {
          console.error('Error:', error);
          alert(`Došlo je do greške prilikom ažuriranja: ${error.message}`);
      });
  });
});
