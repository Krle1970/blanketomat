document.addEventListener('DOMContentLoaded', function() {
    const dropdownMenuPredmeti = document.getElementById('predmeti-dropdown1');
    const dropdownMenuOblasti = document.getElementById('oblasti-dropdown1');
    const dropdownMenuPodoblasti = document.getElementById('podoblasti-dropdown1');
    let selectedPredmetId = null;
    let selectedOblastId = null;
    
    fetch('http://localhost:5246/Predmet/predmeti')
        .then(response => response.json())
        .then(data => {
            dropdownMenuPredmeti.innerHTML = '';
  
            data.forEach(predmet => {
                const item = document.createElement('a');
                item.classList.add('dropdown-item');
                item.setAttribute('data-value', predmet.id);
                item.textContent = predmet.naziv;
                dropdownMenuPredmeti.appendChild(item);

                item.addEventListener('click', function() {
                    selectedPredmetId = predmet.id; // Čuvanje ID-a izabranog predmeta
                    console.log('Izabrani predmet ID:', selectedPredmetId); 
                    console.log(predmet.id);
                    // Pozivanje funkcije za dobijanje oblasti
                    fetchOblastiForPredmet(selectedPredmetId);
                });
            });
        })
        .catch(error => console.error('Error fetching subjects:', error));

    function fetchOblastiForPredmet(predmetId) {
        const url = `http://localhost:5246/Predmet/${predmetId}/oblasti`;

        fetch(url)
            .then(response => response.json())
            .then(data => {
                console.log('Oblasti za izabrani predmet:', data);
                
                dropdownMenuOblasti.innerHTML = ''; 
                
                data.forEach(oblast => {
                    const item = document.createElement('a');
                    item.classList.add('dropdown-item');
                    item.setAttribute('data-value', oblast.id);
                    item.textContent = oblast.naziv;
                    dropdownMenuOblasti.appendChild(item);

                    item.addEventListener('click', function() {
                        selectedOblastId = oblast.id; // Čuvanje ID-a izabrane oblasti
                        console.log('Izabrana oblast ID:', oblast.id);
                        
                       
                        fetchPodoblastiForOblast(selectedOblastId);
                    });
                });

                // Prikazivanje dropdown menija za oblasti
                if (dropdownMenuOblasti.childElementCount > 0) {
                    dropdownMenuOblasti.classList.add('show');
                } else {
                    dropdownMenuOblasti.classList.remove('show');
                }
            })
            .catch(error => console.error('Error fetching oblasti:', error));
    }

    function fetchPodoblastiForOblast(oblastId) {
        const url = `http://localhost:5246/Oblast/${oblastId}/podoblasti`;

        fetch(url)
            .then(response => response.json())
            .then(data => {
                console.log('Podoblasti za izabranu oblast:', data);
                
                dropdownMenuPodoblasti.innerHTML = ''; 

                data.forEach(podoblast => {
                    const item = document.createElement('a');
                    item.classList.add('dropdown-item');
                    item.setAttribute('data-value', podoblast.id);
                    item.textContent = podoblast.naziv;
                    dropdownMenuPodoblasti.appendChild(item);
                });

                // Prikazivanje dropdown menija za podoblasti
                if (dropdownMenuPodoblasti.childElementCount > 0) {
                    dropdownMenuPodoblasti.classList.add('show');
                } else {
                    dropdownMenuPodoblasti.classList.remove('show');
                }
            })
            .catch(error => console.error('Error fetching podoblasti:', error));
    }
});

  

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

        DodajPitanje(tekst.value);
        alert('Pitanje je uspešno registrovan!');
    });
});