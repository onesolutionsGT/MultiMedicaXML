using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IClienteService
    {
        public Respuesta addCliente(SAPbobsCOM.ICompany B1company, Modelos.Cliente cliente);
        public Respuesta addClienteXML(SAPbobsCOM.ICompany B1company);
    }
}
