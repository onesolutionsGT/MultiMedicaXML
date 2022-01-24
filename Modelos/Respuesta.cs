using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Respuesta
    {
        private string mensaje;
        private string codigo;

        public string Mensaje { get => mensaje; set => mensaje = value; }
        public string Codigo { get => codigo; set => codigo = value; }
    }
}
