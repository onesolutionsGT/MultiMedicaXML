using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAP.Interfaces;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace integracionesTecni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificarFacturaController : ControllerBase
    {


        private readonly IConexionService _conexionService;
        private readonly IClienteService _clienteService;

        public CertificarFacturaController(
            IConexionService conexionService,
            IClienteService clienteService
            )
        {
            _conexionService = conexionService;
            _clienteService = clienteService;
        }


        // GET: api/<CertificarFacturaController>
        [HttpGet]
        public async Task<ActionResult<SAPbobsCOM.IBusinessPartners>> Get()
        {
            try
            {
                CertificadorGuateFacturas.Factura factura = new CertificadorGuateFacturas.Factura();
                var bpo = _clienteService.addCliente(_conexionService.getConnect(),null);

                return Ok(bpo);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        // GET api/<CertificarFacturaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CertificarFacturaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CertificarFacturaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CertificarFacturaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
