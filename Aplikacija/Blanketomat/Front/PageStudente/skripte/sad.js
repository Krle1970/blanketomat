document.addEventListener('DOMContentLoaded', function() {
    const oblast = document.getElementByClassName('oblast');
    const tekst = document.getElementByClassName('tekst');
    const dugme = document.getElementByClassName('dugme');

    function DodajZadtak(tekst, oblast) {
        const body = {
           Tekst:tekst,
           Oblast:oblast
        };

       
        fetch(`https://localhost:5246/Auth/register-profesor`, {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json',
            }
        })
        .then(data=>data.json())
        .then(response=consol.log(response));
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();
       
        DodajZadtak(tekst, oblast);
        alert('zadatak je uspešno registrovan!');
    });
});
