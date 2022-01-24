using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces
{
    public interface IConexionService
    {
        
        public SAPbobsCOM.ICompany getConnect();
    }
}
