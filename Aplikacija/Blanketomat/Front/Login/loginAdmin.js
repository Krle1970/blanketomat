document.addEventListener("DOMContentLoaded", () => {
    const loginButton = document.querySelector(".dugmeLogin");

    loginButton.addEventListener("click", async (event) => {
        event.preventDefault();

        const email = document.querySelector(".email").value;
        const password = document.querySelector(".password").value;
        const accountType = document.querySelector("#role").value;

        const loginRequest = {
            email: email,
            password: password,
            accountType: accountType
        };

        let endpoint = '';
        switch (accountType) {
            case 'Administrator':
                endpoint = 'http://localhost:5246/Auth/login-admin';
                break;
            case 'Student':
                endpoint = 'http://localhost:5246/Auth/login-student';
                break;
            case 'Profesor':
                endpoint = 'http://localhost:5246/Auth/login-profesor';
                break;
            case 'Asistent':
                endpoint = 'http://localhost:5246/Auth/login-asistent';
                break;
            default:
                alert('Nepoznat tip naloga');
                return;
        }
     
        try {
            const response = await fetch(endpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(loginRequest)
            });

            if (!response.ok) {
                const errorText = await response.text();
                console.error(`Greška: ${response.status} ${response.statusText} - ${errorText}`);
                throw new Error(`Greška: ${response.status} ${response.statusText} - ${errorText}`);
            }

            const data = await response.json();
            console.log('Uspešno prijavljen:', data);

            localStorage.setItem('token', data.token);
            console.log('Token sačuvan u lokalnoj memoriji:', data.token);

            // odlaganje pre preusmeravanja
            setTimeout(() => {
                if(accountType=="Administrator")
                {
                    window.location.href = '../admin-page/index.html';
                }
                else if (accountType=="Profesor")
                {
                    window.location.href = '../PageStudente/KreirajBlanketP.html';
                }
                else if(accountType=="Asistent")
                {
                    window.location.href = '../PageStudente/KreirajBlanketZ.html';
                }
                else if(accountType=="Student")
                {
                    window.location.href = '../PageStudente/Pocetna.html';
                }
            }, 1000); // 1000ms je 1 sekunda

        } catch (error) {
            console.error('Greška:', error.message);
            alert(error.message);
        }
    });
});
