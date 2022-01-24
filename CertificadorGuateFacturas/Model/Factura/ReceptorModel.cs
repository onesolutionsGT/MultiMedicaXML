using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model
{
    public class ReceptorModel
    {
        private string _NITRECEPTOR;
        private string _Nombre;
        private string _Direccion;

        public string NITRECEPTOR { get => _NITRECEPTOR; set => _NITRECEPTOR = value; }
        public string Nombre { get => _Nombre; set => _Nombre = value; }
        public string Direccion { get => _Direccion; set => _Direccion = value; }
    }
}
