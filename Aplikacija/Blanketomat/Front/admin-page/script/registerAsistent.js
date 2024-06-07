document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('name1');
    const prezime = document.getElementById('surname1');
    const email = document.getElementById('email1');
    const lozinka = document.getElementById('password1');
    const akreditacijaId = document.getElementById('akreditacijaId1');
    const smerId = document.getElementById('smerId1');
    const predmetiIds = document.getElementById('predmetiIds1');
    const dugme = document.getElementById('submitAsistent');

    async function registrujAsistenta(name, surname, mail, pass) {
        const body = {
            Ime: name,
            Prezime: surname,
            Email: mail,
            Password: pass,
            AkreditacijaId: akreditacijaId ? parseInt(akreditacijaId.value) : null,
            SmerId: smerId ? parseInt(smerId.value) : null,
            PredmetiIds: predmetiIds.value.split(',').map(id => parseInt(id.trim()))
        };

        const token = localStorage.getItem('token');
     
        try {
            const response = await fetch('http://localhost:5246/Auth/register-asistent', {
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
            else {
                alert('Uspešno ste registrovali asistenta');
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
        registrujAsistenta(
            ime.value, prezime.value, email.value, lozinka.value
        );
    });
});