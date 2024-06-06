document.addEventListener('DOMContentLoaded', function() {
 
    const tekst = document.querySelector('.tekst');
    const dugme = document.querySelector('.dugme');

    function DodajZadtak(tekst) {
        const body = {
           Tekst:tekst
           
        };


        fetch(`http://localhost:5246/Zadatak` , {
            method: 'POST',
            body: JSON.stringify(body),
            headers: {
                'Content-Type': 'application/json',
            }
        })
        .then(data=>data.json())
        .then(response=>console.log(response));
    }

    dugme.addEventListener('click', function(event) {
        event.preventDefault();

        DodajZadtak(tekst.value);
        alert('Zadatak je uspešno registrovan!');
    });
});