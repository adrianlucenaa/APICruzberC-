using System.Collections.Generic;
using System.Threading.Tasks;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using APICruzber.Datos;
using APICruzber.Interfaces;
<<<<<<< Updated upstream
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using APICruzber.Connection;
using Microsoft.AspNetCore.Authorization;
=======
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
>>>>>>> Stashed changes

namespace APICruzber.Controllers
{
    [ApiController]
    //La ruta que va  a tomar para acceder a la API
    [Route("api/clientes")]
<<<<<<< Updated upstream
    //[Authorize]
=======
    [Authorize]
>>>>>>> Stashed changes
    public class ClienteController : ControllerBase, ICliente
    {
        //Declaramos la variable _cliente
        private readonly ICliente _cliente;

        public IConfiguration _configuration;
        private ConnectionBD _cnxdb;



        //Constructor de ClienteController
        public ClienteController(IConfiguration configuration, ICliente cliente, ConnectionBD cnxdb)
        {
            _configuration = configuration;
            _cliente = cliente;
            _cnxdb = cnxdb;
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
        
        public Task InsertarCliente([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente,
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 20 caracteres.")]  string Nombre)
        {
            //Devuelve el cliente que hemos insertado, llamando a la logica de InsertarCliente de DatosClientes
            return _cliente.InsertarCliente(CodigoCliente, Nombre);
        }

        //Metodo para actualizar cliente
        [HttpPut]
        public Task ActualizarCliente([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente,
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 20 caracteres.")] string Nombre)
        {
            //Devuelve el cliente que hemos actualizado , llamando a la logica de ActualizarCliente de DatosCliente.
            return _cliente.ActualizarCliente(CodigoCliente, Nombre);
        }

        //Metedo para eliminar un cliente por CodigoCliente
        [HttpDelete("{CodigoCliente}")]
        public async Task EliminarCliente([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente)
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
        public async Task<IActionResult> MostrarClientesPorCodigo([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente)
        {
            //return _cliente.MostrarClientesPorCodigo(CodigoCliente);
            try
            {
                //Llama la ICliente para mostrar al cliente
                var clientes = await _cliente.MostrarClientesPorCodigo(CodigoCliente);

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar cliente por codigo: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al mostrar clientes.");
            }
        }

        /*
        
        [HttpPost]
        [Route("eliminar")]
        public dynamic EliminarCliente(ClienteModelo cliente)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // Crear una instancia de la clase Jwt
            var jwt = new Jwt(_configuration, _cnxdb);

            // Llamar al método ValidarToken en la instancia de Jwt
            var respuestaToken = jwt.Validar(identity);

            if (identity == null)
            {
                return new
                {
                    success = false,
                    message = "No se pudo obtener la identidad del usuario",
                    result = ""
                };
            }

            if (!respuestaToken.success)
            {
                return respuestaToken;
            }

            return new
            {
                success = true,
                message = "Cliente eliminado correctamente",
                result = cliente
            };
        }
        
       */

        

    }
}

