using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model.Factura
{
    public class Factura
    {
        public Factura()
        {
            productos = new List<ProductosModel>();
        }
        [DispId(6)]
        public ReceptorModel receptor;
        [DispId(7)]
        public InfoDocModel infoDoc;
        [DispId(8)]
        public TotalesModel totalModel;
        [DispId(9)]
        public List<ProductosModel> productos;
    }
}
