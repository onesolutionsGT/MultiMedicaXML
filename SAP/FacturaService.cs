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
                SAPbobsCOM.Recordset RecSet = null;
                RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
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
                   
                    string QryStr = "select \"U_NEW_COD\" from \"@ARTICULO_REF_COD\" where  \"U_OLD_COD\" = '" + detalle.ItemCode + "'";
                    RecSet.DoQuery(QryStr);
                    var nuevo_codigo = Convert.ToString(RecSet.Fields.Item(0).Value);
                    if(nuevo_codigo != "")
                    {
                        documento.Lines.ItemCode = nuevo_codigo;
                    }
                    else
                    {
                        documento.Lines.ItemCode = detalle.ItemCode;
                    }
                    documento.Lines.ItemCode = nuevo_codigo;
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
                RecSet = null;
                GC.Collect();
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
            int factura_existe = 0;
            string numatcard ="";
            string QryStr = "";
            int procesados = 0;
            bool validacion_err = false;
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Facturas\Cola"); //Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
                SAPbobsCOM.Recordset RecSet = null;
                RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));

                foreach (FileInfo file in Files)    
                {
                    fileName = file.FullName;
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    
                  
                    RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    QryStr = "select Count(*) from oinv t where  t.\"NumAtCard\" = '" + GetElement(xmlConsulta, "//items/document/Numero_InternoSalus") + "'  AND \"CANCELED\" = 'N'";

                    RecSet.DoQuery(QryStr);

                    numatcard = GetElement(xmlConsulta, "//items/document/Numero_InternoSalus");

                    factura_existe = Convert.ToInt32(RecSet.Fields.Item(0).Value);
                    if(factura_existe == 0)
                    {
                        var documentoNumAtCard = Convert.ToString(RecSet.Fields.Item(0).Value);
                        SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);


                        //CAMPOS NATIVOS
                        documento.CardCode = GetElement(xmlConsulta, "//items/document/CardCode"); //factura.CardCode;


                        documento.NumAtCard = GetElement(xmlConsulta, "//items/document/Numero_InternoSalus"); // DocNum


                        documento.DocCurrency = GetElement(xmlConsulta, "//items/document/DocCur");
                        documento.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        documento.DocDueDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDueDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        documento.TaxDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/TaxDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        //CAMPOS DE USUARIO
                        documento.UserFields.Fields.Item("U_SERIE_INTERNA_SALUS").Value = GetElement(xmlConsulta, "//items/document/Serie_InternaSalus");
                        documento.UserFields.Fields.Item("U_SERIE_FACE").Value = GetElement(xmlConsulta, "//items/document/U_FACSERIE");
                        documento.UserFields.Fields.Item("U_NIT").Value = GetElement(xmlConsulta, "//items/document/U_NIT");
                        if (GetElement(xmlConsulta, "//items/document/U_UUID") != "")
                        {
                            documento.UserFields.Fields.Item("U_ESTADO_FACE").Value = "A";
                        }
                        documento.UserFields.Fields.Item("U_NUMERO_DOCUMENTO").Value = GetElement(xmlConsulta, "//items/document/U_FACNUM");
                        documento.UserFields.Fields.Item("U_FIRMA_ELETRONICA").Value = GetElement(xmlConsulta, "//items/document/U_UUID");
                        documento.UserFields.Fields.Item("U_EMPRESA").Value = GetElement(xmlConsulta, "//items/document/U_EMPRESA");
                        documento.UserFields.Fields.Item("U_NOMBRE_SERIE").Value = GetElement(xmlConsulta, "//items/document/NombreSerie");
                        documento.UserFields.Fields.Item("U_TIPO_ENTIDAD").Value = GetElement(xmlConsulta, "//items/document/TipoEntidad");


                        documento.DocType = BoDocumentTypes.dDocument_Items;


                        XmlNodeList nodeList = GetElementNode(xmlConsulta, "//items/document/LineasFacturas/FacturasLineas");
                        foreach (XmlNode nodo in nodeList)
                        {

                            documento.Lines.UserFields.Fields.Item("U_DESC1").Value = nodo["Description"].InnerText;
                            documento.Lines.UserFields.Fields.Item("U_Cant_Servicio").Value = double.Parse(nodo["Quantity"].InnerText);
                            documento.Lines.UserFields.Fields.Item("U_FAMILY").Value = nodo["Family"].InnerText;
                            documento.Lines.UserFields.Fields.Item("U_SUBFAMILY").Value = nodo["SubFamily"].InnerText;
                            documento.Lines.TaxCode = nodo["TaxCode"].InnerText;

                            documento.Lines.PriceAfterVAT = Math.Round(double.Parse(nodo["GTotal"].InnerText), 4);

                            QryStr = "select \"U_NEW_COD\" from \"@ARTICULO_REF_COD\" where  \"U_OLD_COD\" = '" + nodo["ItemCode"].InnerText + "'";
                            RecSet.DoQuery(QryStr);
                            var nuevo_codigo = Convert.ToString(RecSet.Fields.Item(0).Value);

                            if (nuevo_codigo != "")
                            {
                                documento.Lines.ItemCode = nuevo_codigo;
                            }
                            else
                            {
                                documento.Lines.ItemCode = nodo["ItemCode"].InnerText;
                            }



                            documento.Lines.UserFields.Fields.Item("U_HONORARIO").Value = nodo["Honorario"].InnerText;
                            documento.Lines.UserFields.Fields.Item("U_RESPONSABLE").Value = nodo["Responsable"].InnerText;



                            // if(GetElement(xmlConsulta, "//invoice/document/escopago") == "S")
                            //{

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

                        //documento.DocTotal = Math.Round(totalFact, 2);
                        RecSet = null;
                        GC.Collect();
                        int resp = documento.Add();
                        if (resp != 0)
                        {
                            validacion_err = true;
                            Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + B1company.GetLastErrorDescription(), fileName, B1company);
                        }
                        else
                        {
                            File.Move(file.FullName, @"C:\XML\Facturas\Finalizados\" + file.Name);
                            Log.logProc("FACTURA", fileName, B1company);
                            procesados++;
                        }
                    }
                    else
                    {
                        //Factura con este numero interno ya existe
                        validacion_err = true;
                        Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + "Ya se encuentra registrada " + factura_existe + " facturac on este numero interno salus: " + numatcard, fileName, B1company);
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
