using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using PharmaSysAPI.Models;
using System.Collections.Generic;
using System.Collections;

namespace PharmaSysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ProductosController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_lista_Productos", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                    Nombre = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoría")),
                                    IdMarca = reader.GetInt32(reader.GetOrdinal("IdMarca")),
                                    Precio = reader.GetDecimal(reader.GetOrdinal("PrecioVenta")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    DetalleLote = reader.GetInt32(reader.GetOrdinal("IdDetalleLote"))
                                });
                            }
                        }
                    }
                }
                return Ok(new { mensaje = "OK", response = productos });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            Producto producto = null;

            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_obtener_producto", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idProducto", idProducto);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto
                                {
                                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                    Nombre = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                    IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoría")),
                                    IdMarca = reader.GetInt32(reader.GetOrdinal("IdMarca")),
                                    Precio = reader.GetDecimal(reader.GetOrdinal("PrecioVenta")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    DetalleLote = reader.GetInt32(reader.GetOrdinal("IdDetalleLote"))
                                };
                            }
                        }
                    }
                }
                if (producto == null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }
                return Ok(new { mensaje = "OK", response = producto });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_guardar_producto", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                        cmd.Parameters.AddWithValue("stock", objeto.Stock);
                        cmd.Parameters.AddWithValue("idCategoria", objeto.IdCategoria);
                        cmd.Parameters.AddWithValue("idMarca", objeto.IdMarca);
                        cmd.Parameters.AddWithValue("precio", objeto.Precio);
                        cmd.Parameters.AddWithValue("descripcion", objeto.Descripcion);
                        cmd.Parameters.AddWithValue("detalleLote", objeto.DetalleLote);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { mensaje = "OK" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_editar_producto", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto);
                        cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                        cmd.Parameters.AddWithValue("stock", objeto.Stock);
                        cmd.Parameters.AddWithValue("idCategoria", objeto.IdCategoria);
                        cmd.Parameters.AddWithValue("idMarca", objeto.IdMarca);
                        cmd.Parameters.AddWithValue("precio", objeto.Precio);
                        cmd.Parameters.AddWithValue("descripcion", objeto.Descripcion);
                        cmd.Parameters.AddWithValue("detalleLote", objeto.DetalleLote);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { mensaje = "Editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var connection = new SqlConnection(cadenaSQL))
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_eliminar_producto", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("idProducto", idProducto);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error: {error.Message}", response = (object)null });
            }
        }
    }

    }