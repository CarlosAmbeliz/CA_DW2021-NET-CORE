using System;
using System.Collections.Generic;

#nullable disable

namespace dw.UserService.Models
{
    public partial class Contacto
    {
        public long IdContacto { get; set; }
        public long IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}
