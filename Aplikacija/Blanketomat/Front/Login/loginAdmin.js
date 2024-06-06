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

        try {
            const response = await fetch('http://localhost:5246/Auth/login-admin', {
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

            // Dodajte odlaganje pre preusmeravanja
            setTimeout(() => {
                window.location.href = 'http://127.0.0.1:5501/Front/admin-page/index.html';
            }, 1000); // 2000ms je 2 sekunde

        } catch (error) {
            console.error('Greška:', error.message);
            alert(error.message);
        }
    });
});
