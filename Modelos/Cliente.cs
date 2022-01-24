using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Cliente 
    {
        public Cliente()
        {
            Direcciones = new List<Direccion>();
        }
        private string cardCode;
        private string cardName;
        private string cardForeignName;
        private string cardType;
        private string groupCode;
        private string phone1;
        private string phone2;
        private string fax;
        private string currency;
        private string salesPersonCode;
        private string emailAddress;
        private string rtn;
        private string password;
        public string CardCode { get => cardCode; set => cardCode = value; }
        public string CardName { get => cardName; set => cardName = value; }
        public string CardForeignName { get => cardForeignName; set => cardForeignName = value; }
        public string CardType { get => cardType; set => cardType = value; }
        public string GroupCode { get => groupCode; set => groupCode = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        public string Fax { get => fax; set => fax = value; }
        public string Currency { get => currency; set => currency = value; }
        public string SalesPersonCode { get => salesPersonCode; set => salesPersonCode = value; }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public string Rtn { get => rtn; set => rtn = value; }
        public string Password { get => password; set => password = value; }
        public List<Direccion> Direcciones { get => direcciones; set => direcciones = value; }

        private List<Direccion> direcciones;
    }
    public class Direccion
    {
        private string numeroDeDireccion;
        private string addressName;
        private string city;
        private string country;
        private AddressCType addressType;
        private string street;
        private string state;

        public string NumeroDeDireccion { get => numeroDeDireccion; set => numeroDeDireccion = value; }
        public string AddressName { get => addressName; set => addressName = value; }
        public string City { get => city; set => city = value; }
        public string Country { get => country; set => country = value; }
        public AddressCType AddressType { get => addressType; set => addressType = value; }

        public string Street { get => street; set => street = value; }

        public string State { get => state; set => state = value; }
    }

    public enum AddressCType
    {
        FACTURACION = 1,
        DESTINO = 2
    }
}


