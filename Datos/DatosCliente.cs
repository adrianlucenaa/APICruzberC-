using APICruzber.Connection;
using APICruzber.Modelo;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace APICruzber.Datos
{
    public class DatosCliente
    {
        ConnectionBD cnxdb = new ConnectionBD();

        // Método para mostrar todos los clientes
        public async Task<List<ClienteModelo>> MostrarClientes()
        {
            var lista = new List<ClienteModelo>();
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_MostrarClientes", sql))
                {
                    await sql.OpenAsync();                                      // Abre la conexión asíncronamente
                    cmd.CommandType = CommandType.StoredProcedure;              // Especifica que se está utilizando un stored procedure
                    using (var item = await cmd.ExecuteReaderAsync())           // Ejecuta la consulta y crea un lector de datos
                    {
                        while (await item.ReadAsync())                          // Itera sobre cada fila del resultado
                        {
                            var clienteModelo = new ClienteModelo();
                                                                                // Lee los datos de cada fila y los asigna al modelo de cliente
                            clienteModelo.CodigoCliente = (string)item["CodigoCliente"];
                            clienteModelo.Nombre = (string)item["Nombre"];
                            lista.Add(clienteModelo);                           // Agrega el modelo de cliente a la lista
                        }
                    }
                }
            }
            return lista;                                                       // Devuelve la lista de clientes
        }

        // Método para insertar un nuevo cliente
        public async Task InsertarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_InsertaarClientes", sql))                    //Usa el procedimiento almacenado 
                {
                    cmd.CommandType = CommandType.StoredProcedure;                               //Utiliza un stored procedure
                                                                                                // Añade los parámetros necesarios para la inserción del cliente
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                    await sql.OpenAsync();                                                      // Abre la conexión asíncronamente
                    await cmd.ExecuteNonQueryAsync();                                           // Ejecuta el comando de inserción en la base de datos
                }
            }
        }

        // Método para actualizar los datos de un cliente existente
        public async Task ActualizarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_ActualizarClientes", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;                  
                                                                                                // Añade los parámetros necesarios para la actualización del cliente
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                    await sql.OpenAsync();                                                      // Abre la conexión asíncronamente
                    await cmd.ExecuteNonQueryAsync();                                           // Ejecuta el comando de actualización en la base de datos
                }
            }
        }

        // Método para eliminar un cliente
        public async Task EliminarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_EliminarClientes", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 
                                                                                                // Añade los parámetros necesarios para la eliminación del cliente
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    await sql.OpenAsync();                                                      
                    await cmd.ExecuteNonQueryAsync();                                           // Ejecuta el comando de eliminación en la base de datos
                }
            }
        }

        // Método para mostrar clientes filtrados por código
        public async Task<List<ClienteModelo>> MostrarClientesPorCodigo(string codigoCliente)
        {
            var lista = new List<ClienteModelo>();
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_MoostrarClientesPorCodigo", sql))
                {
                    await sql.OpenAsync(); 
                    cmd.CommandType = CommandType.StoredProcedure; 
                                                                                                                            // Añade el parámetro para filtrar los clientes por código
                    cmd.Parameters.Add(new SqlParameter("@CodigoCliente", SqlDbType.VarChar) { Value = codigoCliente });
                    using (var item = await cmd.ExecuteReaderAsync())                                                       // Ejecuta la consulta y crea un lector de datos
                    {
                        while (await item.ReadAsync())                                                                      // Itera sobre cada fila del resultado
                        {
                            var clienteModelo = new ClienteModelo();
                                                                                                                            // Lee los datos de cada fila y los asigna al modelo de cliente
                            clienteModelo.CodigoCliente = (string)item["CodigoCliente"];
                            clienteModelo.Nombre = (string)item["Nombre"];
                            lista.Add(clienteModelo);                                                                           
                        }
                    }
                }
            }
            return lista;                                                                                                   // Devuelve la lista de clientes filtrados por código
        }

    }
}
