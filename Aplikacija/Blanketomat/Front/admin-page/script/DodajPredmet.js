document.addEventListener('DOMContentLoaded', function () {
  const form = document.getElementById('dodajPredmetForm');
  const subjectNameInput = document.getElementById('subjectName');
  const subjectYearSelect = document.getElementById('subjectYear');

  form.addEventListener('submit', async function (event) {
      event.preventDefault();

      const naziv = subjectNameInput.value;
      const godina = subjectYearSelect.value;

      if (!naziv || !godina) {
          alert('Molimo unesite naziv i odaberite godinu predmeta.');
          return;
      }

      const noviPredmet = {
          Naziv: naziv,
          Godina: godina
      };

     try
     {
        const response = await fetch('http://localhost:5246/Predmet', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(noviPredmet)
        });
  
        if (response.ok)
        {
          alert('Predmet je uspešno dodat.');
          
        }
        else if (response.status === 400)
        {
          alert('Predmet već postoji.');
        }
        else
        {
          alert('Greška prilikom dodavanja predmeta.');
        }
     }
     catch (e)
     {
       alert('Greška prilikom dodavanja predmeta.');
     }
  });
});
