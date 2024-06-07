document.addEventListener('DOMContentLoaded', function() {
  const dropdownMenu = document.getElementById('predmeti-dropdown1');

  // Fetch the subjects from the API
  fetch('http://localhost:5246/Predmet/predmeti')
      .then(response => response.json())
      .then(data => {
          // Clear existing items
          dropdownMenu.innerHTML = '';

          // Populate the dropdown with the subjects
          data.forEach(predmet => {
              const item = document.createElement('a');
              item.classList.add('dropdown-item');
              item.setAttribute('data-value', predmet.id);
              item.textContent = predmet.naziv; // Assuming 'naziv' is the name of the subject
              dropdownMenu.appendChild(item);
          });
      })
      .catch(error => console.error('Error fetching subjects:', error));
});
