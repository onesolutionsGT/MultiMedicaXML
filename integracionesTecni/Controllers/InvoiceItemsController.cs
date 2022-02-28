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
    public class FacturaItemsController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IFacturaItemsService _FacturaItemsService;
        public FacturaItemsController(
            IConexionService conexionService,
            IFacturaItemsService facturaItemsService
            )
        {
            _conexionService = conexionService;
            _FacturaItemsService = facturaItemsService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        //[ProducesResponseType(typeof(ErrorMessage), 400)]
        public IActionResult Get()
        {
            try
            {
                var respuesta = _FacturaItemsService.addFacturaItemsXML(_conexionService.getConnect());

                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
