document.getElementById('deleteProfessorForm').addEventListener('submit', function(event) {
  event.preventDefault();
  
  const professorId = document.getElementById('professorId').value;
  
  fetch(`http://localhost:5246/Profesor/${professorId}`, {
      method: 'DELETE'
  })
  .then(response => {
      if (response.ok) {
          alert("Profesor uspešno obrisan");
      } else {
          alert("Došlo je do greške prilikom brisanja profesora");
      }
  })
  .catch(error => {
      console.error("Došlo je do greške:", error);
      alert("Došlo je do greške prilikom brisanja profesora");
  });
});
