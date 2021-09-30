using System;
using System.Collections.Generic;

#nullable disable

namespace dw.UserService.Models
{
    public partial class Usuario
    {
        public long IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
    }
}
