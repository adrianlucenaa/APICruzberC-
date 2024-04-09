
using APICruzber.Datos;
using APICruzber.Interfaces;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly ICliente _icliente; // Declara una instancia de IClienteService

        // Modifica el constructor para recibir IClienteService
        public ClienteController(ICliente icliente)
        {
            _icliente = icliente;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteModelo>>> Get()
        {
            var lista = await _icliente.MostrarClientes(); // Utiliza _clienteService en lugar de crear una instancia de DatosCliente
            return lista;
        }

        [HttpPost]
        public async Task Post([FromBody] ClienteModelo parametros)
        {
            await _icliente.InsertarCliente(parametros); // Utiliza _clienteService en lugar de crear una instancia de DatosCliente
        }

        [HttpPut("{CodigoCliente}")]
        public async Task<ActionResult> Put(string CodigoCliente, [FromBody] ClienteModelo parametros)
        {
            parametros.CodigoCliente = CodigoCliente;
            await _icliente.ActualizarCliente(parametros); // Utiliza _clienteService en lugar de crear una instancia de DatosCliente
            return NoContent();
        }

        [HttpDelete("{CodigoCliente}")]
        public async Task<ActionResult> Delete(string CodigoCliente)
        {
            await _icliente.EliminarCliente(CodigoCliente); // Utiliza _clienteService en lugar de crear una instancia de DatosCliente
            return NoContent();
        }

        [HttpGet("{codigoCliente}")]
        public async Task<ActionResult<List<ClienteModelo>>> Get(string codigoCliente)
        {
            var lista = await _icliente.MostrarClientesPorCodigo(codigoCliente); // Utiliza _clienteService en lugar de crear una instancia de DatosCliente
            if (lista == null || lista.Count() == 0)
            {
                return NotFound();
            }
            return lista;
        }
    }
}
/*
namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/clientes")]                             //Ruta que va a tomar la API
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public async Task <ActionResult<List<ClienteModelo>>> Get()
        {
            var funcion = new DatosCliente();                           //Metodo GetALL que se encarga de traer todos los clientes , llamando al procedimineto, y devolviendo la lista
            var lista = await funcion.MostrarClientes();
            return lista;

        }

        [HttpPost]
        public async Task Post([FromBody] ClienteModelo parametros) 
        {
            var funcion = new DatosCliente();                           //Metodo Post que se encarga de insertar un cliente , llamando al procedimineto, y esperando la respuesta
            await funcion.InsertarCliente(parametros);
        }

        [HttpPut("{CodigoCliente}")]
        public async Task <ActionResult> Put (string CodigoCliente, [FromBody] ClienteModelo parametros)
        {
            var funcion = new DatosCliente();                           //Metodo Put que se encarga de actualizar un cliente , llamando al procedimineto, y esperando la respuesta
            parametros.CodigoCliente = CodigoCliente;                   //Si responde un 200 no content esque la echo correctamente 
            await funcion.ActualizarCliente(parametros);                //Actualiza el cliente por codigo cliente 
            return NoContent();
        }

        [HttpDelete("{CodigoCliente}")]
        public async Task <ActionResult> Delete (string CodigoCliente) 
        {
            var funcion = new DatosCliente();                           //Metodo Delete que se encarga de eliminar un cliente , llamando al procedimineto, y esperando la respuesta
            var parametros = new ClienteModelo();                       //Si responde un 200 no content esque la echo correctamente
            parametros.CodigoCliente = CodigoCliente;                   //Eliminar el cliente por sus parametros que va a ser codigo cliente 
            await funcion.EliminarCliente(parametros);
            return NoContent();
        }

        [HttpGet("{codigoCliente}")]
        public async Task<ActionResult<List<ClienteModelo>>> Get(string codigoCliente)          //Buscar cliente por CodigoCliente
        {
            var funcion = new DatosCliente();
            var lista = await funcion.MostrarClientesPorCodigo(codigoCliente);
            if (lista == null || lista.Count == 0)                       //Si la lista es nula o vacia devuelve un not found, si no pues devuelve la lista de clientes
            {                                                           
                return NotFound();
            }
            return lista;
        }

    }
}
*/