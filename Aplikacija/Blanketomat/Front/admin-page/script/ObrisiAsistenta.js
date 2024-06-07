document.getElementById('deleteAistentForm').addEventListener('submit', function(event) {
  event.preventDefault();
  
  const asistentId = document.getElementById('assistantId').value;
  
  fetch(`http://localhost:5246/Asistent/${asistentId}`, {
      method: 'DELETE'
  })
  .then(response => {
      if (response.ok) {
          alert("Asistent uspešno obrisan");
      } else {
          alert("Došlo je do greške prilikom brisanja asistenta");
      }
  })
  .catch(error => {
      console.error("Došlo je do greške:", error);
      alert("Došlo je do greške prilikom brisanja asistenta");
  });
});
