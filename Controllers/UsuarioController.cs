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
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
       
        public IConfiguration _configuration;
        public ConnectionBD _cnxdb;
        public string _connectionStrings;

        public UsuarioController(IConfiguration configuration, ConnectionBD cnxdb)
        {
            
            _configuration = configuration;
            _cnxdb = cnxdb;
            _connectionStrings = configuration.GetConnectionString("ConnectionStrings");
        }

        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] Object OptData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(OptData.ToString());

            string usuario = data.usuario?.ToString();
            string password = data.password?.ToString();
            //string rol = data.rol.ToString();

            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {
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
                using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(keyBytes);
                }

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

                return new
                {
                    success = true,
                    message = "perfecto",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
        }
    }
}