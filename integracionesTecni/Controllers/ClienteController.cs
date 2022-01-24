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
    //[Produces("application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly IConexionService _conexionService;
        private readonly IClienteService _clienteService;

        public ClienteController(
            IConexionService conexionService,
            IClienteService clienteService
            )
        {
            _conexionService = conexionService;
            _clienteService = clienteService;
        }

        // POST api/<ClienteController>
        [HttpPost]
        public async Task<ActionResult<Modelos.Respuesta>> Post([FromBody] Modelos.Cliente cliente)
        {
            try
            {
                var respuesta = _clienteService.addCliente(_conexionService.getConnect(), cliente);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Modelos.Respuesta), 200)]
        public  IActionResult GetXML()
        {
            try
            {
                var respuesta = _clienteService.addClienteXML(_conexionService.getConnect());
                return Ok(respuesta);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
