using System.Data;
using System.Data.SqlClient;
using APICruzber.Connection;
using APICruzber.Interfaces;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace APICruzber.Datos
{
    public class DatosCliente : ICliente
    {
        //Declaro la variable de conexion a la BBDD
        private readonly ConnectionBD _cnxdb;

        //Constructor de datos cliente
        public DatosCliente(ConnectionBD cnxdb)
        {
            _cnxdb = cnxdb;
        }

        //Metodo para mostrar todos los clientes
        public async Task<IActionResult> MostrarClientes()
        {

            try
            {
                // Obtener la lista de clientes utilizando el método apropiado
                var clientes = await ObtenerClientes(); // Reemplaza esto con el método correcto

                // Devolver la lista de clientes como un OkObjectResult
                return new OkObjectResult(clientes);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar clientes: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //Logica con la que obtienes todos los clientes
        private async Task<List<ClienteModelo>> ObtenerClientes()
        {
            //Declaro una lista de clientes
            var lista = new List<ClienteModelo>();
            try
            {
                //Usamos la clase SqlConnection para conectarnos a la BBDD
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Usamos el SqlCommand para ejecutar un procedimiento almacenado
                    using (var cmd = new SqlCommand("SP_MostrarClientes", sql))
                    {
                        //Ejecutamos el procedimiento de manera asincrona
                        await sql.OpenAsync();
                        //Le decimos que tipo de comando es
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                //Creo un nuevo cliente
                                var clienteModelo = new ClienteModelo();
                                //Asignamos los valores
                                clienteModelo.CodigoCliente = (string)reader["CodigoCliente"];
                                clienteModelo.Nombre = (string)reader["Nombre"];
                                //Añadimos el cliente a la lista
                                lista.Add(clienteModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                //Con Throw, puedo relanzar la excepción para que pueda ser manejada por el controlador
                throw;
            }
            return lista;     //Devuelvo la lista de clientes
        }

        //Metodo para insertar un cliente
        public async Task<IActionResult> InsertarCliente(string CodigoCliente, string Nombre)
        {
            try
            {
                //Usamos la clase SqlConnection para conectarnos a la BBDD
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Usamos el SqlCommand para ejecutar un procedimiento almacenado
                    using (var cmd = new SqlCommand("SP_InsertarClientes", sql))
                    {
                        //Le decimos que tipo de comando es
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Añadimos los valores
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                        //Ejecutamos el procedimiento de manera asincrona
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult(new { message = "Cliente insertado correctamente" });
            }
            catch (Exception ex)
            {
                //Imprimir por el menasaje en caso de error
                Console.WriteLine($"Error al insertar cliente: {ex.Message}");
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }

        //Metodo para actualizar un cliente
        public async Task<IActionResult> ActualizarCliente(string CodigoCliente, string Nombre)
        {
            try
            {
                //Usamos la clase SqlConnection para conectarnos a la BBDD
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Usamos el SqlCommand para ejecutar un procedimiento almacenado
                    using (var cmd = new SqlCommand("SP_ActualizarClientes", sql))
                    {
                        //Le decimos que tipo de comando es
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Añadimos los valores
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                        //Ejecutamos el procedimiento de manera asincrona
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult(new { message = "Cliente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                //Mensaje que se va imprimir por pantalla en caso de que no actualize el cliente
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }

        //Metodo para eliminar un cliente
        public async Task<IActionResult> EliminarCliente(string CodigoCliente)
        {
            try
            {
                //Usamos la clase SqlConnection para conectarnos a la BBDD
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Usamos el SqlCommand para ejecutar un procedimiento almacenado
                    using (var cmd = new SqlCommand("SP_EliminarClientes", sql))
                    {
                        //Le decimos que tipo de comando es
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Añado el parametro necesario
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        //Ejecutamos el procedimiento de manera asincrona
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult(new { message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return new OkObjectResult(new { message = "Error al eliminar el cliente" });
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }


        //Metodo para mostrar un cliente por CodigoCliente
        public async Task<IActionResult> MostrarClientesPorCodigo(string CodigoCliente)
        {
            try
            {
                // Obtener la lista de clientes utilizando el método apropiado
                var clientes = await ObtenerClientesPorCodigo(CodigoCliente);

                // Devolver la lista de clientes como un OkObjectResult
                return new OkObjectResult(clientes);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar cliente por código: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //Logica para mostrar un cliente por CodigoCliente
        private async Task<List<ClienteModelo>> ObtenerClientesPorCodigo(string codigoCliente)
        {
            //Declaro una lista de clientes
            var lista = new List<ClienteModelo>();
            try
            {
                //Usamos la clase SqlConnection para conectarnos a la BBDD
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Usamos el SqlCommand para ejecutar un procedimiento almacenado
                    using (var cmd = new SqlCommand("SP_MostrarClientesPorCodigo", sql))
                    {
                        //Añado el parametro necesario
                        cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                        //Ejecutamos el procedimiento de manera asincrona
                        await sql.OpenAsync();
                        //Le decimos que tipo de comando es
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Crear un nuevo ClienteModelo solo con el código y el nombre
                                var clienteModelo = new ClienteModelo();
                                //Asignamos los valores
                                clienteModelo.CodigoCliente = (string)reader["CodigoCliente"];
                                clienteModelo.Nombre = (string)reader["Nombre"];
                                lista.Add(clienteModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Mensaje que se va imprimir por pantalla en caso de que no encuentre el cliente
                Console.WriteLine($"Error al obtener cliente por código: {ex.Message}");
                throw;// Relanzar la excepción para que pueda ser manejada por el controlador
            }
            //Devuelve la lista de clientes
            return lista;
        }
    }
}
