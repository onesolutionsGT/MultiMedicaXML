using Modelos;
using SAP.Interfaces;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SAP
{
    public class FacturaService : IFacturaService
    {

        public Respuesta addFactura(ICompany B1company, Factura factura)
        {
            try
            {
                SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                documento.CardCode = factura.CardCode;
                documento.DocDate = factura.DocDate;
                documento.DocDueDate = factura.DocDueDate;
                documento.Comments = factura.Commets;
                documento.SalesPersonCode = factura.SalesPersonCode;
                documento.Address2 = factura.DireccionEntrega;
                documento.DiscountPercent = 0;
                if (factura.Rate > 0) documento.DocRate = factura.Rate;

                foreach (Detalle detalle in factura.Detalle)
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
                string sNoFactura;
                B1company.GetNewObjectCode(out sNoFactura);
                return new Respuesta
                {
                    Mensaje = $"Factura creada satisfactoriamente",
                    Codigo = sNoFactura,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }



        }

        public Respuesta addFacturaXML(ICompany B1company)
        {
            string fileName = "";
            int procesados = 0;
            bool validacion_err = false;
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Facturas\Cola"); //Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
                foreach (FileInfo file in Files)
                {
                    fileName = file.FullName;
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    SAPbobsCOM.Recordset RecSet = null;

                    RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    string QryStr = "select Count(*) from oinv t where  t.\"NumAtCard\" = '" + GetElement(xmlConsulta, "//invoice/document/numatcard") + "'";
                    RecSet.DoQuery(QryStr);

                    var documentoNumAtCard = Convert.ToString(RecSet.Fields.Item(0).Value);

                    if (documentoNumAtCard == "0")
                    {
                        SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        documento.CardCode = GetElement(xmlConsulta, "//invoice/document/cardcode"); //factura.CardCode;
                        documento.CardName = GetElement(xmlConsulta, "//invoice/document/cardname");
                        documento.NumAtCard = GetElement(xmlConsulta, "//invoice/document/numatcard");
                        documento.DocCurrency = GetElement(xmlConsulta, "//invoice/document/cursource");
                        documento.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//invoice/document/docdate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        documento.DocDueDate = DateTime.ParseExact(GetElement(xmlConsulta, "//invoice/document/docduedate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        documento.TaxDate = DateTime.ParseExact(GetElement(xmlConsulta, "//invoice/document/taxdate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        documento.Series = int.Parse(GetElement(xmlConsulta, "//invoice/document/series"));
                        //documento.DiscountPercent = double.Parse(GetElement(xmlConsulta, "//invoice/document/discprcnt"));

                        //Campos de usuario.
                        documento.UserFields.Fields.Item("U_COD_PACIENTE").Value = GetElement(xmlConsulta, "//invoice/document/u_paciente");
                        documento.UserFields.Fields.Item("U_Serie").Value = GetElement(xmlConsulta, "//invoice/document/u_seriedoc");
                        documento.UserFields.Fields.Item("U_Numero").Value = GetElement(xmlConsulta, "//invoice/document/u_docnum");
                        documento.UserFields.Fields.Item("U_COD_MED").Value = GetElement(xmlConsulta, "//invoice/document/u_cod_med");
                        documento.UserFields.Fields.Item("U_PROCEDE").Value = GetElement(xmlConsulta, "//invoice/document/u_procede");
                        documento.UserFields.Fields.Item("U_SUB_PROCEDENCIA").Value = GetElement(xmlConsulta, "//invoice/document/u_sub_procedencia");
                        documento.UserFields.Fields.Item("U_F_FACTURA").Value = GetElement(xmlConsulta, "//invoice/document/u_f_factura");
                        documento.UserFields.Fields.Item("U_NO_CAJA").Value = GetElement(xmlConsulta, "//invoice/document/no_caja");
                        documento.UserFields.Fields.Item("U_NO_TRANSACCION").Value = GetElement(xmlConsulta, "//invoice/document/no_transaccion");
                        documento.UserFields.Fields.Item("U_EMP_AUTORIZA_DESC").Value = GetElement(xmlConsulta, "//invoice/document/emp_autoriza_desc");
                        documento.UserFields.Fields.Item("U_HOJA_REFERENCIA").Value = GetElement(xmlConsulta, "//invoice/document/hoja_referencia");
                        documento.UserFields.Fields.Item("U_COD_PROMO").Value = GetElement(xmlConsulta, "//invoice/document/cod_promo");
                        documento.UserFields.Fields.Item("U_COD_EMPLEADO").Value = GetElement(xmlConsulta, "//invoice/document/cod_empleado");
                        documento.UserFields.Fields.Item("U_COD_CLIENTE_CC").Value = GetElement(xmlConsulta, "//invoice/document/cod_cliente_cc");
                        documento.UserFields.Fields.Item("U_COD_TIPOCREDITO").Value = GetElement(xmlConsulta, "//invoice/document/cod_tipocredito");
                        documento.UserFields.Fields.Item("U_NO_PAGOS").Value = GetElement(xmlConsulta, "//invoice/document/no_pagos");
                        documento.UserFields.Fields.Item("U_ORIGEN_GENERACION").Value = GetElement(xmlConsulta, "//invoice/document/origen_generacion");
                        documento.UserFields.Fields.Item("U_COD_CAJERO").Value = GetElement(xmlConsulta, "//invoice/document/cod_cajero");
                        documento.UserFields.Fields.Item("U_COD_USUARIOGENERA").Value = GetElement(xmlConsulta, "//invoice/document/cod_usuariogenera");
                        documento.UserFields.Fields.Item("U_APLICACREDITO").Value = GetElement(xmlConsulta, "//invoice/document/aplicacredito");
                        documento.UserFields.Fields.Item("U_PORCENTAJEIVA").Value = GetElement(xmlConsulta, "//invoice/document/porcentajeiva");
                        documento.UserFields.Fields.Item("U_ESCOPAGO").Value = GetElement(xmlConsulta, "//invoice/document/escopago");
                        documento.UserFields.Fields.Item("U_PORCENTAJECOBERTURA").Value = GetElement(xmlConsulta, "//invoice/document/porcentajecobertura");
                        documento.UserFields.Fields.Item("U_CODIGOCOBERTURA").Value = GetElement(xmlConsulta, "//invoice/document/codigocobertura");
                        documento.UserFields.Fields.Item("U_NOAUTORIZACION").Value = GetElement(xmlConsulta, "//invoice/document/Autoriza_aseguradora");
                        documento.UserFields.Fields.Item("U_OBSERVACIONES_TRANSACCION").Value = GetElement(xmlConsulta, "//invoice/document/observaciones_transaccion");
                        documento.UserFields.Fields.Item("U_NIT").Value = GetElement(xmlConsulta, "//invoice/document/u_nit");

                        if (GetElement(xmlConsulta, "//invoice/document/doctype") != "I")
                        {
                            Console.WriteLine("es de servicios la factura");
                        }
                        documento.DocType = BoDocumentTypes.dDocument_Items;
                        //documento.UserFields.Fields.Item("").Value = "";


                        // if (factura.Rate > 0) documento.DocRate = factura.Rate;

                        XmlNodeList nodeList = GetElementNode(xmlConsulta, "//invoice/document/document_lines/line");
                        foreach (XmlNode nodo in nodeList)
                        {
                            //bop.Addresses.AddressName = nodo["address"].InnerText;//direccion.AddressName;
                            var item = nodo["itemcode"].InnerText;
                            documento.Lines.ItemCode = nodo["itemcode"].InnerText;//detalle.ItemCode;
                            documento.Lines.Quantity = double.Parse(nodo["quantity"].InnerText);
                            documento.Lines.TaxCode = nodo["taxcode"].InnerText;

                            // if(GetElement(xmlConsulta, "//invoice/document/escopago") == "S")
                            //{
                            documento.Lines.PriceAfterVAT = double.Parse(nodo["gtotal"].InnerText);
                            //}
                            //else
                            //{
                            //  documento.Lines.PriceAfterVAT = double.Parse(nodo["priceafvat"].InnerText);
                            //documento.Lines.GrossTotal = double.Parse(nodo["priceafvat"].InnerText) - double.Parse(nodo["gtotal"].InnerText);
                            //}


                            //var disct = double.Parse(nodo["discprcnt"].InnerText) / double.Parse(nodo["priceafvat"].InnerText);
                            //documento.Lines.DiscountPercent = double.Parse(nodo["discprcnt"].InnerText) / double.Parse(nodo["priceafvat"].InnerText);

                            //documento.Lines.CostingCode = nodo["ocrcode"].InnerText;
                            //documento.Lines.CostingCode2 = nodo["ocrcode2"].InnerText;
                            //documento.Lines.CostingCode3 = nodo["ocrcode3"].InnerText;
                            //documento.Lines.CostingCode4 = nodo["ocrcode4"].InnerText;
                            //documento.Lines.CostingCode5 = nodo["ocrcode5"].InnerText;
                            documento.Lines.Add();

                        }
                        int resp = documento.Add();
                        if (resp != 0)
                        {
                            validacion_err = true;
                            Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + B1company.GetLastErrorDescription(), fileName, B1company);
                        }
                        else
                        {
                            File.Move(file.FullName, @"C:\XML\Facturas\Finalizados\" + file.Name);
                            Log.logProc("FACTURA",fileName, B1company);
                            procesados++;
                        }
                    }
                    else
                    {
                        Log.logError("CardName existente (#Ref)", fileName, B1company);
                        validacion_err = true;
                    }
                }
                if (validacion_err)
                {
                    return new Respuesta
                    {
                        Mensaje = $"Ocurrió un error, revisar SAP para mas detalles.",
                    };
                }
                else
                {
                    if (procesados == 0)
                    {
                        return new Respuesta
                        {
                            Mensaje = "No hay documentos para procesar en la carpeta.",
                            Codigo = "0",
                        };
                    }
                    else
                    {
                        return new Respuesta
                        {
                            Mensaje = "Documentos procesados exitosamente, #Procesados: " + procesados.ToString(),
                            Codigo = "0",
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Log.logError(ex.Message + "\n" + ex.InnerException, fileName, B1company);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        private string GetElement(XmlDocument xmlDoc, string etiqueta)
        {
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode(etiqueta);
                if (node == null)
                    throw new Exception("XML no existe elemento: " + etiqueta);
                else
                    return node.InnerText;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private XmlNodeList GetElementNode(XmlDocument xmlDoc, string etiqueta)
        {
            try
            {
                XmlNodeList node = xmlDoc.SelectNodes(etiqueta);
                if (node == null)
                    throw new Exception("XML no existe elemento: " + etiqueta);
                else
                    return node;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
