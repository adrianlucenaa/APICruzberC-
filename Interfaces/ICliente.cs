using Microsoft.AspNetCore.Mvc;

namespace APICruzber.Interfaces
{
    //Inetrfaz que define los metodos que van a llevar a mis clases
    public interface ICliente
    {
        Task<IActionResult> MostrarClientes();

        Task<IActionResult> InsertarCliente(string CodigoCliente, string Nombre);
        Task<IActionResult> ActualizarCliente(string CodigoCliente, string Nombre);
        Task<IActionResult> EliminarCliente(string CodigoCliente);
        Task<IActionResult> MostrarClientesPorCodigo(string CodigoCliente);

    }
}