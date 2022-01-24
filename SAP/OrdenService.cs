using Modelos;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP
{
    public class OrdenService : Interfaces.IOrdenService
    {
        public Respuesta addOrden(ICompany B1company, Orden orden)
        {
            try
            {
                SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                documento.CardCode = orden.CardCode;
                documento.DocDate = orden.DocDate;
                documento.DocDueDate = orden.DocDueDate;
                documento.Comments = orden.Commets;
                documento.SalesPersonCode = orden.SalesPersonCode;
                documento.Address2 = orden.DireccionEntrega;
                documento.DiscountPercent = 0;
                if (orden.Rate > 0) documento.DocRate = orden.Rate;

                foreach (Detalle detalle in orden.Detalle)
                {
                    documento.Lines.ItemCode = detalle.ItemCode;
                    documento.Lines.Quantity = detalle.Quantity;
                    documento.Lines.TaxCode = detalle.TaxCode;
                    documento.Lines.WarehouseCode = detalle.WarehouseCode;
                    documento.Lines.PriceAfterVAT = detalle.PriceAfterVAT;
                    documento.Lines.DiscountPercent = 0;
                    if (detalle.TaxCode == "EXE") documento.Lines.UnitPrice = detalle.PriceAfterVAT;

                    documento.Lines.COGSCostingCode = detalle.COGSCostingCode;
                    documento.Lines.COGSCostingCode2 = detalle.COGSCostingCode2;
                    documento.Lines.COGSCostingCode3 = detalle.COGSCostingCode3;
                    documento.Lines.COGSCostingCode4 = detalle.COGSCostingCode4;
                    documento.Lines.COGSCostingCode5 = detalle.COGSCostingCode5;
                    documento.Lines.Add();

                }
                int resp = documento.Add();
                if (resp != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);


                }
                //return documento;
                string sNoOrden;
                B1company.GetNewObjectCode(out sNoOrden);
                return new Respuesta
                {
                    Mensaje = $"Factura creada satisfactoriamente",
                    Codigo = sNoOrden,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
