using Modelos;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface ISalidaService

    {
        public Respuesta addSalidaXML(ICompany B1compan);
    }
}