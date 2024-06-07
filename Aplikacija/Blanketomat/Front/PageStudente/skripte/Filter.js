document.getElementById('search-predmeti').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const dropdown = document.getElementById('predmeti-dropdown');
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
    dropdown.style.display = anyVisible ? 'block' : 'none';
});

document.getElementById('predmeti-dropdown').addEventListener('click', function(event) {
    if (event.target.classList.contains('dropdown-item')) {
        const predmet = event.target.textContent;
        const searchPredmeti = document.getElementById('search-predmeti');
        searchPredmeti.value = predmet;
        this.style.display = 'none';
    }
});

document.getElementById('search-rok').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const dropdown = document.getElementById('rok-dropdown');
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
    dropdown.style.display = anyVisible ? 'block' : 'none';
});

document.getElementById('rok-dropdown').addEventListener('click', function(event) {
    if (event.target.classList.contains('dropdown-item')) {
        const rok = event.target.textContent;
        const searchRok = document.getElementById('search-rok');
        searchRok.value = rok;
        this.style.display = 'none';
    }
});

document.getElementById('search-oblasti').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const options = document.getElementById('oblasti').options;
    for (let i = 0; i < options.length; i++) {
        const option = options[i];
        const text = option.text.toLowerCase();
        option.style.display = text.includes(filter) ? '' : 'none';
    }
});

document.getElementById('oblasti').addEventListener('change', function() {
    const selectedOptions = Array.from(this.selectedOptions);
    const selectedOblastiContainer = document.getElementById('selected-oblasti-container');
    selectedOptions.forEach(option => {
        const card = createOblastCard(option.text, option.value);
        selectedOblastiContainer.appendChild(card);
        option.style.display = 'none'; // Hide the selected option
    });
    this.value = ''; // Reset the select element
});

function createOblastCard(oblastName, oblastValue) {
    const card = document.createElement('div');
    card.className = 'card mt-3';
    card.dataset.oblastValue = oblastValue;
    card.innerHTML = `
        <div class="card-header">
          <span class="kartica">${oblastName}</span>
            <button type="button" class="btn-close float-end" aria-label="Close"></button>
        </div>
        <div class="card-body ">
            <div class="row redd">
                <div class="col-md-6">
                    <label for="search-podoblasti-${oblastValue}">Odaberite podoblasti:</label>
                    <input type="text" id="search-podoblasti-${oblastValue}" class="form-control mb-2" placeholder="Pretražite podoblasti">
                    <select multiple class="form-control podoblasti-select" id="podoblasti-${oblastValue}" style="color: rgb(87, 87, 87) !important;">
                        <option value="podoblast1">Unutrasnje ulancavanje</option>
                        <option value="podoblast2">Dvostruki</option>
                        <option value="podoblast3">Jednostruki</option>
                        <option value="podoblast4">Binarni</option>
                    </select>
                </div>
                <div class="col-md-6">
                    <label>Odabrane podoblasti:</label>
                    <ul class="list-group mt-2 selected-podoblasti" id="selected-podoblasti-${oblastValue}">
                        <!-- Odabrane podoblasti će biti dodane ovde -->
                    </ul>
                </div>
            </div>
        </div>
    `;

    card.querySelector('.btn-close').addEventListener('click', function() {
        const oblastSelect = document.getElementById('oblasti');
        const option = oblastSelect.querySelector(`option[value="${oblastValue}"]`);
        option.style.display = 'block'; // Show the option again
        card.remove();
    });

    const searchPodoblasti = card.querySelector(`#search-podoblasti-${oblastValue}`);
    searchPodoblasti.addEventListener('input', function() {
        const filter = this.value.toLowerCase();
        const options = card.querySelector(`#podoblasti-${oblastValue}`).options;
        for (let i = 0; i < options.length; i++) {
            const option = options[i];
            const text = option.text.toLowerCase();
            option.style.display = text.includes(filter) ? '' : 'none';
        }
    });

    const podoblastiSelect = card.querySelector(`#podoblasti-${oblastValue}`);
    podoblastiSelect.addEventListener('change', function() {
        const selectedOptions = Array.from(this.selectedOptions);
        const selectedPodoblasti = card.querySelector(`#selected-podoblasti-${oblastValue}`);
        selectedOptions.forEach(option => {
            const li = document.createElement('li');
            li.className = 'list-group-item';
            li.textContent = option.text;
            li.dataset.value = option.value;
            selectedPodoblasti.appendChild(li);
            option.style.display = 'none'; // Hide the selected option
        });
        this.value = ''; // Reset the select element
    });

    const selectedPodoblastiList = card.querySelector(`#selected-podoblasti-${oblastValue}`);
    selectedPodoblastiList.addEventListener('click', function(event) {
        if (event.target.tagName === 'LI') {
            const value = event.target.dataset.value;
            card.querySelector(`#podoblasti-${oblastValue} option[value="${value}"]`).style.display = 'block'; // Show the option again
            event.target.remove();
        }
    });

    return card;
}
