using Microsoft.AspNetCore.Http;
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
    public class PagoController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IPagoService _pagoService;
        public PagoController(
            IConexionService conexionService,IPagoService pagoService
            )
        {
            _conexionService = conexionService;
            _pagoService = pagoService;
        }

        // POST api/<PagoController>
        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        public IActionResult GetXML()
        {
            try
            {
                var respuesta = _pagoService.addPagoXML(_conexionService.getConnect());
                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
