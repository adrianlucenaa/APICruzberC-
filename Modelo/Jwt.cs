using APICruzber.Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Linq;

namespace APICruzber.Modelo
{
    //Modelo del Jwt
    public class Jwt 
    {
        public  string Key;

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Subject { get; set; }

        public IConfiguration _configuration;

        public ConnectionBD _cnxdb;
        
        public string connectionString = "data source=200SERVER;initial catalog=Cruzber;user=logic;password=Sage2009+";

        public Jwt()
        {
        }

        public Jwt(IConfiguration configuration, ConnectionBD cnxdb)
        {
            Key = configuration.GetSection("Jwt").GetSection("Key").ToString();
            _cnxdb = cnxdb;
            connectionString = configuration.GetConnectionString("ConnectionStrings:ConexionBD");
            configuration.GetSection("Jwt").Bind(this);
        }
        /*
        public dynamic ValidarToken(ClaimsIdentity identity)
        {
            using (SqlConnection connection = new SqlConnection(_cnxdb.cadenaSQL()))
            {
                try
                {

                    if (identity.Claims.Count() == 0)
                    {
                        return new
                        {
                            succes = false,
                            message = "Verificando si estas ingresando un token valido",
                            result = ""

                        };
                    }
                    //Obtenemos el usuario por nombre de usuario

                    var cc = identity.Claims.FirstOrDefault(x => x.Type == "CodigoCliente").Value;

                    //ClienteModelo clientes  = .FirstOrDefautl(x => x.usuario == Usuario);
                    
                    return new
                    {
                        success = true,
                        message = "Token valido",
                        //result = 
                    };
                    
                    //return clientes;
                }
                catch (Exception ex)
                {
                    return new
                    {
                        success = false,
                        message = "Catch:" + ex.Message,
                        result = ""
                    };
                }
            }
        }
        */
    }
}