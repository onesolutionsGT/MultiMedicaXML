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
    public class InvoiceController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IFacturaService _FacturaService;
        public InvoiceController(
            IConexionService conexionService,
            IFacturaService facturaService
            )
        {
            _conexionService = conexionService;
            _FacturaService = facturaService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        //[ProducesResponseType(typeof(ErrorMessage), 400)]
        public IActionResult  Get()
        {
            try
            {
                var respuesta = _FacturaService.addFacturaXML(_conexionService.getConnect());

                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
