
using APICruzber.Datos;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public async Task <ActionResult<List<ClienteModelo>>> Get()
        {
            var funcion = new DatosCliente();
            var lista = await funcion.MostrarClientes();
            return lista;

        }
        [HttpPost]
        public async Task Post([FromBody] ClienteModelo parametros) 
        {
            var funcion = new DatosCliente();
            await funcion.InsertarCliente(parametros);
        }
        [HttpPut("{CodigoCliente}")]
        public async Task <ActionResult> Put (string CodigoCliente, [FromBody] ClienteModelo parametros)
        {
            var funcion = new DatosCliente();
            parametros.CodigoCliente = CodigoCliente;
            await funcion.ActualizarCliente(parametros);
            return NoContent();
        }
        [HttpDelete("{CodigoCliente}")]
        public async Task <ActionResult> Delete (string CodigoCliente) 
        {
            var funcion = new DatosCliente();
            var parametros = new ClienteModelo();
            parametros.CodigoCliente = CodigoCliente;
            await funcion.EliminarCliente(parametros);
            return NoContent();
        }

        [HttpGet("{codigoCliente}")]
        public async Task<ActionResult<List<ClienteModelo>>> Get(string codigoCliente)
        {
            var funcion = new DatosCliente();
            var lista = await funcion.MostrarClientesPorCodigo(codigoCliente);
            if (lista == null || lista.Count == 0)
            {
                return NotFound();
            }
            return lista;
        }

    }
}
