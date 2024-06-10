document.getElementById('obrisiPredmetForm').addEventListener('submit', function(event) {
  event.preventDefault();
  
  const predmetID = document.getElementById('predmetId').value;
  
  fetch(`http://localhost:5246/Predmet/${predmetID}`, {
      method: 'DELETE'
  })
  .then(response => {
      if (response.ok) {
          alert("predmet uspešno obrisan,sva pitanja ,svi zadaci, svi blanketi, oblasti i podablasti vezana za ovaj predmet su takođe obrisana.");
      } else {
          alert("Došlo je do greške prilikom brisanja predmeta");
      }
  })
  .catch(error => {
      console.error("Došlo je do greške:", error);
      alert("Došlo je do greške prilikom brisanja predmeta");
  });
});