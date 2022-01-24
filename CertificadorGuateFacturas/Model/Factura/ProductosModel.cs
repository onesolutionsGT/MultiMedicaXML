using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model.Factura
{
    public class ProductosModel
    {
        private string _Producto;
        private string _Descripcion;
        private string _Medida;
        private double _Cantidad;
        private double _Precio;
        private double _PorcDesc;
        private double _ImpBruto;
        private double _ImpDescuento;
        private double _ImpExento;
        private double _ImpOtros;
        private double _ImpNeto;
        private double _ImpIsr;
        private double _ImpIva;
        private double _ImpTotal;
        private string _TipoVentaDet;

        public string Producto { get => _Producto; set => _Producto = value; }
        public string Descripcion { get => _Descripcion; set => _Descripcion = value; }
        public string Medida { get => _Medida; set => _Medida = value; }
        public double Cantidad { get => _Cantidad; set => _Cantidad = value; }
        public double Precio { get => _Precio; set => _Precio = value; }
        public double PorcDesc { get => _PorcDesc; set => _PorcDesc = value; }
        public double ImpBruto { get => _ImpBruto; set => _ImpBruto = value; }
        public double ImpDescuento { get => _ImpDescuento; set => _ImpDescuento = value; }
        public double ImpExento { get => _ImpExento; set => _ImpExento = value; }
        public double ImpOtros { get => _ImpOtros; set => _ImpOtros = value; }
        public double ImpNeto { get => _ImpNeto; set => _ImpNeto = value; }
        public double ImpIsr { get => _ImpIsr; set => _ImpIsr = value; }
        public double ImpIva { get => _ImpIva; set => _ImpIva = value; }
        public double ImpTotal { get => _ImpTotal; set => _ImpTotal = value; }
        public string TipoVentaDet { get => _TipoVentaDet; set => _TipoVentaDet = value; }
    }
}
