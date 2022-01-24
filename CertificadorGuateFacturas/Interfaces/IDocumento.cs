using CertificadorGuateFacturas.Model;
using CertificadorGuateFacturas.Model.Factura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Interfaces
{
    public interface IDocumento
    {
        
          void Certificar(Credenciales credenciales, string xmlGenerado);

          void Anular(Credenciales credenciales, string xmlGenerado);

        string ArmarXML(Model.Factura.Factura facturaenc);

        
        
    }
}
