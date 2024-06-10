document.addEventListener('DOMContentLoaded', () => {
  const numberPerPageInput = document.getElementById('numberPerPage');
  const pageNumberInput = document.getElementById('pageNumber');
  const professorList = document.getElementById('professorList');
  const fetchProfessorsBtn = document.getElementById('fetchProfessorsBtn');
  const prevPageBtn = document.getElementById('prevPageBtn');
  const nextPageBtn = document.getElementById('nextPageBtn');
  let currentPage = 1;

  async function fetchProfessors(page, count) {
      const url = `http://localhost:5246/api/Student?page=${page}&count=${count}`;
      console.log(`Fetching URL: ${url}`);
      try {
          const response = await fetch(url);
          if (!response.ok) {
              throw new Error('Failed to fetch professors');
          }

          const data = await response.json();
          return data;
      } catch (error) {
          console.error('Error:', error);
          return null;
      }
  }

  function displayProfessors(data) {
      professorList.innerHTML = '';
      if (data && data.podaci) {
          data.podaci.forEach(professor => {
              const option = document.createElement('option');
              option.textContent = `${professor.ime} ${professor.prezime}`;
              option.value = professor.id;
              professorList.appendChild(option);
          });
      } else {
          console.error('No data or podaci found:', data);
      }
  }

  async function loadProfessors() {
      const count = parseInt(numberPerPageInput.value) || 10;

      console.log(`Fetching professors with page=${currentPage} and count=${count}`);
      const data = await fetchProfessors(currentPage, count);
      displayProfessors(data);
      pageNumberInput.value = currentPage; // Update the page number input with the current page
  }

  fetchProfessorsBtn.addEventListener('click', loadProfessors);

  prevPageBtn.addEventListener('click', () => {
      if (currentPage > 1) {
          currentPage--;
          loadProfessors();
      }
  });

  nextPageBtn.addEventListener('click', () => {
      currentPage++;
      loadProfessors();
  });
});
