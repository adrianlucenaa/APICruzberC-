
using APICruzber.Connection;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Numerics;
using System.Data.SqlTypes;

namespace APICruzber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    //Controller del usuario
    public class UsuarioController : ControllerBase
    {
        
        public readonly IConfiguration _configuration;
        public ConnectionBD _cnxdb;
        public string _connectionStrings;

        //Constructor
        public UsuarioController(IConfiguration configuration, ConnectionBD cnxdb)
        {
            
            _configuration = configuration;
            _cnxdb = cnxdb;
            _connectionStrings = configuration.GetConnectionString("ConnectionStrings");
        }
       
        [HttpPost]
        [Route("login")]
        //Metodo para iniciar sesion, con usuario existente en la base de datos
        public dynamic IniciarSesion([FromBody] Object OptData)
        {
            //Deserializamos el JSON para obtener los datos
            var data = JsonConvert.DeserializeObject<dynamic>(OptData.ToString());

            var jwtConfig = _configuration.GetSection("Jwt");
            //string jwtKey = jwtConfig["Key"];

            string usuario = data.usuario.ToString();
            string password = data.password.ToString();

            //Conectamos con la base de datos
            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {
                //Comprobamos si el usuario existe
                if (usuario == null)
                {
                    return new
                    {
                        success = false,
                        message = "Usuario o contraseña incorrectos",
                        result = ""
                    };
                }

                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                //Generamos el token , pasandole las entidades por claim
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("usuario", usuario),
                    new Claim("password", password)
                };

                //Generamos la llave para cifrar el token             
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estosoloesunaclavedepruebaadmincruzber08"));

                //Ciframos el token
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //Creo el token con las características deseadas
                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("usuario", usuario),
                        new Claim("password", password)
                    },
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: sigIn
                );

                //Devuelvo el token, si se cumple todo
                return new
                {
                    success = true,
                    message = "perfecto",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
        }
        
        
                
        [HttpPost]
        [Route("validar")]
        //Metodo para validar el token
        public IActionResult Validar()
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];
            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí validas el token JWT
                var esValido = ValidarToken(token);
                if (esValido)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Token válido" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token inválido" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token no proporcionado" });
            }
        }

        //Logica para validar token
        [HttpPost]
        public bool ValidarToken(string token)
        {
            //Instancia de JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Obetenemos la configuración del token
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            //Instanciamos  la clave secreta

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estosoloesunaclavedepruebaadmincruzber08"));

            //Validamos los parametros del token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            //Validamos el token y devolvemos true o false
            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
