using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificadorGuateFacturas.Model
{
    public class InfoDocModel
    {
        private string _TipoVenta;
        private int _DestinoVenta;
        private DateTime _Fecha;
        private TipoMoneda _Moneda;
        private float _Tasa;
        private string _Referencia;

        public string TipoVenta { get => _TipoVenta; set => _TipoVenta = value; }
        public int DestinoVenta { get => _DestinoVenta; set => _DestinoVenta = value; }
        public DateTime Fecha { get => _Fecha; set => _Fecha = value; }
        public TipoMoneda Modeda { get => _Moneda; set => _Moneda = value; }
        public float Tasa { get => _Tasa; 
            set {
                if (_Moneda == TipoMoneda.Quetzal)
                {
                    _Tasa = 1;
                }
                else
                {
                    _Tasa = value;
                }
                
            }  
        }
        public string Referencia { get => _Referencia; set => _Referencia = value; }

    }
}

public enum TipoMoneda
{
    Quetzal = 1,
    Dolar = 2
}
