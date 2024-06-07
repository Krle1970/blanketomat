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

// Funkcija za odabir predmeta iz dropdown-a
document.getElementById('predmeti-dropdown').addEventListener('click', function(event) {
    if (event.target.classList.contains('dropdown-item')) {
        const predmet = event.target.textContent;
        const searchPredmeti = document.getElementById('search-predmeti');
        searchPredmeti.value = predmet;
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
            ${oblastName}
            <button type="button" class="btn-close float-end" aria-label="Close"></button>
        </div>
        <div class="card-body">
            <div class="col-md-12">
                <label>Odaberite podoblasti:</label>
                <div class="subareas" id="podoblasti-${oblastValue}">
                    <button class="dpodoblast btn  m-1" value="podoblast1">Unutrasnje ulancavanje</button>
                    <button class="dpodoblast btn  m-1" value="podoblast2">Dvostruki</button>
                    <button class="dpodoblast btn  m-1" value="podoblast3">Jednostruki</button>
                    <button class="dpodoblast btn  m-1" value="podoblast4">Binarni</button>
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

    const subareaButtons = card.querySelectorAll(`#podoblasti-${oblastValue} .btn`);

    subareaButtons.forEach(button => {
        button.addEventListener('click', function() {
            // If this button is already selected, re-enable all buttons
            if (this.classList.contains('selected')) {
                subareaButtons.forEach(btn => {
                    btn.disabled = false;
                    btn.classList.remove('selected');
                });
            } else {
                // Otherwise, disable all other buttons
                subareaButtons.forEach(btn => {
                    btn.disabled = btn !== this;
                    btn.classList.remove('selected');
                });
                this.classList.add('selected');
            }
        });
    });

    return card;
}
