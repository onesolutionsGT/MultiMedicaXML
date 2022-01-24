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
    public class OrdenController : ControllerBase
    {
        private readonly IConexionService _conexionService;
        public OrdenController(
            IConexionService conexionService
            )
        {
            _conexionService = conexionService;
        }

        // POST api/<OrdenController>
        [HttpPost]
        public void Post([FromBody] Modelos.Orden orden )
        {
        }

    }
}
