using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dw.UserService.Controllers.Dto
{
    public class ContactDto
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
