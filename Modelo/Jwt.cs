﻿
using APICruzber.Connection;
using APICruzber.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Claims;

namespace APICruzber.Modelo
{
    public class Jwt
    {
        public readonly string Key;

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Subject { get; set; }

        public IConfiguration _configuration;

        public  ConnectionBD _cnxdb;

        public string _connectionStrings;

        
        public Jwt( )
        {
            
        }

        public Jwt(IConfiguration configuration,ConnectionBD cnxdb)
        {
            Key = configuration.GetSection("Jwt").GetSection("Key").ToString();
            _cnxdb = cnxdb;
            _connectionStrings = configuration.GetConnectionString("ConnectionStrings:ConexionBD");
            configuration.GetSection("Jwt").Bind(this);
        }

        

        public  dynamic ValidarToken(ClaimsIdentity identity )
        {

                    
            

            try
            {
                
                SqlConnection  connection = new SqlConnection(_cnxdb.cadenaSQL());

                if (identity.Claims.Count() == 0)
                {

                    return new
                    {
                        success = false,
                        message = "Verificando si estas ingresando un token valido",
                        result = ""
                    };

                }else 
                {
                    var usuario = identity.Claims.FirstOrDefault(x => x.Type == "usuario").Value;

                    return new
                    {
                        success = true,
                        message = "Token valido",
                        result = usuario
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Error al obtener la cadena de conexión: " + ex.Message,
                    result = ""
                };
                
            }
        }
    }
}
