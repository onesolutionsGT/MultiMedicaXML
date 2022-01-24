using Microsoft.AspNetCore.Mvc;
using SAP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace integracionesTecni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IContactoService _contactoService;
        public ContactoController(
             IConexionService conexionService,
            IContactoService contacto
            )
        {
            _conexionService = conexionService;
            _contactoService = contacto;
        }


        // POST api/<ContactoController>
        [HttpPost]
        public async Task<ActionResult<Modelos.Respuesta>> Post([FromBody] Modelos.Contacto contacto, string cardCode)
        {
            try
            {
                var respuesta = _contactoService.addContacto(_conexionService.getConnect(), contacto, cardCode);
                return Ok(respuesta);
            }
            catch (Exception)
            {

                throw;
            }
        }

      
    }
}
