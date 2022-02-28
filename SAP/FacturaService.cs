﻿using Modelos;
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
                    string QryStr = "select Count(*) from oinv t where  t.\"NumAtCard\" = '" + GetElement(xmlConsulta, "//items/document/Numero_InternoSalus") + "'";
                    RecSet.DoQuery(QryStr);

                    var documentoNumAtCard = Convert.ToString(RecSet.Fields.Item(0).Value);
                    SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);


                    //CAMPOS NATIVOS
                    documento.CardCode = GetElement(xmlConsulta, "//items/document/CardCode"); //factura.CardCode;
                    documento.NumAtCard = GetElement(xmlConsulta, "//items/document/Numero_InternoSalus");
                    documento.DocCurrency = GetElement(xmlConsulta, "//items/document/DocCur");
                    documento.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    documento.DocDueDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDueDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    documento.TaxDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/TaxDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);                   

                    //CAMPOS DE USUARIO
                    documento.UserFields.Fields.Item("U_SERIE_INTERNA_SALUS").Value = GetElement(xmlConsulta, "//items/document/Serie_InternaSalus");
                    documento.UserFields.Fields.Item("U_SERIE_FACE").Value = GetElement(xmlConsulta, "//items/document/U_FACSERIE");
                    documento.UserFields.Fields.Item("U_NIT").Value = GetElement(xmlConsulta, "//items/document/U_NIT");
                    documento.UserFields.Fields.Item("U_NUMERO_DOCUMENTO").Value = GetElement(xmlConsulta, "//items/document/U_FACNUM");
                    documento.UserFields.Fields.Item("U_FIRMA_ELETRONICA").Value = GetElement(xmlConsulta, "//items/document/U_UUID");
                    documento.UserFields.Fields.Item("U_EMPRESA").Value = GetElement(xmlConsulta, "//items/document/U_EMPRESA");
                    documento.UserFields.Fields.Item("U_NOMBRE_SERIE").Value = GetElement(xmlConsulta, "//items/document/NombreSerie");
                    documento.UserFields.Fields.Item("U_TIPO_ENTIDAD").Value = GetElement(xmlConsulta, "//items/document/TipoEntidad");




                    //documento.CardName = GetElement(xmlConsulta, "//invoice/document/cardname");  

                    //documento.Series = int.Parse(GetElement(xmlConsulta, "//invoice/document/series"));
                    //documento.DiscountPercent = double.Parse(GetElement(xmlConsulta, "//invoice/document/discprcnt"));
                    //Campos de usuario.
                    //documento.UserFields.Fields.Item("U_COD_PACIENTE").Value = GetElement(xmlConsulta, "//invoice/document/u_paciente");
                    //documento.UserFields.Fields.Item("U_Numero").Value = GetElement(xmlConsulta, "//invoice/document/u_docnum");
                    //documento.UserFields.Fields.Item("U_COD_MED").Value = GetElement(xmlConsulta, "//invoice/document/u_cod_med");
                    //documento.UserFields.Fields.Item("U_PROCEDE").Value = GetElement(xmlConsulta, "//invoice/document/u_procede");
                    //documento.UserFields.Fields.Item("U_SUB_PROCEDENCIA").Value = GetElement(xmlConsulta, "//invoice/document/u_sub_procedencia");
                    //documento.UserFields.Fields.Item("U_F_FACTURA").Value = GetElement(xmlConsulta, "//invoice/document/u_f_factura");
                    //documento.UserFields.Fields.Item("U_NO_CAJA").Value = GetElement(xmlConsulta, "//invoice/document/no_caja");
                    //documento.UserFields.Fields.Item("U_NO_TRANSACCION").Value = GetElement(xmlConsulta, "//invoice/document/no_transaccion");
                    //documento.UserFields.Fields.Item("U_EMP_AUTORIZA_DESC").Value = GetElement(xmlConsulta, "//invoice/document/emp_autoriza_desc");
                    //documento.UserFields.Fields.Item("U_HOJA_REFERENCIA").Value = GetElement(xmlConsulta, "//invoice/document/hoja_referencia");
                    //documento.UserFields.Fields.Item("U_COD_PROMO").Value = GetElement(xmlConsulta, "//invoice/document/cod_promo");
                    //documento.UserFields.Fields.Item("U_COD_EMPLEADO").Value = GetElement(xmlConsulta, "//invoice/document/cod_empleado");
                    //documento.UserFields.Fields.Item("U_COD_CLIENTE_CC").Value = GetElement(xmlConsulta, "//invoice/document/cod_cliente_cc");
                    //documento.UserFields.Fields.Item("U_COD_TIPOCREDITO").Value = GetElement(xmlConsulta, "//invoice/document/cod_tipocredito");
                    //documento.UserFields.Fields.Item("U_NO_PAGOS").Value = GetElement(xmlConsulta, "//invoice/document/no_pagos");
                    //documento.UserFields.Fields.Item("U_ORIGEN_GENERACION").Value = GetElement(xmlConsulta, "//invoice/document/origen_generacion");
                    //documento.UserFields.Fields.Item("U_COD_CAJERO").Value = GetElement(xmlConsulta, "//invoice/document/cod_cajero");
                    //documento.UserFields.Fields.Item("U_COD_USUARIOGENERA").Value = GetElement(xmlConsulta, "//invoice/document/cod_usuariogenera");
                    //documento.UserFields.Fields.Item("U_APLICACREDITO").Value = GetElement(xmlConsulta, "//invoice/document/aplicacredito");
                    //documento.UserFields.Fields.Item("U_PORCENTAJEIVA").Value = GetElement(xmlConsulta, "//invoice/document/porcentajeiva");
                    //documento.UserFields.Fields.Item("U_ESCOPAGO").Value = GetElement(xmlConsulta, "//invoice/document/escopago");
                    //documento.UserFields.Fields.Item("U_PORCENTAJECOBERTURA").Value = GetElement(xmlConsulta, "//invoice/document/porcentajecobertura");
                    //documento.UserFields.Fields.Item("U_CODIGOCOBERTURA").Value = GetElement(xmlConsulta, "//invoice/document/codigocobertura");
                    //documento.UserFields.Fields.Item("U_NOAUTORIZACION").Value = GetElement(xmlConsulta, "//invoice/document/Autoriza_aseguradora");
                    //documento.UserFields.Fields.Item("U_OBSERVACIONES_TRANSACCION").Value = GetElement(xmlConsulta, "//invoice/document/observaciones_transaccion");



                    /*if (GetElement(xmlConsulta, "//items/document/DocType") != "S")
                    {
                        Console.WriteLine("es de Items la factura");
                        throw new Exception("La factura no es de servicios");  
                    }*/

                    documento.DocType = BoDocumentTypes.dDocument_Items;

                    // if (factura.Rate > 0) documento.DocRate = factura.Rate;

                    XmlNodeList nodeList = GetElementNode(xmlConsulta, "//items/document/LineasFacturas/FacturasLineas");
                    foreach (XmlNode nodo in nodeList)
                    {
                        
                        documento.Lines.UserFields.Fields.Item("U_DESC1").Value = nodo["Description"].InnerText;
                        documento.Lines.UserFields.Fields.Item("U_Cant_Servicio").Value =  double.Parse(nodo["Quantity"].InnerText);
                        documento.Lines.UserFields.Fields.Item("U_FAMILY").Value = nodo["Family"].InnerText;
                        documento.Lines.UserFields.Fields.Item("U_SUBFAMILY").Value = nodo["SubFamily"].InnerText;
                        documento.Lines.TaxCode = nodo["TaxCode"].InnerText;

                        //documento.Lines.GrossTotal = double.Parse(nodo["PriceAfVAT"].InnerText) - double.Parse(nodo["GTotal"].InnerText);

                        documento.Lines.PriceAfterVAT = Math.Round(double.Parse(nodo["GTotal"].InnerText),4);
                        //documento.Lines.RowTotalFC = double.Parse(nodo["GTotal"].InnerText);


                        //SETEO DE LOS CODIGOS DE CUENTA DEPENDIENDO DEL SERVICIO
                        /*switch (nodo["Description"].InnerText.ToUpper())
                        {
                            case "SERVICIOS DE HOSPITAL":
                                documento.Lines.AccountCode = "4112";
                                break;
                            case "MEDICAMENTOS":
                                documento.Lines.AccountCode = "411201";
                                break;
                            case "MATERIALES Y SUMINISTROS":
                                documento.Lines.AccountCode = "411202";
                                break;
                            case "USO DE EQUIPO HOSPITALARIO":
                                documento.Lines.AccountCode = "411203";
                                break;
                            case "ESTERILIZACIÓN DE EQUIPO":
                                documento.Lines.AccountCode = "411204";
                                break;
                            case "EXTRAORDINARIOS":
                                documento.Lines.AccountCode = "411205";
                                break;
                            case "PRONTO PAGO":
                                documento.Lines.AccountCode = "411206";
                                break;
                            case "COMISIONES COBRADAS POR EMISORES DE TARJETAS":
                                documento.Lines.AccountCode = "411207";
                                break;
                            case "SERVICIOS HOSPITALARIOS":
                                documento.Lines.AccountCode = "411208";
                                break;
                            case "LABORATORIOS Y DIAGNÓSTICO":
                                documento.Lines.AccountCode = "4113";
                                break;
                            case "LABORATORIO":
                                documento.Lines.AccountCode = "411301";
                                break;
                            case "ANGIO RESONANCIA":
                                documento.Lines.AccountCode = "411302";
                                break;
                            case "ANGIO TOMOGRAFIA":
                                documento.Lines.AccountCode = "411303";
                                break;
                            case "CARDIOLOGIA":
                                documento.Lines.AccountCode = "411304";
                                break;
                            case "DENSITOMETRÍA ÓSEA":
                                documento.Lines.AccountCode = "411305";
                                break;
                            case "DOPPLER COLOR":
                                documento.Lines.AccountCode = "411306";
                                break;
                            case "MAMOGRAFÍA":
                                documento.Lines.AccountCode = "411307";
                                break;
                            case "NEUROFISIOLOGIA":
                                documento.Lines.AccountCode = "411308";
                                break;
                            case "RAYOS X":
                                documento.Lines.AccountCode = "411309";
                                break;
                            case "RESONANCIA MAGNETICA":
                                documento.Lines.AccountCode = "411310";
                                break;
                            case "TOMOGRAFIA COMPUTADA":
                                documento.Lines.AccountCode = "411311";
                                break;
                            case "ULTRASONIDO":
                                documento.Lines.AccountCode = "411312";
                                break;
                            case "SERVICIOS DE TERCEROS":
                                documento.Lines.AccountCode = "4114";
                                break;
                            case "EQUIPO ESPECIAL":
                                documento.Lines.AccountCode = "411401";
                                break;
                            case "USO DE EQUIPO":
                                documento.Lines.AccountCode = "411402";
                                break;
                            case "AMBULANCIA":
                                documento.Lines.AccountCode = "411403";
                                break;
                            case "PATOLOGIA":
                                documento.Lines.AccountCode = "411404";
                                break;
                            case "UNIDAD DE SANGRE":
                                documento.Lines.AccountCode = "411405";
                                break;
                            case "TERAPIA RESPIRATORIA":
                                documento.Lines.AccountCode = "411406";
                                break;
                            case "FISIOTERAPIA":
                                documento.Lines.AccountCode = "411407";
                                break;
                            case "USO DE EQUIPO ESPECIAL":
                                documento.Lines.AccountCode = "411408";
                                break;
                            case "EQUIPO MEDICO(suplido)":
                                documento.Lines.AccountCode = "411409";
                                break;
                            case "MATERIAL ESPECIAL":
                                documento.Lines.AccountCode = "411410";
                                break;
                            case "HONORARIOS MEDICOS":
                                documento.Lines.AccountCode = "4115";
                                break;
                            case "HONORARIOS MEDICOS":
                                documento.Lines.AccountCode = "411501";
                                break;
                            case "HONORARIOS MEDICOS CIRUJANOS":
                                documento.Lines.AccountCode = "411502";
                                break;
                            case "HONORARIOS MEDICOS ANESTESIOLOGOS":
                                documento.Lines.AccountCode = "411503";
                                break;
                            default:
                                break;

                        }*/

                        //documento.Lines.DiscountPercent = double.Parse(nodo["DiscPrcnt"].InnerText);

                        documento.Lines.ItemCode = nodo["ItemCode"].InnerText;


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
