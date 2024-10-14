// Variables de botones
const btnGuardar = document.getElementById('btnGuardar');
const btnEditar = document.getElementById('btneditar');
const btnEliminar = document.getElementById('btneliminar');

btnGuardar.addEventListener('click', guardarCliente);
btnEditar.addEventListener('click', cargarFormularioEditar);
btnEliminar.addEventListener('click', eliminarCliente);

document.addEventListener('DOMContentLoaded', function () {
    cargarClientes();
    
});

let clienteSeleccionado = null;

function guardarCliente() {
    const idCliente = document.getElementById('idCliente').value;
    const nombreCliente = document.getElementById('inputnombre').value;
    const cedulaCliente = document.getElementById('inputcedula').value;
    const telefonoCliente = document.getElementById('inputtelefono').value;
    const direccionCliente = document.getElementById('inputdireccion').value;
    const tipoCliente = document.getElementById('cbcliente').value;

    const cliente = {
        IdCliente: idCliente ? parseInt(idCliente) : 0,
        NombreCliente: nombreCliente,
        CedulaCliente: cedulaCliente,
        TelefonoCliente: telefonoCliente,
        DireccionCliente: direccionCliente,
        TipoCliente: tipoCliente
    };

    if (!ValidarCampos()) return;

    if (idCliente) {
        fetch('https://localhost:7266/api/Clientes/Editar', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cliente)
        })
            .then(response => response.json())
            .then(data => {
                if (data.mensaje === "Editado") {
                    alert("Cliente actualizado correctamente");
                    cargarClientes(); 
                    limpiarCampos();  
                } else {
                    console.error(data.mensaje);
                }
            })
            .catch(error => console.error('Error:', error));
    } else {
        fetch('https://localhost:7266/api/Clientes/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(cliente)
        })
            .then(response => response.json())
            .then(data => {
                if (data.mensaje === "Guardado") {
                    alert("Cliente guardado correctamente");
                    cargarClientes(); 
                    limpiarCampos();  
                } else {
                    console.error(data.mensaje);
                }
            })
            .catch(error => console.error('Error:', error));
    }
}

function eliminarCliente() {
    // Verificar si hay un cliente seleccionado
    const idCliente = clienteSeleccionado ? clienteSeleccionado.idCliente : null;

    if (!idCliente) {
        alert("Por favor, seleccione un cliente para eliminar.");
        return;
    }

    // Confirmar la acción de eliminar
    if (!confirm("¿Está seguro de que desea eliminar este cliente?")) {
        return;  
    }

    // Realizar la solicitud para eliminar el cliente
    fetch(`https://localhost:7266/api/Clientes/Eliminar/${idCliente}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.mensaje === "Eliminado") {
                alert("Cliente eliminado correctamente.");
                cargarClientes();  
                limpiarCampos();   
                clienteSeleccionado = null; 
            } else {
                console.error(data.mensaje);
            }
        })
        .catch(error => console.error('Error en la petición:', error));
}


// Función para cargar la lista de clientes desde la API
function cargarClientes() {
    fetch('https://localhost:7266/api/Clientes/Lista')
        .then(response => response.json())
        .then(data => {
            if (data.mensaje === "OK") {
                const clientes = data.response;
                const tabla = document.getElementById('tablaClientes').getElementsByTagName('tbody')[0];

                tabla.innerHTML = ''; 

                clientes.forEach(cliente => {
                    const fila = tabla.insertRow();

                    fila.insertCell(0).innerHTML = cliente.idCliente;
                    fila.insertCell(1).innerHTML = cliente.nombreCliente;
                    fila.insertCell(2).innerHTML = cliente.cedulaCliente;
                    fila.insertCell(3).innerHTML = cliente.telefonoCliente;
                    fila.insertCell(4).innerHTML = cliente.direccionCliente;
                    fila.insertCell(5).innerHTML = cliente.tipoCliente;

                    fila.onclick = function () {
                        seleccionarFila(fila, cliente); // Seleccionar fila y cliente
                    };
                });
            } else {
                console.error('Mensaje del servidor:', data.mensaje);
            }
        })
        .catch(error => console.error('Error en la petición:', error));
}

// Función para seleccionar una fila de la tabla
function seleccionarFila(fila, cliente) {
    const filas = document.querySelectorAll('#tablaClientes tbody tr');
    filas.forEach(f => f.classList.remove('seleccionada')); 
    fila.classList.add('seleccionada'); 

    clienteSeleccionado = cliente; 
}

// Cargar el formulario para editar con los datos del cliente seleccionado
function cargarFormularioEditar() {
    if (!clienteSeleccionado) {
        alert('Por favor, seleccione un cliente de la tabla.');
        return;
    }

    document.getElementById('idCliente').value = clienteSeleccionado.idCliente;
    document.getElementById('inputnombre').value = clienteSeleccionado.nombreCliente;
    document.getElementById('inputcedula').value = clienteSeleccionado.cedulaCliente;
    document.getElementById('inputtelefono').value = clienteSeleccionado.telefonoCliente;
    document.getElementById('inputdireccion').value = clienteSeleccionado.direccionCliente;
    document.getElementById('cbcliente').value = clienteSeleccionado.tipoCliente;
}

// Validar campos del formulario
function ValidarCampos() {
    const nombre = document.getElementById('inputnombre').value.trim();
    const cedula = document.getElementById('inputcedula').value.trim();
    const telefono = document.getElementById('inputtelefono').value.trim();
    const direccion = document.getElementById('inputdireccion').value.trim();
    const tipoCliente = document.getElementById('cbcliente').value.trim();

    if (!nombre || !cedula || !telefono || !direccion || !tipoCliente) {
        alert("Por favor rellene todos los campos.");
        return false;
    }
    return true;
}

// Limpiar campos del formulario
function limpiarCampos() {
    document.getElementById('idCliente').value = null;
    document.getElementById('inputnombre').value = '';
    document.getElementById('inputcedula').value = '';
    document.getElementById('inputtelefono').value = '';
    document.getElementById('inputdireccion').value = '';
    document.getElementById('cbcliente').value = '';
}

