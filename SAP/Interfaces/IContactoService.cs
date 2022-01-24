using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IContactoService
    {
        public Respuesta addContacto(SAPbobsCOM.ICompany B1company, Contacto contacto, string CardCode);
    }
}
