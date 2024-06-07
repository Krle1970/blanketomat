document.getElementById('deleteStudentForm').addEventListener('submit', function(event) {
  event.preventDefault();
  
  const studentid = document.getElementById('deleteStudentId').value;
  
  fetch(`http://localhost:5246/api/Student/${studentid}`, {
      method: 'DELETE'
  })
  .then(response => {
      if (response.ok) {
          alert("Student uspešno obrisan");
      } else {
          alert("Došlo je do greške prilikom brisanja studenta");
      }
  })
  .catch(error => {
      console.error("Došlo je do greške:", error);
      alert("Došlo je do greške prilikom brisanja studenta");
  });
});
