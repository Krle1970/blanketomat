document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('name1');
    const prezime = document.getElementById('surname1');
    const email = document.getElementById('email1');
    const lozinka = document.getElementById('password1');
    const akreditacijaId = document.getElementById('akreditacijaId1');
    const smerId = document.getElementById('smerId1');
    const predmetiIds = document.getElementById('predmetiIds1');
    const dugme = document.getElementById('submitProfesor');

    async function registrujProfesora(name, surname, mail, pass) {
        const body = {
            Ime: name,
            Prezime: surname,
            Email: mail,
            Password: pass,
            AkreditacijaId: akreditacijaId ? parseInt(akreditacijaId.value) : null,
            SmerId: smerId ? parseInt(smerId.value) : null,
            PredmetiIds: predmetiIds.value.split(',').map(id => parseInt(id.trim()))
        };

        const token = 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNzE3Njk2NjAzfQ.ltK8C5EjwQl2dygYH_xiwuposDaHvgq2mif_xYbW4dmXiXcf2DOH3AFQDog1wl3gPPjDsvw9mNjlH4VBKUAHSQ'; 
        console.log(token);

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
            }

            const data = await response.json();
            console.log(data);

        } catch (error) {
            console.error('Error:', error);
            alert('Došlo je do greške pri registraciji');
        }
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();
        registrujProfesora(
            ime.value, prezime.value, email.value, lozinka.value
        );
    });
});
