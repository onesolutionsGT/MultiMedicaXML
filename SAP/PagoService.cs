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

                foreach(Modelos.Cheque cheque in pago.PagoCheque)
                {
                    DocumentoPago.Checks.BankCode = cheque.BankCode;
                    DocumentoPago.Checks.CheckNumber = cheque.CheckNumber;
                    DocumentoPago.Checks.AccounttNum = cheque.AccounttNum;
                    DocumentoPago.Checks.DueDate = cheque.DueDate;
                    DocumentoPago.Checks.CheckSum = cheque.CheckSum;
                    montoAplicado += cheque.CheckSum;
                    DocumentoPago.Checks.Add();
                }

                foreach(Modelos.Tarjeta tarjeta in pago.PagoTarjeta)
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
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Pagos\Cola");

                FileInfo[] Files = d.GetFiles("*.xml");
                foreach (FileInfo file in Files)
                {
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    SAPbobsCOM.Recordset RecSet = null;
                    SAPbobsCOM.Payments DocumentoPago = (SAPbobsCOM.Payments)B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                    double montoAplicado = 0.0;
                    DocumentoPago.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//body/document/docdate"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);                    
                    DocumentoPago.CardCode = GetElement(xmlConsulta, "//body/document/cardcode"); //pago.CardCode;
                    DocumentoPago.Remarks = GetElement(xmlConsulta, "//body/document/remarks");//pago.Comentario;
                    DocumentoPago.DocCurrency = GetElement(xmlConsulta, "//body/document/doccurrency");                    
                    DocumentoPago.Series =int.Parse( GetElement(xmlConsulta, "//body/document/series"));

                    if(!string.IsNullOrEmpty(GetElement(xmlConsulta, "//body/document/cashaccount")))
                    {
                        DocumentoPago.CashAccount = GetElement(xmlConsulta, "//body/document/cashaccount");
                        DocumentoPago.CashSum = double.Parse(GetElement(xmlConsulta, "//body/document/cashsum"));
                        montoAplicado+= double.Parse(GetElement(xmlConsulta, "//body/document/cashsum"));
                    }


                    if (!String.IsNullOrEmpty(GetElement(xmlConsulta, "//body/document/transferaccount")))
                    {
                        //DocumentoPago.TransferDate = pago.PagoTransferencia.TransferDate;
                        DocumentoPago.TransferReference = GetElement(xmlConsulta, "//body/document/transferreference"); //pago.PagoTransferencia.TransferReference;
                        DocumentoPago.TransferAccount = GetElement(xmlConsulta, "//body/document/transferaccount");// pago.PagoTransferencia.TransferAccount;
                        DocumentoPago.TransferSum = double.Parse( GetElement(xmlConsulta, "//body/document/transfersum")); //pago.PagoTransferencia.TransferSum;
                        montoAplicado += double.Parse(GetElement(xmlConsulta, "//body/document/transfersum")); //pago.PagoTransferencia.TransferSum;
                    }


                    XmlNodeList nodeList = GetElementNode(xmlConsulta, "//body/document/document_lines/pay");

                    foreach (XmlNode nodo in nodeList)
                    {
                        if (!String.IsNullOrEmpty(nodo["checknumber"].InnerText))
                        {
                            DocumentoPago.Checks.BankCode = nodo["bankcode"].InnerText; //cheque.BankCode;
                            if(nodo["checknumber"].InnerText.Length >=10)
                            DocumentoPago.Checks.CheckNumber = int.Parse( nodo["checknumber"].InnerText.Substring(0,9)); //cheque.CheckNumber;
                            else                            
                                DocumentoPago.Checks.CheckNumber = Convert.ToInt32(nodo["checknumber"].InnerText.ToString()); //cheque.CheckNumber;

                            DocumentoPago.Checks.AccounttNum = nodo["accounttnum"].InnerText; //cheque.AccounttNum;
                            DocumentoPago.Checks.CheckAccount = nodo["checkaccount"].InnerText;
                            DocumentoPago.Checks.DueDate = DateTime.ParseExact(nodo["checkdate"].InnerText, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            DocumentoPago.Checks.CheckSum = double.Parse(nodo["checksum"].InnerText);//cheque.CheckSum;
                            DocumentoPago.Checks.Details = nodo["checknumber"].InnerText;
                            montoAplicado += double.Parse(nodo["checksum"].InnerText);
                            DocumentoPago.Checks.Add();
                        }                        
                    }

                    foreach (XmlNode nodo in nodeList)
                    {
                        if (!String.IsNullOrEmpty(nodo["creditcard"].InnerText))
                        {
                            DocumentoPago.CreditCards.CreditCard = int.Parse(nodo["creditcard"].InnerText);//tarjeta.CreditCard;
                            DocumentoPago.CreditCards.CreditCardNumber = nodo["creditcardnumber"].InnerText;//tarjeta.CreditCardNumber;
                            DocumentoPago.CreditCards.CreditSum = double.Parse(nodo["creditsum"].InnerText);//tarjeta.CreditSum;
                            DocumentoPago.CreditCards.VoucherNum = nodo["vouchernum"].InnerText;// tarjeta.VoucherNum;
                            DocumentoPago.CreditCards.CardValidUntil = DateTime.ParseExact(nodo["cardvaliduntil"].InnerText, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);//tarjeta.CardValidUntil;
                            DocumentoPago.CreditCards.Add();
                            montoAplicado += double.Parse(nodo["creditsum"].InnerText);//tarjeta.CreditSum;
                        }
                    }

                    RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    string QryStr = "select top 1 T.\"DocEntry\", T.\"DocNum\" from oinv t where t.\"NumAtCard\" = '"+GetElement(xmlConsulta, "//body/document/document_lines/line/serie") +" - "+ GetElement(xmlConsulta, "//body/document/document_lines/line/docnum")+"'";
                    RecSet.DoQuery(QryStr);                   

                    var documento = Convert.ToString(RecSet.Fields.Item(0).Value);
                    DocumentoPago.Invoices.DocEntry = Convert.ToInt32(documento);
                    DocumentoPago.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                    DocumentoPago.Invoices.SumApplied = montoAplicado;
                    DocumentoPago.Invoices.AppliedFC = montoAplicado;

                    int resp = DocumentoPago.Add();
                    if (resp != 0)
                    {
                        string errMsg = B1company.GetLastErrorDescription();
                        int ErrNo = B1company.GetLastErrorCode();
                        //throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);
                        using (StreamWriter writer = File.AppendText(@"C:\XML\Pagos\PagosError\logError.txt"))
                        {
                            // var mensaje = data["Mensajes"][0];
                            writer.WriteLine(ErrNo.ToString() + " - " + errMsg + " -- " + DateTime.Now.ToString() + " Xml pago" + "-- " + file.FullName);
                            //File.Move(file.FullName, @"C:\XML\Pagos\PagosError\" + file.Name);
                            writer.Close();
                        }

                    }
                    else
                    {
                        //System.Runtime.InteropServices.Marshal.ReleaseComObject(RecSet);
                        // RecSet = null;
                        // GC.Collect();
                        File.Move(file.FullName, @"C:\XML\Pagos\Finalizados\" + file.Name);
                    }  
                    
                }
               
                return new Respuesta
                {
                    Mensaje = $"Pago aplicado correctamente",
                    Codigo = "0",
                };
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = File.AppendText(@"C:\XML\Pagos\PagosError\logError.txt"))
                {
                    // var mensaje = data["Mensajes"][0];
                    writer.WriteLine(ex.Message +" -- Error no controlado -- " + DateTime.Now.ToString());                    
                    writer.Close();
                }
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
