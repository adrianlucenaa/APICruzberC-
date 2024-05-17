using APICruzber.Connection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APICruzber.Modelo
{
    //Modelo que dice la informcaion que va a contener el usuario
    public class Usuario
    {
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El usuario  debe tener entre 3 y 20 caracteres.")]
        public String usuario { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 3 y 20 caracteres.")]
        public String password { get; set; }

      
    }
}
