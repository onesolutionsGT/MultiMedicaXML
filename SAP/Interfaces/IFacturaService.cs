using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IFacturaService
    {
        public Respuesta addFactura(SAPbobsCOM.ICompany B1company, Modelos.Factura factura);
        public Respuesta addFacturaXML(SAPbobsCOM.ICompany B1compan);
    }
}
