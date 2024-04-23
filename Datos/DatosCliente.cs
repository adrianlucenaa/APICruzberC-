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
        private readonly ConnectionBD _cnxdb;

        public DatosCliente(ConnectionBD cnxdb)
        {
            _cnxdb = cnxdb;
        }

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

        private async Task<List<ClienteModelo>> ObtenerClientes()
        {
            var lista = new List<ClienteModelo>();
            try
            {
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    using (var cmd = new SqlCommand("SP_MostrarClientes", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var clienteModelo = new ClienteModelo();
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
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                throw;
            }
            return lista;
        }


        public async Task InsertarCliente(string CodigoCliente, string Nombre)
        {
            try
            {
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    using (var cmd = new SqlCommand("SP_InsertarClientes", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar cliente: {ex.Message}");
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }

        public async Task ActualizarCliente(string CodigoCliente, string Nombre)
        {
            try
            {
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    using (var cmd = new SqlCommand("SP_ActualizarClientes", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        cmd.Parameters.AddWithValue("@Nombre", Nombre);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }


        public async Task EliminarCliente(string CodigoCliente)
        {
            try
            {
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    using (var cmd = new SqlCommand("SP_EliminarClientes", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                throw; // Relanzar la excepción para que pueda ser manejada por el controlador
            }
        }

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

        private async Task<List<ClienteModelo>> ObtenerClientesPorCodigo(string codigoCliente)
        {
            var lista = new List<ClienteModelo>();
            try
            {
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    using (var cmd = new SqlCommand("SP_MostrarClientesPorCodigo", sql))
                    {
                        cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Crear un nuevo ClienteModelo solo con el código y el nombre
                                var clienteModelo = new ClienteModelo();
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
                Console.WriteLine($"Error al obtener cliente por código: {ex.Message}");
                throw;
            }
            return lista;
        }


    }
}
