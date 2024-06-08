document.getElementById('search-predmeti1').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const dropdown = document.getElementById('predmeti-dropdown1');
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

document.getElementById('predmeti-dropdown1').addEventListener('click', function(event) {
    if (event.target.classList.contains('dropdown-item')) {
        const predmet = event.target.textContent;
        const searchPredmeti = document.getElementById('search-predmeti1');
        searchPredmeti.value = predmet;
        this.classList.remove('show');
    }
});

document.addEventListener('click', function(event) {
    const searchPredmeti = document.getElementById('search-predmeti1');
    const dropdown = document.getElementById('predmeti-dropdown1');
    if (!searchPredmeti.contains(event.target) && !dropdown.contains(event.target)) {
        dropdown.classList.remove('show');
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

document.getElementById('search-podoblasti1').addEventListener('input', function() {
    const filter = this.value.toLowerCase();
    const dropdown = document.getElementById('podoblasti-dropdown1');
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

document.getElementById('podoblasti-dropdown1').addEventListener('click', function(event) {
    if (event.target.classList.contains('dropdown-item')) {
        const podoblast = event.target.textContent;
        const searchPodoblasti = document.getElementById('search-podoblasti1');
        searchPodoblasti.value = podoblast;
        this.classList.remove('show');
    }
});

document.addEventListener('click', function(event) {
    const searchOblasti = document.getElementById('search-oblasti1');
    const dropdownOblasti = document.getElementById('oblasti-dropdown1');
    const searchPodoblasti = document.getElementById('search-podoblasti1');
    const dropdownPodoblasti = document.getElementById('podoblasti-dropdown1');
    if (!searchOblasti.contains(event.target) && !dropdownOblasti.contains(event.target)) {
        dropdownOblasti.classList.remove('show');
    }
    if (!searchPodoblasti.contains(event.target) && !dropdownPodoblasti.contains(event.target)) {
        dropdownPodoblasti.classList.remove('show');
    }
});
