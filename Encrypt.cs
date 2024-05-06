using System.Security.Cryptography;
using System.Text;

namespace APICruzber
{
    public class Encrypt
    {
        //Metodologia implementada para encriptar la contraseñas que desehemos almacenar
        public static string GetSHA256(string str)
        {
            SHA256 sha = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        
     }
}
