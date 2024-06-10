document.addEventListener('DOMContentLoaded', function() {
  const numberOfTasks = document.getElementById('task-number').value;
  const dropdownMenuPredmeti = document.getElementById('predmeti-dropdown1');
  const dropdownMenuOblasti = document.getElementById('oblasti-dropdown1');
  const dropdownMenuPodoblasti = document.getElementById('podoblasti-dropdown1');

  
 
  fetch('http://localhost:5246/Predmet/predmeti')
      .then(response => response.json())
      .then(data => {
          dropdownMenuPredmeti.innerHTML = '';

          data.forEach(predmet => {
              const item = document.createElement('a');
              item.classList.add('dropdown-item');
              item.setAttribute('data-value', predmet.id);
              item.textContent = predmet.naziv;
              dropdownMenuPredmeti.appendChild(item);

              item.addEventListener('click', function() {
                  selectedPredmetId = predmet.id;
                  console.log('Izabrani predmet ID:', selectedPredmetId);
                  fetchOblastiForPredmet(selectedPredmetId);
              });
          });
      })
      .catch(error => console.error('Error fetching subjects:', error));

  function fetchOblastiForPredmet(predmetId) {
      

      fetch(`http://localhost:5246/Predmet/${predmetId}/oblasti`)
          .then(response => response.json())
          .then(data => {
              console.log('Oblasti za izabrani predmet:', data);
              
              dropdownMenuOblasti.innerHTML = '';
              
              data.forEach(oblast => {
                  const item = document.createElement('a');
                  item.classList.add('dropdown-item');
                  item.setAttribute('data-value', oblast.id);
                  item.textContent = oblast.naziv;
                  dropdownMenuOblasti.appendChild(item);

                  item.addEventListener('click', function() {
                      selectedOblastId = oblast.id;
                      console.log('Izabrana oblast ID:', oblast.id);
                      fetchPodoblastiForOblast(selectedOblastId);
                  });
              });

              if (dropdownMenuOblasti.childElementCount > 0) {
                  dropdownMenuOblasti.classList.add('show');
              } else {
                  dropdownMenuOblasti.classList.remove('show');
              }
          })
          .catch(error => console.error('Error fetching oblasti:', error));
  }

  function fetchPodoblastiForOblast(oblastId) {
    

      fetch(`http://localhost:5246/Oblast/${oblastId}/podoblasti`)
          .then(response => response.json())
          .then(data => {
              console.log('Podoblasti za izabranu oblast:', data);
              
              dropdownMenuPodoblasti.innerHTML = '';

              data.forEach(podoblast => {
                  const item = document.createElement('a');
                  item.classList.add('dropdown-item');
                  item.setAttribute('data-value', podoblast.id);
                  item.textContent = podoblast.naziv;
                  dropdownMenuPodoblasti.appendChild(item);

                  item.addEventListener('click', function() {
                      selectedPodoblastId = podoblast.id;
                      console.log('Izabrana podoblast ID:', podoblast.id);
                  });
              });

              if (dropdownMenuPodoblasti.childElementCount > 0) {
                  dropdownMenuPodoblasti.classList.add('show');
              } else {
                  dropdownMenuPodoblasti.classList.remove('show');
              }
          })
          .catch(error => console.error('Error fetching podoblasti:', error));
  }

});

document.querySelector('.dugme').addEventListener('click', async function() {
  
  const numberOfTasks = document.getElementById('task-number').value;
  const promises = [];
  const questions = new Set(); // Use a Set to track unique questions
  const questionsArray = []; // Use an array to store the unique questions

  for (let i = 1; i <= numberOfTasks; i++) {
     console.log('Izabrana oblast ID:', selectedOblastId);
      const url = `http://localhost:5246/Pitanje/Oblast/${selectedOblastId}`;
      const promise = fetch(url)
          .then(response => {
              if (!response.ok) {
                  return response.json().then(error => { throw new Error(JSON.stringify(error)); });
              }
              return response.json();
          })
          .then(data => {
              console.log(`Data for URL ${url}:`, data);

              if (data.length > 0) {
                  let randomQuestion = null;
                  let attempt = 0;
                  const maxAttempts = 10; // Prevent infinite loop

                  // Ensure unique question
                  while (attempt < maxAttempts) {
                      const randomIndex = Math.floor(Math.random() * data.length);
                      randomQuestion = data[randomIndex];
                      if (randomQuestion && randomQuestion.tekst && !questions.has(randomQuestion.id)) {
                          questions.add(randomQuestion.id); // Track unique question by id
                          questionsArray.push({ id: randomQuestion.id, tekst: randomQuestion.tekst });
                          return { id: randomQuestion.id, tekst: randomQuestion.tekst };
                      }
                      attempt++;
                  }

                  if (attempt === maxAttempts) {
                      console.error('Unable to find a unique question after multiple attempts');
                      return null;
                  }
              } else {
                  return null;
              }
          })
          .catch(error => {
              console.error('Error fetching questions:', error);
              return null;
          });

      promises.push(promise);
  }

  Promise.all(promises).then(() => {
      const uniqueQuestions = questionsArray.filter(question => question !== null); // Filter out null values

      if (uniqueQuestions.length < numberOfTasks) {
          alert(`Only ${uniqueQuestions.length} unique questions found out of the requested ${numberOfTasks}.`);
      }

      // Generate PDF
      const { jsPDF } = window.jspdf;
      const doc = new jsPDF();
      let yOffset = 10; // Initial Y position for the first question

      uniqueQuestions.forEach((question, index) => {
          const lines = doc.splitTextToSize(`Pitanje ${index + 1}: ${question.tekst}`, 170); // Set width to 170
          doc.text(lines, 10, yOffset);
          yOffset += lines.length * 10; // Adjust Y position for the next block of text
          yOffset += 6; // Add extra space between questions
      });

      doc.save('teorija_vezbanje.pdf');
  });
});
