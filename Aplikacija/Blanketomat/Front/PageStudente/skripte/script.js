var toggleBtn = document.querySelector('.toggle-btn');

toggleBtn.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");

    
    var icon = toggleBtn.querySelector('i');

    
    if (icon.classList.contains('lni-angle-double-right')) {
        
        icon.classList.remove('lni-angle-double-right');
        icon.classList.add('lni-angle-double-left');
    } else {
        
        icon.classList.remove('lni-angle-double-left');
        icon.classList.add('lni-angle-double-right');
    }
});

 document.querySelector('.logDugme').addEventListener('click', () => {
 window.location.href = '../Login/LoginPage.html';
   });


   async function fetchBlankets() {
    try {
        const response = await fetch('http://localhost:5246/Blanket/blanketi');
        if (!response.ok) {
            throw new Error(`Error fetching blankets: ${response.statusText}`);
        }
        const blankets = await response.json();
        return blankets;
    } catch (error) {
        console.error('Error:', error);
        return [];
    }
}

async function fetchPredmetForBlanket(blanketId) {
    try {
        const response = await fetch(`http://localhost:5246/Blanket/${blanketId}/predmet`);
        if (!response.ok) {
            throw new Error(`Error fetching predmet for blanket ${blanketId}: ${response.statusText}`);
        }
        const predmet = await response.json();
        return predmet;
    } catch (error) {
        console.error('Error:', error);
        return null;
    }
}

async function fetchOblastiForPredmet(predmetId) {
    try {
        const response = await fetch(`http://localhost:5246/Predmet/${predmetId}/oblasti`);
        if (!response.ok) {
            throw new Error(`Error fetching oblasti for predmet ${predmetId}: ${response.statusText}`);
        }
        const oblasti = await response.json();
        return oblasti;
    } catch (error) {
        console.error('Error:', error);
        return [];
    }
}

async function generateHTML() {
    const blankets = await fetchBlankets();

    for (const blanket of blankets) {
        const predmet = await fetchPredmetForBlanket(blanket.id);

        if (predmet) {
            const oblasti = await fetchOblastiForPredmet(predmet.id);
            createCard(blanket, predmet, oblasti);
        }
    }
}

function createCard(blanket, predmet, oblasti) {
    const cardContainer = document.createElement('div');
    cardContainer.className = 'col-12 col-md-6 mt-3';

    const card = document.createElement('div');
    card.className = 'card custom-card';
    card.onclick = handleCardClick;

    const cardHeader = document.createElement('div');
    cardHeader.className = 'card-header';
    cardHeader.textContent = predmet.naziv;

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body p-0';

    const rok = document.createElement('h5');
    rok.className = 'card-text rok';
    rok.textContent = 'Jun 2024';

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

    oblasti.forEach(oblast => {
        const li = document.createElement('li');
        li.textContent = `${oblast.naziv},`;
        ul.appendChild(li);
    });

    cardFooter.appendChild(ul);

    card.append(cardHeader, cardBody, cardFooter);
    cardContainer.appendChild(card);

    document.getElementById('cards-container').appendChild(cardContainer);
}

document.addEventListener('DOMContentLoaded', generateHTML);