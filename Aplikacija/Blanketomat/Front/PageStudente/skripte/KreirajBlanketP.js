
document.addEventListener('DOMContentLoaded', function() {
    const searchPredmeti = document.getElementById('search-predmeti1');
    const predmetiDropdown = document.getElementById('predmeti-dropdown1');
    const generateButton = document.getElementById('generate-tasks');
   
    // Event listener for the search input
    searchPredmeti.addEventListener('input', function() {
        const filter = this.value.toLowerCase();
        const items = predmetiDropdown.getElementsByClassName('dropdown-item');
        let anyVisible = false;
        for (let i = 0; i < items.length; i++) {
            const item = items[i];
            const text = item.textContent.toLowerCase();
            item.style.display = text.includes(filter) ? '' : 'none';
            if (text.includes(filter)) {
                anyVisible = true;
            }
        }
        predmetiDropdown.classList.toggle('show', anyVisible);
    });

    // Event listener for the dropdown items
    predmetiDropdown.addEventListener('click', function(event) {
        if (event.target.classList.contains('dropdown-item')) {
            const predmet = event.target.textContent;
            searchPredmeti.value = predmet;
            this.classList.remove('show');
            generateButton.disabled = false;
            document.getElementById('generate-tasks').disabled = false; 
        }
    });


    document.addEventListener('click', function(event) {
        if (!predmetiDropdown.contains(event.target) && event.target !== searchPredmeti) {
            predmetiDropdown.classList.remove('show');
        }
    });

   
    fetch('http://localhost:5246/Predmet/predmeti') 
        .then(response => response.json())
        .then(data => {
            predmetiDropdown.innerHTML = '';
            data.forEach(predmet => {
                const item = document.createElement('a');
                item.classList.add('dropdown-item');
                item.setAttribute('data-value', predmet.id);
                item.textContent = predmet.naziv;
                item.addEventListener('click', () => {
                    predmetID = predmet.id; 
                    console.log(`Izabran predmet ID: ${predmetID}`); 
                });
                predmetiDropdown.appendChild(item);
            });
        })
        .catch(error => console.error('Error fetching subjects:', error));
        
});
  


    

document.getElementById('generate-tasks').addEventListener('click', function() {

    console.log(`Izabran predmet ID: ${predmetID}`)
    let oblastID;
 
    const numberOfTasks = document.getElementById('task-number').value;
    const tasksContainer = document.getElementById('tasks-container');
    tasksContainer.innerHTML = '';
    
    
    for (let i = 1; i <= numberOfTasks; i++) {
        const taskCard = document.createElement('div');
        taskCard.className = 'card task-card';
        taskCard.innerHTML = `
            <div class="card-body">
                <h5 class="card-title">Pitanje ${i}</h5>
            </div>
            <div class="task-details card-body unos">
                <div class="form-group mb-3">
                    <input type="radio" name="task${i}" id="generisi${i}" value="generisi" checked>
                    <label for="generisi${i}">Generiši</label>
                    <input type="radio" name="task${i}" id="kreiraj${i}" value="kreiraj" class="ms-3">
                    <label for="kreiraj${i}">Kreiraj</label>
                </div>
                
                <div class="form-group mb-3">
                    <label for="search-oblasti${i}">Odaberite oblasti:</label>
                    <input type="text" id="search-oblasti${i}" class="oblast form-control mb-2" placeholder="Pretražite oblasti">
                    <div class="dropdown-menu w-100" id="oblasti-dropdown${i}">
                    <a class="dropdown-item" data-value="oblast1">Algebra</a>
                    <a class="dropdown-item" data-value="oblast2">Matrice</a>
                    <a class="dropdown-item" data-value="oblast3">Funkcije</a>
                    <a class="dropdown-item" data-value="oblast4">Analiticka</a></div>
                </div>
                <div class="form-group mb-3">
                    <label for="search-podoblasti${i}">Odaberite podoblasti:</label>
                    <input type="text" id="search-podoblasti${i}" class="form-control mb-2" placeholder="Pretražite podoblasti">
                    <div class="dropdown-menu w-100" id="podoblasti-dropdown${i}">
                    <a class="dropdown-item" data-value="podoblast1">Unutrasnje ulancavanje</a>
                    <a class="dropdown-item" data-value="podoblast2">Dvostruki</a>
                    <a class="dropdown-item" data-value="podoblast3">Jednostruki</a>
                    <a class="dropdown-item" data-value="podoblast4">Binarni</a></div>
                </div>
                <div class="form-group mb-3">
                    <label for="task-text${i}">Unesite tekst pitanja:</label>
                    <textarea id="task-text${i}" class="form-control" rows="3" disabled>
                    </textarea>
                </div>
                <div class="form-group mb-3">
                    <label for="task-image${i}">Dodajte sliku:</label>
                    <input type="file" id="task-image${i}" class="form-control" disabled>
                </div>
            </div>
        `;
        tasksContainer.appendChild(taskCard);

        taskCard.querySelector('.card-body').addEventListener('click', function() {
            const taskDetails = taskCard.querySelector('.task-details');
            if (taskDetails.classList.contains('show')) {
                taskDetails.classList.remove('show');
                setTimeout(() => {
                    taskDetails.style.display = 'none';
                }, 400); 
            } else {
                taskDetails.style.display = 'block';
                setTimeout(() => {
                    taskDetails.classList.add('show');
                }, 10); 
            }
        });

        taskCard.querySelectorAll(`input[name="task${i}"]`).forEach(radio => {
            radio.addEventListener('change', function() {
                const isGenerisi = document.getElementById(`generisi${i}`).checked;
                const taskText = document.getElementById(`task-text${i}`);
                const taskImage = document.getElementById(`task-image${i}`);
                taskText.disabled = isGenerisi;
                taskImage.disabled = isGenerisi;
            });
        });

      
        fetch(`http://localhost:5246/Predmet/${predmetID}/oblasti`)
            .then(response => response.json())
            .then(data => {
                const dropdownMenu = document.getElementById(`oblasti-dropdown${i}`);
                dropdownMenu.innerHTML = '';
                data.forEach(oblast => {
                    const item = document.createElement('a');
                    item.classList.add('dropdown-item');
                    item.setAttribute('data-value', oblast.id);
                    item.textContent = oblast.naziv;
                    item.addEventListener('click', () => {
                        oblastID = oblast.id;
                        console.log(`Izabran oblast ID: ${oblastID}`);
                        // Fetch podoblasti based on selected oblastID
                        fetchPodoblasti(i, oblastID);
                    });
                    dropdownMenu.appendChild(item);
                });
            })
            .catch(error => console.error('Error fetching oblasti:', error));
           

        
            function fetchPodoblasti(taskIndex, oblastID) {
                fetch(`http://localhost:5246/Oblast/${oblastID}/podoblasti`)
                    .then(response => response.json())
                    .then(data => {
                        const dropdownMenu = document.getElementById(`podoblasti-dropdown${taskIndex}`);
                        dropdownMenu.innerHTML = '';
                        data.forEach(podoblast => {
                            const item = document.createElement('a');
                            item.classList.add('dropdown-item');
                            item.setAttribute('data-value', podoblast.id);
                            item.textContent = podoblast.naziv;
                            dropdownMenu.appendChild(item);
                        });
                    })
                    .catch(error => console.error('Error fetching podoblasti:', error));
            }

          
            

        // Event listener for search-podoblasti
        document.getElementById(`search-podoblasti${i}`).addEventListener('input', function() {
            const filter = this.value.toLowerCase();
            const dropdown = document.getElementById(`podoblasti-dropdown${i}`);
            const items = dropdown.getElementsByClassName('dropdown-item');
            let anyVisible = false;
            for (let j = 0; j < items.length; j++) {
                const item = items[j];
                const text = item.textContent.toLowerCase();
                item.style.display = text.includes(filter) ? '' : 'none';
                if (text.includes(filter)) {
                    anyVisible = true;
                }
            }
            dropdown.classList.toggle('show', anyVisible);
        });

        // Event listener for podoblasti-dropdown
        document.getElementById(`podoblasti-dropdown${i}`).addEventListener('click', function(event) {
            if (event.target.classList.contains('dropdown-item')) {
                const podoblast = event.target.textContent;
                const searchPodoblasti = document.getElementById(`search-podoblasti${i}`);
                searchPodoblasti.value = podoblast;
                this.classList.remove('show');
            }
        });

        // Event listener for search-oblasti
        document.getElementById(`search-oblasti${i}`).addEventListener('input', function() {
            const filter = this.value.toLowerCase();
            const dropdown = document.getElementById(`oblasti-dropdown${i}`);
            const items = dropdown.getElementsByClassName('dropdown-item');
            let anyVisible = false;
            for (let j = 0; j < items.length; j++) {
                const item = items[j];
                const text = item.textContent.toLowerCase();
                item.style.display = text.includes(filter) ? '' : 'none';
                if (text.includes(filter)) {
                    anyVisible = true;
                }
            }
            dropdown.classList.toggle('show', anyVisible);
        });

        // Event listener for oblasti-dropdown
        
        document.getElementById(`oblasti-dropdown${i}`).addEventListener('click', function(event) {
            if (event.target.classList.contains('dropdown-item')) {
                const podoblast = event.target.textContent;
                const searchPodoblasti = document.getElementById(`search-oblasti${i}`);
                searchPodoblasti.value = podoblast;
                this.classList.remove('show');
            }
        });
        document.addEventListener('click', function(event) {
            for (let i = 1; i <= numberOfTasks; i++) {
                
                const searchOblasti = document.getElementById(`search-oblasti${i}`);
                const dropdownOblasti = document.getElementById(`oblasti-dropdown${i}`);
                const searchPodoblasti = document.getElementById(`search-podoblasti${i}`);
                const dropdownPodoblasti = document.getElementById(`podoblasti-dropdown${i}`);
    
                
    
                if (searchOblasti && dropdownOblasti && !searchOblasti.contains(event.target) && !dropdownOblasti.contains(event.target)) {
                    dropdownOblasti.classList.remove('show');
                }
    
                if (searchPodoblasti && dropdownPodoblasti && !searchPodoblasti.contains(event.target) && !dropdownPodoblasti.contains(event.target)) {
                    dropdownPodoblasti.classList.remove('show');
                }
            }
        });
        
        
    }

});
document.querySelector('.KreirajButton').addEventListener('click', function() {
    console.log('KreirajBlanket button clicked');
    const numberOfTasks = document.getElementById('task-number').value;
    console.log(`Broj pitanja: ${numberOfTasks}`);

    for (let i = 1; i <= numberOfTasks; i++) {
        const kreirajRadio = document.getElementById(`kreiraj${i}`);
        if (kreirajRadio && kreirajRadio.checked) {
            const taskText = document.getElementById(`task-text${i}`).value;
            console.log(`Pitanje ${i}: ${taskText}`);
        }
    }
});


