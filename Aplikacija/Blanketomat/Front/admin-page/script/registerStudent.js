document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('addName');
    const prezime = document.getElementById('addSurname');
    const email = document.getElementById('addEmail');
    const lozinka = document.getElementById('addPassword');
    const akreditacijaId = document.getElementById('addAkreditacija');
    const smer = document.getElementById('addSmer');
    const predmet = document.getElementById('addPredmet');
    
    const dugme = document.getElementById('submitStudent');

    async function registrujStudenta(name, surname, mail, pass) {
        const body = {
            Ime: name,
            Prezime: surname,
            Email: mail,
            Password: pass,
            AkreditacijaId: akreditacijaId ? parseInt(akreditacijaId.value) : null,
            Smer: smer,
            Predmet: predmet
        };

        const token = localStorage.getItem('token');
        try {
            const response = await fetch('http://localhost:5246/Auth/register-student', {
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
            }

            const data = await response.json();
            console.log(data);

        } catch (error) {
            console.error('Error:', error);
            //alert('Došlo je do greške pri registraciji');
        }
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();
        registrujStudenta(
            ime.value, prezime.value, email.value, lozinka.value
        );
    });
});
