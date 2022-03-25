using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP.Interfaces;
using System;

namespace integracionesTecni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IArticuloService _ArticuloService;

        public ArticuloController(
            IConexionService conexionService,
            IArticuloService articuloService
            )
        {
            _conexionService = conexionService;
            _ArticuloService = articuloService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        //[ProducesResponseType(typeof(ErrorMessage), 400)]
        public IActionResult Get()
        {
            try
            {
                var respuesta = _ArticuloService.addArticuloXML(_conexionService.getConnect());

                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
