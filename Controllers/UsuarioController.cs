
using APICruzber.Connection;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
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

            string usuario = data.usuario.ToString();
            string password = data.password.ToString();

          

            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {



                if (usuario == null)
                {
                    return new
                    {
                        success = false,
                        message = "Usuario o contraseña incorrectos",
                        result = ""
                    };
                }
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("usuario", usuario),
                    new Claim("password", password)
                    //new Claim("usuario", usuario.usuario),
                    //new Claim("password", usuario.password)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                byte[] keyBytes = new byte[16]; // 16 bytes = 128 bits
                using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(keyBytes);
                }

                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
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
    
    /*
    using System;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APICruzber.Connection;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _configuration;
        private ConnectionBD _cnxdb;
        private string _connectionStrings;


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

            string user = data.usuario.ToString();
            string password = data.password.ToString();

            Usuario usuario = null;
        
            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {
                //SqlCommand command = new SqlCommand("ObteenerUsuario", connection);
                SqlCommand command = new SqlCommand("AgregarUsuario", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@usuario", user);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    // Si el usuario existe en la base de datos, crea un objeto Usuario con los datos obtenidos
                    usuario = new Usuario
                    {
                        usuario = reader["usuario"].ToString(),
                        password = reader["password"].ToString()
                    };
                }

                reader.Close();
            }

            
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

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("usuario", usuario.usuario),
                new Claim("password", usuario.password)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var sigIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: sigIn
                );
            return new
            {
                success = true,
                message = "Autenticación exitosa",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }
    }
}
    */