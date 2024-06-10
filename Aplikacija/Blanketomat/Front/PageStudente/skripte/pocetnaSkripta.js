async function fetchBlankets() {
    try {
        const response = await fetch('http://localhost:5246/Blanket/blanketiPrikaz');
        if (!response.ok) {
            throw new Error(`Error fetching blankets: ${response.statusText}`);
        }
        const blankets = await response.json();
        console.log(blankets);
        return blankets;
    } catch (error) {
        console.error('Error:', error);
        return [];
    }
}

async function generateHTML() {
    const blankets = await fetchBlankets();

    for (const blanket of blankets) {
        createCard(blanket);
    }
}

function createCard(blanket) {
    const cardContainer = document.createElement('div');
    cardContainer.className = 'col-12 col-md-6 mt-3';

    const card = document.createElement('div');
    card.className = 'card custom-card';
    card.setAttribute('data-id', blanket.id);
    
    card.addEventListener('click', handleCardClick); 

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

// Pozovite generateHTML da generiše kartice
generateHTML();
