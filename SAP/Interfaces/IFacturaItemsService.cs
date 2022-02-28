using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IFacturaItemsService
    {
        public Respuesta addFacturaItems(SAPbobsCOM.ICompany B1company, Modelos.Factura factura);
        public Respuesta addFacturaItemsXML(SAPbobsCOM.ICompany B1compan);
    }
}
