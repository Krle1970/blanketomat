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

// Event listener za radio dugmad
document.querySelectorAll('input[name="task1"]').forEach(radio => {
    radio.addEventListener('change', function() {
        const isGenerisi = document.getElementById('generisi1').checked;
        const taskText = document.getElementById('task-text1');
        const taskImage = document.getElementById('task-image1');
        taskText.disabled = isGenerisi;
        taskImage.disabled = isGenerisi;
    });
});

// Inicijalno onemogućite polja za unos teksta i slike ako je "Generiši" čekirano
document.addEventListener('DOMContentLoaded', function() {
    const isGenerisi = document.getElementById('generisi1').checked;
    const taskText = document.getElementById('task-text1');
    const taskImage = document.getElementById('task-image1');
    taskText.disabled = isGenerisi;
    taskImage.disabled = isGenerisi;
});