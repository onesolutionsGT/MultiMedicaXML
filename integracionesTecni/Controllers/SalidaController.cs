using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace integracionesTecni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalidaController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly ISalidaService _SalidaService;

        public SalidaController(
            IConexionService conexionService,
            ISalidaService salidaService
            )
        {
            _conexionService = conexionService;
            _SalidaService = salidaService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        //[ProducesResponseType(typeof(ErrorMessage), 400)]
        public IActionResult Get()
        {
            try
            {
                var respuesta = _SalidaService.addSalidaXML(_conexionService.getConnect());

                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}