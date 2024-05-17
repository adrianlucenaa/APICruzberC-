using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using APICruzber.Connection;
using APICruzber.Interfaces;
using Microsoft.AspNetCore.Authorization;
using APICruzber.Controllers;
using Microsoft.Extensions.Configuration;
using APICruzber.Modelo;


namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize]
    public class ClienteController : ControllerBase, ICliente
    {
        public readonly ConnectionBD _cnxdb;
        public readonly IConfiguration _configuration;
        public readonly ICliente _cliente;

        public ClienteController(IConfiguration configuration, ConnectionBD cnxdb, ICliente cliente)
        {
            _configuration = configuration;
            _cnxdb = cnxdb;
            _cliente = cliente;
        }
        

        [HttpPut]
        public async Task<IActionResult> ActualizarCliente(
        [Required, StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente,
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 20 caracteres.")] string Nombre)
        {
            // Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            // Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Validar el token JWT
                var usuarioController = new UsuarioController(_configuration, _cnxdb);
                var esValido = usuarioController.ValidarToken(token);

                if (esValido)
                {
                    // Verificar la existencia del cliente
                    var actionResult = await _cliente.MostrarClientesPorCodigo(CodigoCliente);
                    var okObjectResult = actionResult as OkObjectResult;
                    var clientes = okObjectResult?.Value as List<ClienteModelo>;

                    if (clientes != null && clientes.Any())
                    {
                        var primerCliente = clientes.First();
                        // Actualizar el cliente solo si se proporciona un nombre válido
                        if (!string.IsNullOrWhiteSpace(Nombre))
                        {
                            var resultado = await _cliente.ActualizarCliente(CodigoCliente, Nombre);
                            return resultado;
                        }
                        else
                        {
                            return BadRequest(new { message = "El nombre es obligatorio para la actualización" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Cliente no encontrado" });
                    }
                }
                else
                {
                    return Unauthorized(new { message = "Token no válido" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Token no proporcionado o incorrecto" });
            }
        }

        [HttpDelete("{CodigoCliente}")]
        public async Task<IActionResult> EliminarCliente([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente)
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí creas una instancia de UsuarioController
                var usuarioController = new UsuarioController(_configuration, _cnxdb);

                // Aquí validas el token JWT
                var esValido = usuarioController.ValidarToken(token);
                if (esValido)
                {
                    // Verificar la existencia del cliente
                    var actionResult = await _cliente.MostrarClientesPorCodigo(CodigoCliente);
                    var okObjectResult = actionResult as OkObjectResult;
                    var clientes = okObjectResult?.Value as List<ClienteModelo>;
                    if (clientes != null && clientes.Any())
                    {
                        var primerCliente = clientes.First();
                        await _cliente.EliminarCliente(CodigoCliente);
                        return Ok(new { message = "Cliente eliminado correctamente" });
                    }
                    else {
                        return BadRequest(new { message = "Cliente Incorrecto" });
                    }
                }
                else
                {
                    return Unauthorized(new { message = "Error al eliminar cliente" });
                }
            }
            else
            {
                return BadRequest(new { message = "Token no proporcionado o incorrecto" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertarCliente([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente,
            [StringLength(20, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 20 caracteres.")] string Nombre)
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí creas una instancia de UsuarioController
                var usuarioController = new UsuarioController(_configuration, _cnxdb);

                // Aquí validas el token JWT
                var esValido = usuarioController.ValidarToken(token);
                if (esValido)
                {
                    await _cliente.InsertarCliente(CodigoCliente, Nombre);
                    return Ok(new { message = "Cliente insertado correctamente" });
                }
                else
                {
                    return BadRequest(new { message = "Cliente Incorrecto faltan campos o no existe" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Error al actualizar cliente, falta el token o esta incorrecto" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MostrarClientes()
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí creas una instancia de UsuarioController
                var usuarioController = new UsuarioController(_configuration, _cnxdb);

                // Aquí validas el token JWT
                var esValido = usuarioController.ValidarToken(token);
                if (esValido)
                {
                    var clientes = await _cliente.MostrarClientes();
                    return Ok(clientes);
                }
                else
                {
                    return BadRequest(new { message = "Error al mostrar clientes" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Error al devolver los clientes, falta el token o esta incorrecto" });
            }
        }

        [HttpGet("{CodigoCliente}")]
        public async Task<IActionResult> MostrarClientesPorCodigo([StringLength(7, MinimumLength = 6, ErrorMessage = "Debe tener entre 6 y 7 caracteres.")] string CodigoCliente)
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí creas una instancia de UsuarioController
                var usuarioController = new UsuarioController(_configuration, _cnxdb);

                // Aquí validas el token JWT
                var esValido = usuarioController.ValidarToken(token);
                if (esValido)
                {
                    var actionResult = await _cliente.MostrarClientesPorCodigo(CodigoCliente);
                    var okObjectResult = actionResult as OkObjectResult;
                    var clientes = okObjectResult?.Value as List<ClienteModelo>; // Ajusta ClienteModelo al tipo correcto

                    if (clientes != null && clientes.Any())
                    {
                        var primerCliente = clientes.First();
                        return Ok(primerCliente);
                    }
                    else
                    {
                        return BadRequest(new { message = "No se encontraron clientes para el código proporcionado" });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Error al mostrar cliente por su código" });
                }
            }
            else
            {
                return Unauthorized(new { message = "Error al devolver los clientes, falta el token o está incorrecto" });
            }
        }
    }
}