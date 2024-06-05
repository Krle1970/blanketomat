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

// Funkcija za pretragu oblasti
document.getElementById('search-oblasti').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const options = document.getElementById('oblasti').options;
    for (let i = 0; i < options.length; i++) {
        const option = options[i];
        const text = option.text.toLowerCase();
        option.style.display = text.includes(filter) ? '' : 'none';
    }
});

// Funkcija za dodavanje odabranih oblasti u listu
document.getElementById('oblasti').addEventListener('change', function() {
    const selectedOptions = Array.from(this.selectedOptions);
    const selectedOblasti = document.getElementById('selected-oblasti');
    selectedOptions.forEach(option => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = option.text;
        li.dataset.value = option.value;
        selectedOblasti.appendChild(li);
        option.disabled = true; // Onemogućiti izbor oblasti koje su već dodane
    });
    this.value = ''; // Resetovati padajući meni
});

// Funkcija za uklanjanje oblasti sa liste
document.getElementById('selected-oblasti').addEventListener('click', function(event) {
    if (event.target.tagName === 'LI') {
        const value = event.target.dataset.value;
        document.querySelector(`#oblasti option[value="${value}"]`).disabled = false; // Ponovno omogućiti izbor
        event.target.remove();
    }
});
// Funkcija za pretragu oblasti
document.getElementById('search-podoblasti').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const options = document.getElementById('podoblasti').options;
    for (let i = 0; i < options.length; i++) {
        const option = options[i];
        const text = option.text.toLowerCase();
        option.style.display = text.includes(filter) ? '' : 'none';
    }
});

document.getElementById('podoblasti').addEventListener('change', function() {
    const selectedOptions = Array.from(this.selectedOptions);
    const selectedOblasti = document.getElementById('selected-podoblasti');
    selectedOptions.forEach(option => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = option.text;
        li.dataset.value = option.value;
        selectedOblasti.appendChild(li);
        option.disabled = true; // Onemogućiti izbor oblasti koje su već dodane
    });
    this.value = ''; // Resetovati padajući meni
});


// Funkcija za uklanjanje oblasti sa liste
document.getElementById('selected-podoblasti').addEventListener('click', function(event) {
    if (event.target.tagName === 'LI') {
        const value = event.target.dataset.value;
        document.querySelector(`#podoblasti option[value="${value}"]`).disabled = false; // Ponovno omogućiti izbor
        event.target.remove();
    }
});