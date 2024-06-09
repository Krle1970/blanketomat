document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('name1');
    const prezime = document.getElementById('surname1');
    const email = document.getElementById('email1');
    const lozinka = document.getElementById('password1');
    const katedraId = document.getElementById('katedraId1');
    const smerId = document.getElementById('smerId1');
    const predmetiIds = document.getElementById('predmetiIds1');
    const dugme = document.getElementById('submitProfesor');

    async function fetchData(url) {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        const data = await response.json();
        console.log('Fetched data:', data);
        return data;
    }

    async function populateSelect(selectElement, url, valueField, textField) {
        try {
            selectElement.innerHTML = '';
            const data = await fetchData(url);
            if (!Array.isArray(data)) {
                console.error('Expected array but got:', data);
                throw new Error('Invalid data format');
            }
            data.forEach(item => {
                const option = document.createElement('option');
                //console.log('Item:', item);
                option.value = item[valueField];
                option.text = item[textField];
                selectElement.appendChild(option);
            });
        } catch (error) {
            console.error(`Error populating select for ${url}:`, error);
        }
    }

    async function initialize() {
        try {
            await populateSelect(katedraId, 'http://localhost:5246/Katedra/katedre', 'id', 'naziv');
            await populateSelect(smerId, 'http://localhost:5246/Smer/smerovi', 'id', 'naziv');
            await populateSelect(predmetiIds, 'http://localhost:5246/Predmet/predmeti', 'id', 'naziv');
        } catch (error) {
            console.error('Error:', error);
            alert('Došlo je do greške pri učitavanju podataka');
        }
    }

    async function registrujProfesora(name, surname, mail, pass, katedra, smer, predmeti) {
        const body = {
            Ime: name,
            Prezime: surname,
            Email: mail,
            Password: pass,
            KatedraId: katedra,
            SmerId: smer,
            PredmetiIds: predmeti
        };

        console.log('Body:', body);

        const token = localStorage.getItem('token');

        try {
            const response = await fetch('http://localhost:5246/Auth/register-profesor', {
                method: 'POST',
                body: JSON.stringify(body),
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });

            console.log(response);

            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            } else {
                alert('Uspešno ste registrovali profesora');
                const responseText = await response.text();
                console.log('Response text:', responseText);

                const data = responseText ? JSON.parse(responseText) : {};
                console.log(data);
            }

        } catch (error) {
            console.error('Error:', error);
            alert('Došlo je do greške pri registraciji');
        }
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();
        const selectedKatedra = katedraId.value ? parseInt(katedraId.value) : null;
        const selectedSmer = smerId.value ? parseInt(smerId.value) : null;
        const selectedPredmeti = Array.from(predmetiIds.selectedOptions).map(option => parseInt(option.value));
        registrujProfesora(
            ime.value, prezime.value, email.value, lozinka.value, selectedKatedra, selectedSmer, selectedPredmeti
        );
    });

    initialize();
});
