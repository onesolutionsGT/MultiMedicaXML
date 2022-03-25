using Modelos;
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
    public class PagoService : Interfaces.IPagoService
    {
        public Respuesta addPago(ICompany B1company, Modelos.Pago pago)
        {
            try
            {
                SAPbobsCOM.Payments DocumentoPago = (SAPbobsCOM.Payments)B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                double montoAplicado = 0.0;
                DocumentoPago.DocDate = pago.DocDate;
                DocumentoPago.CardCode = pago.CardCode;
                DocumentoPago.Remarks = pago.Comentario;
                DocumentoPago.DocCurrency = pago.Currency;

                if (pago.Rate > 0) DocumentoPago.DocRate = pago.Rate;
                DocumentoPago.Series = 17;


                if (String.IsNullOrEmpty(pago.PagoTransferencia.TransferDate.ToString()))
                {
                    DocumentoPago.TransferDate = pago.PagoTransferencia.TransferDate;
                    DocumentoPago.TransferReference = pago.PagoTransferencia.TransferReference;
                    DocumentoPago.TransferAccount = pago.PagoTransferencia.TransferAccount;
                    DocumentoPago.TransferSum = pago.PagoTransferencia.TransferSum;
                    montoAplicado += pago.PagoTransferencia.TransferSum;
                }

                foreach (Modelos.Cheque cheque in pago.PagoCheque)
                {
                    DocumentoPago.Checks.BankCode = cheque.BankCode;
                    DocumentoPago.Checks.CheckNumber = cheque.CheckNumber;
                    DocumentoPago.Checks.AccounttNum = cheque.AccounttNum;
                    DocumentoPago.Checks.DueDate = cheque.DueDate;
                    DocumentoPago.Checks.CheckSum = cheque.CheckSum;
                    montoAplicado += cheque.CheckSum;
                    DocumentoPago.Checks.Add();
                }

                foreach (Modelos.Tarjeta tarjeta in pago.PagoTarjeta)
                {
                    DocumentoPago.CreditCards.CreditCard = tarjeta.CreditCard;
                    DocumentoPago.CreditCards.CreditCardNumber = tarjeta.CreditCardNumber;
                    DocumentoPago.CreditCards.CreditSum = tarjeta.CreditSum;
                    DocumentoPago.CreditCards.VoucherNum = tarjeta.VoucherNum;
                    DocumentoPago.CreditCards.CardValidUntil = tarjeta.CardValidUntil;
                    DocumentoPago.CreditCards.Add();
                    montoAplicado += tarjeta.CreditSum;
                }


                DocumentoPago.Invoices.DocEntry = Convert.ToInt32(pago.DocEntry);
                DocumentoPago.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                DocumentoPago.Invoices.SumApplied = montoAplicado;
                DocumentoPago.Invoices.AppliedFC = montoAplicado;

                int resp = DocumentoPago.Add();
                if (resp != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);


                }
                string sNumeroPago;
                B1company.GetNewObjectCode(out sNumeroPago);



                return new Respuesta
                {
                    Mensaje = $"Pago aplicado correctamente",
                    Codigo = sNumeroPago,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Respuesta addPagoXML(ICompany B1company)
        {
            string fileName = "";
            int procesados = 0;
            int factura_DocEntry = 0;
            int factura_DocNum = 0;
            bool validacion_err = false;
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Pagos\Cola");

                FileInfo[] Files = d.GetFiles("*.xml");
                foreach (FileInfo file in Files)
                {
                    fileName = file.FullName;
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    SAPbobsCOM.Recordset RecSet = null;
                    SAPbobsCOM.Payments DocumentoPago = (SAPbobsCOM.Payments)B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                    double montoAplicado = 0.0;
                    DocumentoPago.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DocumentoPago.CardCode = GetElement(xmlConsulta, "//items/document/cardcode"); //pago.CardCode;
                    DocumentoPago.Remarks = GetElement(xmlConsulta, "//items/document/Remarks");//pago.Comentario;
                    if(GetElement(xmlConsulta, "//items/document/DocCur") == "Q")
                    {
                        DocumentoPago.DocCurrency = "QTZ";
                    }
                    else
                    {
                        DocumentoPago.DocCurrency = GetElement(xmlConsulta, "//items/document/DocCur");
                    }
                    
                    //DocumentoPago.Series =int.Parse( GetElement(xmlConsulta, "//items/document/series"));

                    if (GetElement(xmlConsulta, "//items/document/cashaccount") != "N")
                    {
                        DocumentoPago.CashAccount = "111102";
                        DocumentoPago.CashSum = double.Parse(GetElement(xmlConsulta, "//items/document/cashsum"));
                        montoAplicado += double.Parse(GetElement(xmlConsulta, "//items/document/cashsum"));
                    }


                    if (!String.IsNullOrEmpty(GetElement(xmlConsulta, "//items/document/TrsfrRef")))
                    {
                        //DocumentoPago.TransferDate = pago.PagoTransferencia.TransferDate;
                        DocumentoPago.TransferReference = GetElement(xmlConsulta, "//items/document/TrsfrRef"); //pago.PagoTransferencia.TransferReference;
                        //DocumentoPago.TransferAccount = GetElement(xmlConsulta, "//items/document/Trsfraccount");// pago.PagoTransferencia.TransferAccount;
                        DocumentoPago.TransferSum = double.Parse(GetElement(xmlConsulta, "//items/document/TrsfrSum")); //pago.PagoTransferencia.TransferSum;
                        montoAplicado += double.Parse(GetElement(xmlConsulta, "//items/document/TrsfrSum")); //pago.PagoTransferencia.TransferSum;
                    }


                    XmlNodeList nodeList = GetElementNode(xmlConsulta, "//items/document/document_lines/Pay");

                    foreach (XmlNode nodo in nodeList)
                    {
                        if (!String.IsNullOrEmpty(nodo["CheckNumber"].InnerText))
                        {
                            DocumentoPago.Checks.BankCode = nodo["BankCode"].InnerText; //cheque.BankCode;
                            if (nodo["CheckNumber"].InnerText.Length >= 10)
                                DocumentoPago.Checks.CheckNumber = int.Parse(nodo["CheckNumber"].InnerText.Substring(0, 9)); //cheque.CheckNumber;
                            else
                                DocumentoPago.Checks.CheckNumber = Convert.ToInt32(nodo["CheckNumber"].InnerText.ToString()); //cheque.CheckNumber;

                            //DocumentoPago.Checks.AccounttNum = nodo["accounttnum"].InnerText; //cheque.AccounttNum;
                            //DocumentoPago.Checks.CheckAccount = nodo["checkaccount"].InnerText;
                            //DocumentoPago.Checks.DueDate = DateTime.ParseExact(nodo["checkdate"].InnerText, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            DocumentoPago.Checks.CheckSum = double.Parse(nodo["CheckSum"].InnerText);//cheque.CheckSum;
                            DocumentoPago.Checks.Details = nodo["CheckNumber"].InnerText;
                            //montoAplicado += double.Parse(nodo["CheckSum"].InnerText);
                            DocumentoPago.Checks.Add();
                        }
                    }

                    foreach (XmlNode nodo in nodeList)
                    {
                        if (!String.IsNullOrEmpty(nodo["CreditCard"].InnerText))
                        {
                            DocumentoPago.CreditCards.CreditCard = int.Parse(nodo["CreditCard"].InnerText);//tarjeta.CreditCard;
                            DocumentoPago.CreditCards.CreditCardNumber = nodo["CreditCardNumber"].InnerText;//tarjeta.CreditCardNumber;
                            DocumentoPago.CreditCards.CreditSum = double.Parse(nodo["CreditSum"].InnerText);//tarjeta.CreditSum;
                            DocumentoPago.CreditCards.VoucherNum = nodo["VoucherNum"].InnerText;// tarjeta.VoucherNum;
                            DocumentoPago.CreditCards.CardValidUntil = DateTime.ParseExact(nodo["CardValidUntil"].InnerText, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);//tarjeta.CardValidUntil;
                            DocumentoPago.CreditCards.Add();
                            //montoAplicado += double.Parse(nodo["CreditSum"].InnerText);//tarjeta.CreditSum;
                        }
                    }


                    XmlNodeList listNode = GetElementNode(xmlConsulta, "//items/document/document_lines/Line");

                    foreach (XmlNode nodo in listNode)
                    {
                        if (!String.IsNullOrEmpty(nodo["SumApplied"].InnerText))
                        {
                            montoAplicado += double.Parse(nodo["SumApplied"].InnerText);//tarjeta.CreditSum;
                        }
                    }
                    factura_DocEntry = 0;
                    factura_DocNum = 0;
                    RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    string QryStr = "select top 1 T.\"DocEntry\", T.\"DocNum\" from oinv t where t.\"NumAtCard\" = '" + GetElement(xmlConsulta, "//items/document/document_lines/Line/Docnum") + "'  AND \"CANCELED\" = 'N'";
                    RecSet.DoQuery(QryStr);
                    factura_DocEntry = Convert.ToInt32(RecSet.Fields.Item(0).Value);
                    factura_DocNum = Convert.ToInt32(RecSet.Fields.Item(1).Value);
                    if (RecSet.RecordCount == 0)
                    {
                        File.Move(file.FullName, @"C:\XML\Pagos\Sin_factura\" + file.Name);
                        Log.logProc("PAGO", fileName, B1company);
                        validacion_err = true;
                        Log.logError("No hay factura asociada, ("+ GetElement(xmlConsulta, "//items/document/document_lines/Line/Docnum") +")", fileName, B1company);
                    }
                    else
                    {
                        DocumentoPago.Invoices.DocEntry = factura_DocEntry;
                        DocumentoPago.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                        DocumentoPago.Invoices.SumApplied = double.Parse(GetElement(xmlConsulta, "//items/document/document_lines/Line/SumApplied"));
                        DocumentoPago.Invoices.AppliedFC = double.Parse(GetElement(xmlConsulta, "//items/document/document_lines/Line/SumApplied"));

                        int resp = DocumentoPago.Add();
                        if (resp != 0)
                        {
                            validacion_err = true;
                            Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + B1company.GetLastErrorDescription(), fileName, B1company);
                        }
                        else
                        {
                            File.Move(file.FullName, @"C:\XML\Pagos\Finalizados\" + file.Name);
                            Log.logProc("PAGO", fileName, B1company);
                            procesados++;
                        }
                    }
                }

                if (validacion_err)
                {
                    return new Respuesta
                    {
                        Mensaje = $"Ocurrió un error, revisar SAP para mas detalles.",
                    };
                }
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
