using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace APICruzber.Modelo
{
    //Modelo que dice la informcaion que va a contener el cliente
    public class ClienteModelo
    {
        [Required(ErrorMessage = "El código del cliente es obligatorio.")]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "El código del cliente debe tener entre 3 y 10 caracteres.")]
        public string? CodigoCliente { get; set; }  //Geteamos y seteamos el Codigocliente de Cliente

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre del cliente debe tener entre 3 y 50 caracteres.")]
        public string? Nombre { get; set; }        //Geteamos y seteamos el Nombre de Cliente
    }
}