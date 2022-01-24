using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model
{
    public class TotalesModel
    {
        private double _Bruto;
        private double _Descuento;
        private double _Exento;
        private double _Otros;
        private double _Neto;
        private double _ISR;
        private double _Iva;
        private double _Total;

        [Description("Importe bruto luego de calcular precio por cantidad")]
        public double Bruto { get => _Bruto; set => _Bruto = value; }
        
        [Description("Importe descuento aplicado")]
        public double Descuento { get => _Descuento; set => _Descuento = value; }

        [Description("Importe exento de cálculo de IVA")]
        public double Exento { get => _Exento; set => _Exento = value; }

        [Description("Importe otros impuestos no afectos a IVA")]
        public double Otros { get => _Otros; set => _Otros = value; }

        [Description("Importe neto sjeto a cálculo de IVA")]
        public double Neto { get => _Neto; set => _Neto = value; }

        [Description("Importe ISR")]
        public double ISR { get => _ISR; set => _ISR = value; }

        [Description("Importe IVA aplicado sobre el importe neto")]
        public double Iva { get => _Iva; set => _Iva = value; }

        [Description("Importe IVA aplicado sobre el importe neto")]
        public double Total { get => _Total; set => _Total = value; }
       
    }
}
