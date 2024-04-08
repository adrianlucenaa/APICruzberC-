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

        public async Task<List<ClienteModelo>> MostrarClientes()
        {
            var lista = new List<ClienteModelo>();
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_MostrarClientes", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var clienteModelo = new ClienteModelo();
                            clienteModelo.CodigoCliente = (string)item["CodigoCliente"];
                            clienteModelo.Nombre = (string)item["Nombre"];
                            lista.Add(clienteModelo);
                        }
                    }
                }
            }
            return lista;
        }

        public async Task InsertarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_InsertaarClientes", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task ActualizarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_ActualizarClientes", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Nombre", parametros.Nombre);
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task EliminarCliente(ClienteModelo parametros)
        {
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_EliminarClientes", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoCliente", parametros.CodigoCliente);
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //Metodo de listar clientes por codigo cliente
        public async Task<List<ClienteModelo>> MostrarClientesPorCodigo(string codigoCliente)
        {
            var lista = new List<ClienteModelo>();
            using (var sql = new SqlConnection(cnxdb.cadenaSQL()))
            {
                using (var cmd = new SqlCommand("SP_MoostrarClientesPorCodigo", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CodigoCliente", SqlDbType.VarChar) { Value = codigoCliente });
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var clienteModelo = new ClienteModelo();
                            clienteModelo.CodigoCliente = (string)item["CodigoCliente"];
                            clienteModelo.Nombre = (string)item["Nombre"];
                            lista.Add(clienteModelo);
                        }
                    }
                }
            }
            return lista;
        }

    }
}