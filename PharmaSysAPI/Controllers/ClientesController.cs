using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using PharmaSysAPI.Models;
using Microsoft.AspNetCore.Cors;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PharmaSysAPI.Controllers
{
    [EnableCors("reglasCors")]
    [Route("api/[controller]")]
    [ApiController]

    public class ClientesController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ClientesController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_lista_Clientes", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clientes.Add(new Cliente()
                                {
                                    IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                                    NombreCliente = reader.GetString("NombreCliente"),
                                    CedulaCliente = reader.GetString("CédulaCliente"),
                                    TelefonoCliente = reader.GetInt32("TeléfonoCliente"),
                                    DireccionCliente = reader.GetString("DirecciónCliente"),
                                    TipoCliente = reader.GetString("TipoCliente")
                                });
                            }
                        }
                    }
                    return Ok(new { mensaje = "OK", response = clientes });
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }
        [HttpGet]
        [Route("Obtener/{idCliente:int}")]

        public IActionResult Obtener(int idCliente)
        {
            List<Cliente> Clientes = new List<Cliente>();
            Cliente cliente = new Cliente();

            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_lista_Clientes", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clientes.Add(new Cliente()
                                {
                                    IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                                    NombreCliente = reader.GetString(reader.GetOrdinal("NombreCliente")),
                                    CedulaCliente = reader.GetString(reader.GetOrdinal("CédulaCliente")),
                                    TelefonoCliente = reader.GetInt32(reader.GetOrdinal("TeléfonoCliente")),
                                    DireccionCliente = reader.GetString(reader.GetOrdinal("DirecciónCliente")),
                                    TipoCliente = reader.GetString(reader.GetOrdinal("TipoCliente"))

                                });
                            }
                        }
                    }
                }
                cliente = Clientes.Where(item => item.IdCliente == idCliente).FirstOrDefault();
                return Ok(new { mensaje = "OK", response = cliente });
            }

            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Cliente objeto)
        {
            try
            {
                if (objeto == null)
                {
                    return BadRequest(new { mensaje = "Los datos del cliente no fueron proporcionados." });
                }
                
                if (string.IsNullOrEmpty(objeto.NombreCliente))
                {
                    return BadRequest(new { mensaje = "El nombre del cliente es requerido." });
                }

                if (string.IsNullOrEmpty(objeto.CedulaCliente))
                {
                    return BadRequest(new { mensaje = "La cédula del cliente es requerida." });
                }

                if (string.IsNullOrEmpty(objeto.TelefonoCliente.ToString()))
                {
                    return BadRequest(new { mensaje = "El teléfono del cliente es requerido." });
                }

                if (string.IsNullOrEmpty(objeto.DireccionCliente))
                {
                    return BadRequest(new { mensaje = "La dirección del cliente es requerida." });
                }

                if (string.IsNullOrEmpty(objeto.TipoCliente))
                {
                    return BadRequest(new { mensaje = "El tipo del cliente es requerida." });
                }

                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_insertar_Cliente", connection))
                    {
                        cmd.Parameters.AddWithValue("NombreCliente", objeto.NombreCliente);
                        cmd.Parameters.AddWithValue("CedulaCliente", objeto.CedulaCliente);
                        cmd.Parameters.AddWithValue("TelefonoCliente", objeto.TelefonoCliente);
                        cmd.Parameters.AddWithValue("DireccionCliente", objeto.DireccionCliente);  
                        cmd.Parameters.AddWithValue("tipoCliente", objeto.TipoCliente);

                        cmd.CommandType = CommandType.StoredProcedure;
                        var filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            return Ok(new { mensaje = "Cliente guardado exitosamente." });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "No se pudo guardar el cliente." });
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error en la base de datos: {sqlEx.Message}", response = (object)null });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Ocurrió un error: {error.Message}", response = (object)null });
            }
        }


        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Cliente objeto)
        {
            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_editar_Clientes", connection))
                    {
                        cmd.Parameters.AddWithValue("idCliente", objeto.IdCliente == 0 ? DBNull.Value : objeto.IdCliente);
                        cmd.Parameters.AddWithValue("nombre", objeto.NombreCliente is null ? DBNull.Value : objeto.NombreCliente);
                        cmd.Parameters.AddWithValue("cedula", objeto.CedulaCliente is null ? DBNull.Value : objeto.CedulaCliente);
                        cmd.Parameters.AddWithValue("telefono", objeto.TelefonoCliente == 0 ? DBNull.Value : objeto.TelefonoCliente);
                        cmd.Parameters.AddWithValue("direccion", objeto.DireccionCliente is null ? DBNull.Value : objeto.DireccionCliente);
                        cmd.Parameters.AddWithValue("tipoCliente", objeto.TipoCliente is null ? DBNull.Value : objeto.TipoCliente);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });

            }

            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idCliente:int}")]
        public IActionResult Eliminar(int idCliente)
        {
            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_eliminar_cliente", connection))
                    {
                        cmd.Parameters.AddWithValue("idCliente", idCliente);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });

            }

            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }
    }
}