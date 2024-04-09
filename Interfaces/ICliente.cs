using APICruzber.Modelo;

namespace APICruzber.Interfaces
{
    public interface ICliente
    {
        Task<List<ClienteModelo>> MostrarClientes();
        Task InsertarCliente(ClienteModelo parametros);
        Task ActualizarCliente(ClienteModelo parametros);
        Task EliminarCliente(string codigoCliente);
        Task<List<ClienteModelo>> MostrarClientesPorCodigo(string codigoCliente);
    }
}
