using dw.UserService.Controllers.Dto;
using dw.UserService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dw.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {

        [HttpGet("authorization")]
        [Authorize(Roles = "Administrators")]
        [Route("getall")]
        public List<ContactDto> GetAll(long idUsuario)
        {
            using (DWContext context = new DWContext())
            {
                return context.Contactos.Where(x => x.IdUsuario == idUsuario).OrderBy(x=> x.Nombres).ThenBy(x=> x.Apellidos).Select(contacto =>
                  new ContactDto()
                  {
                      Apellidos = contacto.Apellidos,
                      Direccion = contacto.Direccion,
                      Email = contacto.Email,
                      IdContacto = contacto.IdContacto,
                      IdUsuario = contacto.IdUsuario,
                      Nombres = contacto.Nombres,
                      Telefono = contacto.Telefono,
                  }).ToList();
            }
        }

        [HttpPost]
        [Route("add")]
        public ContactDto Add(ContactDto contacto)
        {
            using (DWContext context = new DWContext())
            {
                context.Add(new Contacto()
                {
                    Apellidos = contacto.Apellidos,
                    Direccion = contacto.Direccion,
                    Email = contacto.Email,
                    IdUsuario = contacto.IdUsuario,
                    Nombres = contacto.Nombres,
                    Telefono = contacto.Telefono,
                });
                context.SaveChanges();
            }
            return contacto;
        }

        [HttpDelete]
        [Route("remove")]
        public void Remove(long idUsuario, long idContacto)
        {
            using (DWContext context = new DWContext())
            {
                context.Remove(context.Contactos.Where(x => x.IdUsuario == idUsuario & x.IdContacto == idContacto));
                context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("getbyidcontacto")]
        public ContactDto GetByIdContacto(long idUsuario, long idContacto)
        {
            using (DWContext context = new DWContext())
            {
                var contacto = context.Contactos.Where(x => x.IdUsuario == idUsuario & x.IdContacto == idContacto).FirstOrDefault();
                return new ContactDto
                {
                    Apellidos = contacto.Apellidos,
                    Direccion = contacto.Direccion,
                    Email = contacto.Email,
                    IdContacto = contacto.IdContacto,
                    IdUsuario = contacto.IdUsuario,
                    Nombres = contacto.Nombres,
                    Telefono = contacto.Telefono,
                };
            }
        }
    }
}
