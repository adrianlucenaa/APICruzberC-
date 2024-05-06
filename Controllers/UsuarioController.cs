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

namespace APICruzber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Controller del usuario
    public class UsuarioController : ControllerBase
    {
<<<<<<< Updated upstream
       
        public IConfiguration _configuration;
        public ConnectionBD _cnxdb;
        public string _connectionStrings;

        public UsuarioController(IConfiguration configuration, ConnectionBD cnxdb)
        {
            
=======
        //Variables de la clase
        public readonly IConfiguration _configuration;
        public ConnectionBD _cnxdb;
        public string _connectionStrings;

        //Constructor
        public UsuarioController(IConfiguration configuration, ConnectionBD cnxdb)
        {           
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            string usuario = data.usuario?.ToString();
            string password = data.password?.ToString();
            //string rol = data.rol.ToString();
=======
            string usuario = data.usuario.ToString();
            string password = data.password.ToString();

>>>>>>> Stashed changes

            //Conectamos con la base de datos
            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {
<<<<<<< Updated upstream
=======


                //Comprobamos si el usuario existe
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                /*
                if (jwt.Key == null)
                {
                    // Manejar la situación donde la clave de JWT es nula
                    return new
                    {
                        success = false,
                        message = "La clave de JWT no está configurada correctamente",
                        result = ""
                    };
                }
                */
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                //var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                byte[] keyBytes = new byte[16];
=======
                
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
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                //Ciframos el token
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //Establecemos la cantidad de bytes para la clave
                byte[] keyBytes = new byte[16]; // 16 bytes = 128 bits
>>>>>>> Stashed changes
                using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(keyBytes);
                }

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

<<<<<<< Updated upstream
=======

                //Devuelvo el token, si se cumple todo
>>>>>>> Stashed changes
                return new
                {
                    success = true,
                    message = "perfecto",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
        }
<<<<<<< Updated upstream
    }
}
=======

        [ HttpPost]
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
        private bool ValidarToken(string token)
        {
            //Instancia de JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Obetenemos la configuración del token
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            //Instanciamos  la clave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

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

>>>>>>> Stashed changes
