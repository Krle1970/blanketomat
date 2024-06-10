document.addEventListener('DOMContentLoaded', function() {
    const ime = document.getElementById('name2');
    const prezime = document.getElementById('surname2');
    const email = document.getElementById('email2');
    const lozinka = document.getElementById('password2');
   
    const dugme = document.getElementById('submitS');
  
    async function registrujStudenta(name, surname, mail, pass) {
        const body = {
            Ime: name,
            Prezime: surname,
            Email: mail,
            Password: pass
            
        };
  
        const token = localStorage.getItem('token');
       
        console.log(token);
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
            else {
                alert('Uspešno ste registrovali studenta');
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
        registrujStudenta(
            ime.value, prezime.value, email.value, lozinka.value
        );
    });
  });
  