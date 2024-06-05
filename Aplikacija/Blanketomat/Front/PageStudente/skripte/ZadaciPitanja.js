document.getElementById('generate-tasks').addEventListener('click', function() {
    const numberOfTasks = document.getElementById('task-number').value;
    const tasksContainer = document.getElementById('tasks-container');
    tasksContainer.innerHTML = '';

    for (let i = 1; i <= numberOfTasks; i++) {
        const taskCard = document.createElement('div');
        taskCard.className = 'card task-card';
        taskCard.innerHTML = `
            <div class="card-body zadatak">
                <h5 class="card-title">Zadatak ${i}</h5>
            </div>
            <div class="task-details card-body unos">
                <div class="form-group mb-3">
                    <input type="radio" name="task${i}" id="generisi${i}" value="generisi">
                    <label for="generisi${i}">Generiši</label>
                    <input type="radio" name="task${i}" id="kreiraj${i}" value="kreiraj" class="ms-3">
                    <label for="kreiraj${i}">Kreiraj</label>
                </div>
                <div class="form-group mb-3">
                    <label for="search-predmeti${i}">Odaberite predmet:</label>
                    <input type="text" id="search-predmeti${i}" class="form-control mb-2" placeholder="Pretražite predmete">
                    <div class="dropdown-menu w-100" id="predmeti-dropdown${i}">
                        <a class="dropdown-item" data-value="predmet1">Matematika 1</a>
                        <a class="dropdown-item" data-value="predmet2">Fizika</a>
                        <a class="dropdown-item" data-value="predmet3">Matematika 2</a>
                    </div>
                </div>
                <div class="form-group mb-3">
                <label for="search-oblasti1" class=" no-select">Odaberite oblasti:</label>
                <input type="text" id="search-oblasti1" class="oblast form-control mb-2" placeholder="Pretražite oblasti">
                <div class="dropdown-menu w-100" id="oblasti-dropdown1">
                    <a class="dropdown-item" data-value="oblast1">Algebra</a>
                    <a class="dropdown-item" data-value="oblast2">Matrice</a>
                    <a class="dropdown-item" data-value="oblast3">Funkcije</a>
                    <a class="dropdown-item" data-value="oblast4">Analiticka</a>
                </div>
                </div>
                <div class="form-group mb-3">
                    <label for="search-podoblasti${i}">Odaberite podoblasti:</label>
                    <input type="text" id="search-podoblasti${i}" class="form-control mb-2" placeholder="Pretražite podoblasti">
                    <div class="dropdown-menu w-100" id="podoblasti-dropdown${i}">
                        <a class="dropdown-item" data-value="podoblast1">Unutrasnje ulancavanje</a>
                        <a class="dropdown-item" data-value="podoblast2">Dvostruki</a>
                        <a class="dropdown-item" data-value="podoblast3">Jednostruki</a>
                        <a class="dropdown-item" data-value="podoblast4">Binarni</a>
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label for="task-text${i}">Unesite tekst zadatka:</label>
                    <textarea id="task-text${i}" class="form-control" rows="3" disabled></textarea>
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
                }, 400); // Trajanje tranzicije u milisekundama
            } else {
                taskDetails.style.display = 'block';
                setTimeout(() => {
                    taskDetails.classList.add('show');
                }, 10); // Kratko kašnjenje da se omogući animacija
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

        // Event listener for search-predmeti
        document.getElementById(`search-predmeti${i}`).addEventListener('input', function() {
            const filter = this.value.toLowerCase();
            const dropdown = document.getElementById(`predmeti-dropdown${i}`);
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

        // Event listener for predmeti-dropdown
        document.getElementById(`predmeti-dropdown${i}`).addEventListener('click', function(event) {
            if (event.target.classList.contains('dropdown-item')) {
                const predmet = event.target.textContent;
                const searchPredmeti = document.getElementById(`search-predmeti${i}`);
                searchPredmeti.value = predmet;
                this.classList.remove('show');
            }
        });

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
        document.getElementById('search-oblasti1').addEventListener('input', function() {
            const filter = this.value.toLowerCase();
            const dropdown = document.getElementById('oblasti-dropdown1');
            const items = dropdown.getElementsByClassName('dropdown-item');
            let anyVisible = false;
            for (let i = 0; i < items.length; i++) {
                const item = items[i];
                const text = item.textContent.toLowerCase();
                item.style.display = text.includes(filter) ? '' : 'none';
                if (text.includes(filter)) {
                    anyVisible = true;
                }
            }
            dropdown.classList.toggle('show', anyVisible);
        });
        
        document.getElementById('oblasti-dropdown1').addEventListener('click', function(event) {
            if (event.target.classList.contains('dropdown-item')) {
                const oblast = event.target.textContent;
                const searchOblasti = document.getElementById('search-oblasti1');
                searchOblasti.value = oblast;
                this.classList.remove('show');
            }
        });
        
    }
});