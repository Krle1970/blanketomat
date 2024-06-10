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
        const predmet = event.target.textContent;
        const searchPredmeti = document.getElementById('search-oblasti1');
        searchPredmeti.value = predmet;
        this.classList.remove('show');
    }
});

document.addEventListener('click', function(event) {
    const searchPredmeti = document.getElementById('search-oblasti1');
    const dropdown = document.getElementById('oblasti-dropdown1');
    if (!searchPredmeti.contains(event.target) && !dropdown.contains(event.target)) {
        dropdown.classList.remove('show');
    }
});

document.addEventListener('DOMContentLoaded', function() {
    const dropdownMenuPredmeti = document.getElementById('predmeti-dropdown1');
    const dropdownMenuOblasti = document.getElementById('oblasti-dropdown1');
    const zadatakForma = document.querySelector('.zadatak-forma');
    const pitanjeForma = document.querySelector('.pitanje-forma');
    const tekst = document.querySelector('.tekst');
    const dugme = document.querySelector('.dugme');
    let selectedPredmetId = null;
    let selectedOblastId = null;

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
        const url = `http://localhost:5246/Predmet/${predmetId}/oblasti`;

        fetch(url)
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
})

async function fetchBlankets() {
    try {
        const response = await fetch('http://localhost:5246/Blanket/blanketiPrikaz');
        if (!response.ok) {
            throw new Error(`Error fetching blankets: ${response.statusText}`);
        }
        const blankets = await response.json();
        console.log(blankets); // Dodajte ovo za debugovanje
        return blankets;
    } catch (error) {
        console.error('Error:', error);
        return [];
    }
}


function provera(blanket)
{
    const checkKategorija = document.querySelectorAll('.check-kategorija');
    const checkTip = document.querySelectorAll('.check-tip');
    const searchPredmeti = document.getElementById('search-predmeti1');
    const searchOblasti = document.getElementById('search-oblasti1');
    let postojiOblast = false;

    checkKategorija.forEach(checkbox => {
        if (checkbox.checked) {
            console.log(`Checkbox with value ${checkbox.value} is selected.`);
            if (checkbox.value === blanket.kategorija) {
                console.log(`Selected value ${checkbox.value} is equal to ${blanket.kategorija}`);
            } else {
                console.log(`Selected value ${checkbox.value} is not equal to ${blanket.kategorija}`);
                return false;                     
            }
        }})
    checkTip.forEach(checkbox => {
        if (checkbox.checked) {
            console.log(`Checkbox with value ${checkbox.value} is selected.`);
            if (checkbox.value === blanket.kategorija) {
                console.log(`Selected value ${checkbox.value} is equal to ${blanket.tip}`);
            } else {
                console.log(`Selected value ${checkbox.value} is not equal to ${blanket.tip}`);  
                return false;                  
            }
        }})
    if (searchPredmeti.value === blanket.predmetNaziv) {
        console.log(`Postoji blanket sa predmetom ${searchPredmeti.value}`);
    } else {
        console.log(`Ne poostoji blanket sa predmetom ${searchPredmeti.value}`);
        return false;
    }
    console.log(`ime predmeta blanketa: ${blanket.oblasti}`);
    for (const oblast of blanket.oblasti) {
        if (searchOblasti.value === oblast) {
        console.log(`Postoji blanket sa oblascu ${searchOblasti.value}`);
        postojiOblast = true;
    }}
     if (!postojiOblast) {
        return false;
    }     
    return true;
}
async function generateHTML() 
{
    const blankets = await fetchBlankets();
    const checkKategorija = document.querySelectorAll('.check-kategorija');
    const checkTip = document.querySelectorAll('.check-tip');
    const searchPredmeti = document.getElementById('search-predmeti1');
    const searchOblasti = document.getElementById('search-oblasti1');

    for (const blanket of blankets) 
    {
        if(provera(blanket))                        
            createCard(blanket);
    }
};

function createCard(blanket) {
    const cardContainer = document.createElement('div');
    cardContainer.className = 'col-12 col-md-6 mt-3';

    const card = document.createElement('div');
    card.className = 'card custom-card';
    card.setAttribute('data-id', blanket.id); // Dodaj blanket ID kao data-id atribut
    
    card.addEventListener('click', handleCardClick); // Koristite addEventListener umesto onclick

    const cardHeader = document.createElement('div');
    cardHeader.className = 'card-header';
    cardHeader.textContent = blanket.predmetNaziv;

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body p-0';

    const rok = document.createElement('h5');
    rok.className = 'card-text rok';
    rok.textContent = blanket.ispitniRokNaziv ? `${blanket.ispitniRokNaziv} - ${blanket.ispitniRokDatum}` : 'N/A';

    const kategorija = document.createElement('h5');
    kategorija.className = 'card-text kategorija';
    kategorija.textContent = blanket.kategorija;

    const tip = document.createElement('h5');
    tip.className = 'card-text tip';
    tip.textContent = blanket.tip;

    cardBody.append(rok, kategorija, tip);

    const cardFooter = document.createElement('div');
    cardFooter.className = 'card-footer';

    const ul = document.createElement('ul');
    ul.className = 'flex-list';

    if (blanket.oblasti && blanket.oblasti.length > 0) {
        blanket.oblasti.forEach(oblast => {
            const li = document.createElement('li');
            li.textContent = `${oblast},`;
            ul.appendChild(li);
        });
    }

    cardFooter.appendChild(ul);

    card.append(cardHeader, cardBody, cardFooter);
    cardContainer.appendChild(card);

    document.getElementById('cards-container').appendChild(cardContainer);
}

async function handleCardClick(event) {
    const card = event.currentTarget;
    const blanketId = card.getAttribute('data-id');
    

    if (blanketId === undefined) {
        console.error('data-id attribute is undefined');
        return;
    }

    try {
        const response = await fetch(`http://localhost:5246/Blanket/${blanketId}/content`);
        if (!response.ok) {
            throw new Error(`Error fetching content for blanket ID ${blanketId}: ${response.statusText}`);
        }
        const content = await response.json();
        generatePDF(content);
    } catch (error) {
        console.error('Error:', error);
    }
}

function generatePDF(content) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    let yOffset = 10; // Initial Y position for the first question or task

    content.forEach((item, index) => {
        const lines = doc.splitTextToSize(`[ ${index + 1} ]: ${item.tekst}`, 170); // Set width to 170
        doc.text(lines, 10, yOffset);
        yOffset += lines.length * 10; // Adjust Y position for the next block of text
        yOffset += 6; // Add extra space between questions/tasks
    });

    doc.save('blanket.pdf');
}