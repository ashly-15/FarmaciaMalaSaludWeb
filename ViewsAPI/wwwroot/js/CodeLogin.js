document.querySelector('.btnIniciarSesion').addEventListener('click', function (e) {
    e.preventDefault(); 

    let usuario = document.getElementById('txtUser').value;
    let contraseña = document.getElementById('txtContraseña').value;

    if (!contraseña) {
        alert("La contraseña no puede estar vacía");
        return;
    }

    fetch('https://localhost:7266/api/Employees/login', { 
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            user: usuario,
            password: contraseña
        })
    })
        .then(response => {
            if (response.ok) {
                return response.json(); 
            } else {
                throw new Error('Credenciales incorrectas');
            }
        })
        .then(data => {
            alert(data.mensaje);
            window.location.href = 'htmlpage.html'; 
        })
        .catch(error => {
            alert(error.message); 
        });
});

