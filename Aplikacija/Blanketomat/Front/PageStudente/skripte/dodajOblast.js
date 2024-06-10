document.addEventListener("DOMContentLoaded", function () {
  const searchPredmetiInput = document.getElementById('search-predmeti1');
  const predmetiDropdown = document.getElementById('predmeti-dropdown1');
  const dugme = document.getElementById('dugme');
  const inputNazivOblasti = document.getElementById('task-text1');
  let selectedPredmetId = null;

  // Function to fetch predmeti from the backend
  async function fetchPredmeti() {
      try {
          const response = await fetch('http://localhost:5246/Predmet/predmeti');
          if (response.ok) {
              const predmeti = await response.json();
              populateDropdown(predmeti);
          } else {
              console.error('Failed to fetch predmeti:', response.statusText);
          }
      } catch (error) {
          console.error('Error fetching predmeti:', error);
      }
  }

  // Function to populate the dropdown menu with predmeti
  function populateDropdown(predmeti) {
      predmetiDropdown.innerHTML = '';
      predmeti.forEach(predmet => {
          const item = document.createElement('a');
          item.classList.add('dropdown-item');
          item.textContent = predmet.naziv;
          item.href = '#';
          item.addEventListener('click', () => {
              searchPredmetiInput.value = predmet.naziv;
              predmetiDropdown.classList.remove('show');
              selectedPredmetId = predmet.id; // Save the selected predmet ID
          });
          predmetiDropdown.appendChild(item);
      });
  }

  // Event listener to show dropdown when input is focused
  searchPredmetiInput.addEventListener('focus', () => {
      predmetiDropdown.classList.add('show');
  });

  // Event listener to hide dropdown when input loses focus
  searchPredmetiInput.addEventListener('blur', () => {
      setTimeout(() => {
          predmetiDropdown.classList.remove('show');
      }, 200);
  });

  // Fetch predmeti when the page loads
  fetchPredmeti();

  // Event listener for button click to add a new oblast
  dugme.addEventListener('click', function(event) {
      event.preventDefault();
      const nazivOblasti = inputNazivOblasti.value;
      if (selectedPredmetId && nazivOblasti) {
          const newOblast = {
              naziv: nazivOblasti
          };
          fetch(`http://localhost:5246/Predmet/${selectedPredmetId}/oblasti`, {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify(newOblast)
          })
          .then(response => response.json())
          .then(data => {
              console.log('Nova oblast dodata:', data);
              alert('Oblast je uspešno dodata!');
          })
          .catch(error => console.error('Error adding oblast:', error));
      } else {
          alert('Molimo odaberite predmet i unesite naziv oblasti.');
      }
  });
});
