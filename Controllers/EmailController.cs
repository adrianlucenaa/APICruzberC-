using APICruzber.Connection;
using APICruzber.Interfaces;
using APICruzber.Modelo;
using APICruzber.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/emails")]
    public class EmailController: ControllerBase , IEmail
    {
        public readonly ConnectionBD _cnxdb;

        public readonly IConfiguration _configuration;

        public readonly IEmail _email;

        private readonly ILogger<EmailController> _logger;

        public EmailController(IConfiguration configuration, ConnectionBD cnxdb, IEmail email, ILogger<EmailController> logger)
        {
            _configuration = configuration;
            _cnxdb = cnxdb;
            _email = email;
            _logger = logger;
        }

        [HttpGet("{lang}")]
        
        public async Task<IActionResult> GetMails(string lang)
        {
            try
            {
                // Obtener la lista de correos electrónicos
                var mails = await _email.GetMails(lang);
                //Si mail es igual a null impreme un no content , si no es igual te vuelve un 200 y la lista de emails
                if (mails == null )
                {
                    return NoContent(); // Devuelve un 204 si no hay correos electrónicos
                }
                return Ok(mails); // Devuelve un 200 con la lista de correos electrónicos
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los correos electrónicos para el idioma {lang}", lang);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

