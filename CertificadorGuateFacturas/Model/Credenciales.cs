using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model
{
    public class Credenciales
    {
        private string pUsuario;
        private string pPassword;
        private string pNitEmisor;
        private decimal pEstablecimiento;
        private decimal pTipoDoc;
        private string pIdMaquina;
        private string pTipoRespuesta;
        

        public string PUsuario { get => pUsuario; set => pUsuario = value; }
        public string PPassword { get => pPassword; set => pPassword = value; }
        public string PNitEmisor { get => pNitEmisor; set => pNitEmisor = value; }
        public decimal PEstablecimiento { get => pEstablecimiento; set => pEstablecimiento = value; }
        public decimal PTipoDoc { get => pTipoDoc; set => pTipoDoc = value; }
        public string PIdMaquina { get => pIdMaquina; set => pIdMaquina = value; }
        public string PTipoRespuesta { get => pTipoRespuesta; set => pTipoRespuesta = value; }
    }
}
