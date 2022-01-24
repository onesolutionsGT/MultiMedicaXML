using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IOrdenService
    {
        public Respuesta addOrden(SAPbobsCOM.ICompany B1company, Modelos.Orden orden);
    }
}
