using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Documento
    {
       public  Documento()
        {
            Detalle = new List<Detalle>();
        }
        private string cardCode;
        private DateTime docDueDate;
        private DateTime docDate;
        private string docCurrency;
        private Double rate;
        private string commets;
        private int salesPersonCode;
        private string direccionEntrega;
        private List<Detalle> detalle;

        public string CardCode { get => cardCode; set => cardCode = value; }
        public DateTime DocDueDate { get => docDueDate; set => docDueDate = value; }
        public DateTime DocDate { get => docDate; set => docDate = value; }
        public string DocCurrency { get => docCurrency; set => docCurrency = value; }
        public double Rate { get => rate; set => rate = value; }
        public string Commets { get => commets; set => commets = value; }
        public int SalesPersonCode { get => salesPersonCode; set => salesPersonCode = value; }
        public string DireccionEntrega { get => direccionEntrega; set => direccionEntrega = value; }
        public List<Detalle> Detalle { get => detalle; set => detalle = value; }
    }


    public class Detalle
    {
        private int lineNum;
        private string itemCode;
        private string taxCode;
        private string warehouseCode;
        private double quantity;
        private double priceAfterVAT;
        private string cOGSCostingCode;
        private string cOGSCostingCode2;
        private string cOGSCostingCode3;
        private string cOGSCostingCode4;
        private string cOGSCostingCode5;


        public int LineNum { get => lineNum; set => lineNum = value; }
        public string ItemCode { get => itemCode; set => itemCode = value; }
        public string TaxCode { get => taxCode; set => taxCode = value; }
        public string WarehouseCode { get => warehouseCode; set => warehouseCode = value; }
        public double Quantity { get => quantity; set => quantity = value; }
        public double PriceAfterVAT { get => priceAfterVAT; set => priceAfterVAT = value; }
        public string COGSCostingCode { get => cOGSCostingCode; set => cOGSCostingCode = value; }
        public string COGSCostingCode2 { get => cOGSCostingCode2; set => cOGSCostingCode2 = value; }
        public string COGSCostingCode3 { get => cOGSCostingCode3; set => cOGSCostingCode3 = value; }
        public string COGSCostingCode4 { get => cOGSCostingCode4; set => cOGSCostingCode4 = value; }
        public string COGSCostingCode5 { get => cOGSCostingCode5; set => cOGSCostingCode5 = value; }

    }

}