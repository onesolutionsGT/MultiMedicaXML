using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IArticuloService
    {
        public Respuesta addArticulo(SAPbobsCOM.ICompany B1company, Modelos.Articulo articulo);
        public Respuesta addArticuloXML(SAPbobsCOM.ICompany B1compan);
    }
}
