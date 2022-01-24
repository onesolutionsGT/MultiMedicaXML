using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IPagoService
    {
        public Respuesta addPago(SAPbobsCOM.ICompany B1company, Modelos.Pago pago);
        public Respuesta addPagoXML(SAPbobsCOM.ICompany B1company);
    }
}
