
using Microsoft.Extensions.Configuration;

namespace APICruzber.Connection
{
    public class ConnectionBD
    {
        private string ConnectionString = string.Empty;
        
        public ConnectionBD()
        {
            var mybuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            ConnectionString = mybuilder.GetSection("ConnectionStrings:ConexionBD").Value;
            Console.WriteLine(ConnectionString.ToString());
        }
        public string cadenaSQL()
        {
            return ConnectionString;
        }
        
    }
}
