using System.Collections.Generic;
using System.Threading.Tasks;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using APICruzber.Datos;
using APICruzber.Interfaces;

namespace APICruzber.Controllers
{
    [ApiController]
    //La ruta que va  a tomar para acceder a la API
    [Route("api/clientes")]
    public class ClienteController : ControllerBase, ICliente
    {
        //Declaramos la variable _cliente
        private readonly ICliente _cliente;

        //Constructor de ClienteController
        public ClienteController(ICliente cliente)                                  
        {
            
            _cliente = cliente;
        }

        //Metodo para mostrar los clientes
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> MostrarClientes()
        {
            try
            {
                // Llamar a la implementación de ICliente para mostrar clientes
                var clientes = await _cliente.MostrarClientes();

                // Devolver la lista de clientes como un OkObjectResult
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar clientes: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al mostrar clientes.");
            }
        }

        //Metodo para añadir cliente 
        [HttpPost]
        public Task InsertarCliente(string CodigoCliente, string Nombre)
        {
            //Devuelve el cliente que hemos insertado, llamando a la logica de InsertarCliente de DatosClientes
            return _cliente.InsertarCliente(CodigoCliente, Nombre);
        }

        //Metodo para actualizar cliente
        [HttpPut]
        public Task ActualizarCliente(string CodigoCliente, string Nombre)
        {
            //Devuelve el cliente que hemos actualizado , llamando a la logica de ActualizarCliente de DatosCliente.
            return _cliente.ActualizarCliente(CodigoCliente, Nombre);
        }

        //Metedo para eliminar un cliente por CodigoCliente
        [HttpDelete("{CodigoCliente}")]
        public async Task EliminarCliente(string CodigoCliente)
        {
            try
            {
                // Llamar a la implementación de ICliente para eliminar el cliente
                await _cliente.EliminarCliente(CodigoCliente);

             
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
               
            }
        }

        //Metodo para buscar un cliente por CodigoCliente
        [HttpGet("{CodigoCliente}")]
        public async Task<IActionResult> MostrarClientesPorCodigo(string CodigoCliente)
        {
            //return _cliente.MostrarClientesPorCodigo(CodigoCliente);
            try
            {
                //Llama la ICliente para mostrar al cliente
                var clientes = await _cliente.MostrarClientesPorCodigo(CodigoCliente);

                return Ok(clientes);
            }
            catch(Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar cliente por codigo: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al mostrar clientes.");
            }
        }

    }
}
