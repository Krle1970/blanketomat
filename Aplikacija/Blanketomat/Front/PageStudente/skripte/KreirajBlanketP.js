document.addEventListener('DOMContentLoaded', function() {
    const searchPredmeti = document.getElementById('search-predmeti1');
    const predmetiDropdown = document.getElementById('predmeti-dropdown1');
    const generateButton = document.getElementById('generate-tasks');

    const searchRokovi = document.getElementById('search-rokovi1');
    const ispitniRokoviDropdown = document.getElementById('rokovi-dropdown1');
   
   
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

    searchRokovi.addEventListener('input', function() {
        const filter = this.value.toLowerCase();
        const items = ispitniRokoviDropdown.getElementsByClassName('dropdown-item');
        let anyVisible = false;
        for (let i = 0; i < items.length; i++) {
            const item = items[i];
            const text = item.textContent.toLowerCase();
            item.style.display = text.includes(filter) ? '' : 'none';
            if (text.includes(filter)) {
                anyVisible = true;
            }
        }
        ispitniRokoviDropdown.classList.toggle('show', anyVisible);
    });

   
    predmetiDropdown.addEventListener('click', function(event) {
        if (event.target.classList.contains('dropdown-item')) {
            const predmet = event.target.textContent;
            searchPredmeti.value = predmet;
            this.classList.remove('show');
            generateButton.disabled = false;
            document.getElementById('generate-tasks').disabled = false; 
        }
    });

    ispitniRokoviDropdown.addEventListener('click', function(event) {
        if (event.target.classList.contains('dropdown-item')) {
            const rok = event.target.textContent;
            searchRokovi.value = rok;
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

    document.addEventListener('click', function(event) {
        if (!ispitniRokoviDropdown.contains(event.target) && event.target !== searchRokovi) {
            ispitniRokoviDropdown.classList.remove('show');
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
                item.textContent = `${predmet.naziv} (${predmet.godina})`; // Prikazuje naziv predmeta i godinu
                item.addEventListener('click', () => {
                    predmetID = predmet.id;
                    selectedPredmetNaziv = predmet.naziv;
                    selectedPredmetGodina = predmet.godina;
                    
                    console.log(`Izabran predmet ID: ${predmetID}`); 
                    console.log(`Izabran predmet: ${selectedPredmetNaziv}`);
                    console.log(`Godina: ${selectedPredmetGodina}`);
                });
                predmetiDropdown.appendChild(item);
            });
        })
        .catch(error => console.error('Error fetching subjects:', error));
        

   fetch('http://localhost:5246/PonavljanjeIspitnogRoka/PonavljanjaIspitnihRokova')
    .then(response => response.json())
    .then(data => {
        ispitniRokoviDropdown.innerHTML = '';
        data.forEach(ispitniRok => {
            const item = document.createElement('a');
            item.classList.add('dropdown-item');
            item.setAttribute('data-value', ispitniRok.id);
            item.textContent = ispitniRok.naziv;
            item.addEventListener('click', () => {
                ispitniRokID = ispitniRok.id;
                ispitniRokNaziv = ispitniRok.naziv;
                ispitniRokDatum = ispitniRok.datum;
                console.log(`Izabran ispitni rok ID: ${ispitniRokID}`);
                console.log(`Izabran ispitni rok: ${ispitniRokNaziv}`);
                console.log(`Datum ispitnog roka: ${ispitniRokDatum}`);
            });
            ispitniRokoviDropdown.appendChild(item);
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
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label for="search-podoblasti${i}">Odaberite podoblasti:</label>
                    <input type="text" id="search-podoblasti${i}" class="form-control mb-2" placeholder="Pretražite podoblasti">
                    <div class="dropdown-menu w-100" id="podoblasti-dropdown${i}">
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label for="task-text${i}">Unesite tekst pitanja:</label>
                    <textarea id="task-text${i}" class="form-control" rows="3" disabled>
                    </textarea>
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
               // const taskImage = document.getElementById(`task-image${i}`);
                taskText.disabled = isGenerisi;
                //taskImage.disabled = isGenerisi;
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
                        item.classList.add('active'); // Add active class
                        console.log(`Izabran oblast ID: ${oblastID}`);
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

        document.getElementById(`podoblasti-dropdown${i}`).addEventListener('click', function(event) {
            if (event.target.classList.contains('dropdown-item')) {
                const podoblast = event.target.textContent;
                const searchPodoblasti = document.getElementById(`search-podoblasti${i}`);
                searchPodoblasti.value = podoblast;
                this.classList.remove('show');
            }
        });

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

document.querySelector('.KreirajButton').addEventListener('click', async function() {
    console.log('KreirajBlanket button clicked');

    const kategorijaTipTesta = document.querySelector('input[name="tipTesta"]:checked').value;
    const tipBlanketa = "Usmeni";
    const numberOfTasks = document.getElementById('task-number').value;
    const promises = [];
    const questions = [];

    for (let i = 1; i <= numberOfTasks; i++) {
        const generisiRadio = document.getElementById(`generisi${i}`);
        if (generisiRadio && generisiRadio.checked) {
            const selectedOblastDropdown = document.querySelector(`#oblasti-dropdown${i} .dropdown-item.active`);
            const selectedOblastId = selectedOblastDropdown ? selectedOblastDropdown.getAttribute('data-value') : null;

            if (selectedOblastId == null) {
                alert(`Molimo vas da odaberete oblast za pitanje ${i}`);
                return;
            }

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
                        const randomIndex = Math.floor(Math.random() * data.length);
                        const randomQuestion = data[randomIndex];
                        console.log(`Random question:`, randomQuestion);
                        if (randomQuestion && randomQuestion.tekst) {
                            questions.push({ id: randomQuestion.id, tekst: randomQuestion.tekst });
                            return randomQuestion;
                        } else {
                            console.error('tekst property is missing in:', randomQuestion);
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
    }

    // Kod za kreiranje pitanja
    for (let i = 1; i <= numberOfTasks; i++) {
        const kreirajRadio = document.getElementById(`kreiraj${i}`);
        const selectedOblastDropdown = document.querySelector(`#oblasti-dropdown${i} .dropdown-item.active`);
        const selectedOblastId = selectedOblastDropdown ? selectedOblastDropdown.getAttribute('data-value') : null;

        if (kreirajRadio && kreirajRadio.checked) {
            if (!selectedOblastId) {
                alert(`Molimo vas da odaberete oblast za pitanje ${i}`);
                return;
            }

            const taskText = document.getElementById(`task-text${i}`).value;
            const selectedPodoblastId = document.querySelector(`#podoblasti-dropdown${i} .dropdown-item.active`)?.getAttribute('data-value') || null;

            console.log(`kreiraj ${i}: ${taskText}`);
            console.log(`kreiraj Oblast ID ${i}: ${selectedOblastId}`);
            console.log(`kreiraj Podoblast ID ${i}: ${selectedPodoblastId}`);

            const body = {
                Tekst: taskText,
                Oblast: selectedOblastId ? { Id: selectedOblastId, naziv: 'nazivOblasti' } : null,
                Podoblast: selectedPodoblastId ? { Id: selectedPodoblastId, naziv: 'nazivPodoblasti' } : null
            };

            const promise = fetch('http://localhost:5246/Pitanje/dodajPitanje', {
                method: 'POST',
                body: JSON.stringify(body),
                headers: {
                    'Content-Type': 'application/json',
                }
            })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(error => { throw new Error(JSON.stringify(error)); });
                }
                return response.json();
            })
            .then(data => {
                console.log(`Dodata pitanje ID: ${data.id}`);
                questions.push({ id: data.id, tekst: taskText });
                return data;
            })
            .catch(error => {
                console.error('Error adding question:', error);
                return null;
            });

            promises.push(promise);
        }
    }

    Promise.all(promises).then(() => {
        console.log('Sva pitanja:', questions);
        const ids = questions.map(q => q.id);
        console.log('Dodata pitanja ID-jevi:', ids);

        const blanketDto = {
            Tip: tipBlanketa,
            Kategorija: kategorijaTipTesta,
            Putanja: "path/to/document.pdf", // Adjust this as needed
            Predmet: {
                Id: predmetID,
                Naziv: selectedPredmetNaziv,
                Godina: selectedPredmetGodina
            },
            PonavljanjeIspitnogRoka: {
                Id: ispitniRokID,
                Naziv: ispitniRokNaziv,
                Datum: ispitniRokDatum
            },
            Pitanja: questions.map(q => ({ Id: q.id, Tekst: q.tekst }))
        };

        fetch('http://localhost:5246/Blanket/dodajBlanket', {
            method: 'POST',
            body: JSON.stringify(blanketDto),
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => { throw new Error(JSON.stringify(error)); });
            }
            return response.json();
        })
        .then(data => {
            console.log('Blanket uspešno dodat:', data);

            // Generisanje PDF-a
            const { jsPDF } = window.jspdf;
            const doc = new jsPDF();
            let yOffset = 10; // Initial Y position for the first question

            questions.forEach((question, index) => {
                const lines = doc.splitTextToSize(`Pitanje ${index + 1}: ${question.tekst}`, 170); // Set width to 170
                doc.text(lines, 10, yOffset);
                yOffset += lines.length * 10; // Adjust Y position for the next block of text
                yOffset += 6; // Add extra space between questions
            });

            doc.save('usmeni.pdf');
        })
        .catch(error => {
            console.error('Error adding blanket:', error);
        });
    });
});
