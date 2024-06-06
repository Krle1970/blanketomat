document.addEventListener('DOMContentLoaded', function() {
 
    const tekst = document.querySelector('.tekst');
    const dugme = document.querySelector('.dugme');

    function DodajPitanje(tekst) {
        const body = {
           Tekst:tekst
           
        };


        fetch(`http://localhost:5246/Pitanje` , {
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
        alert('Pitanje je uspešno registrovan!');
    });
});