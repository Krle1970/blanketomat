document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('name1');
    const prezime = document.getElementById('surname1');
    const email = document.getElementById('email1');
    const lozinka = document.getElementById('password1');
    const akreditacijaId = document.getElementById('akreditacijaId');
    const smerId = document.getElementById('smerId');
    const predmetiId = document.getElementById('predmetiId');
    const dugme = document.getElementById('submitProfesor');

    function registrujProfesora(name, surname, mail, pass, akrId, sId, pId) {
        const body = {
            ime: name,
            prezime: surname,
            email: mail,
            lozinka: pass,
            akreditacijaId: null,
            smerId: null,
            predmetiId: null
        };

        const token = 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzE3Njg0MDQzfQ.CCJCTIHPavU18ZvaM2pVSuGW7DywwqxKBgeoGD-fxjQP3GOt1SP4k0XXbopPtiykKvhXDh1OSZkgZR8WnshRLA';
        console.log(token);
        fetch(`https://localhost:5246/Auth/register-profesor`, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Mrežna greška ili greška servera: ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            console.log('Uspešan odgovor:', data);
        })
        .catch(error => {
            console.error('Greška u fetch pozivu:', error);
        });
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();
       
        registrujProfesora(
            ime.value, prezime.value, email.value, lozinka.value, 
            null, null,null
        );
        alert('Profesor je uspešno registrovan!');
    });
});
