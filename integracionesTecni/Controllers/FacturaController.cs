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
    public class FacturaController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        private readonly IFacturaService _FacturaService;
        public FacturaController(
            IConexionService conexionService,
            IFacturaService facturaService
            )
        {
            _conexionService = conexionService;
            _FacturaService = facturaService;
        }
        // POST api/<FacturaController>
        //[HttpPost]
        //public async Task<ActionResult<Modelos.Respuesta>> Post([FromBody] Modelos.Factura factura)
        //{
        //    try
        //    {
        //        var respuesta = _FacturaService.addFactura(_conexionService.getConnect(), factura);

        //        return Ok(respuesta);

        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}


        //[HttpGet("Invoice")]
        //public async Task<ActionResult<Modelos.Respuesta>> GetXML()
        //{
        //    try
        //    {
        //        var respuesta = _FacturaService.addFacturaXML(_conexionService.getConnect());

        //        return Ok(respuesta);

        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

    }
}
