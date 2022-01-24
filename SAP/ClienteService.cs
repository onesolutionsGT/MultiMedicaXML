using Modelos;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SAP
{
    public class ClienteService : Interfaces.IClienteService
    {
        public Respuesta addCliente(SAPbobsCOM.ICompany B1company, Modelos.Cliente cliente)
        {
            try
            {
                SAPbobsCOM.BusinessPartners bop = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                bop.CardCode = cliente.CardCode;
                bop.CardName = cliente.CardName;
                bop.UnifiedFederalTaxID = cliente.Rtn;
                bop.FederalTaxID = "NO APLICABLE";
                bop.Phone1 = cliente.Phone1;
                bop.Phone2 = cliente.Phone2;
                bop.Fax = cliente.Fax;
                bop.Currency = cliente.Currency;

                bop.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                foreach (Direccion direccion in cliente.Direcciones)
                {
                    bop.Addresses.AddressName = direccion.AddressName;
                    bop.Addresses.Street = direccion.Street;
                    bop.Addresses.City = direccion.City;
                    bop.Addresses.State = direccion.State;
                    bop.Addresses.Country = direccion.Country;

                    if (direccion.AddressType == AddressCType.FACTURACION)
                    {
                        bop.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                    }
                    else
                    {
                        bop.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                    }
                    bop.Addresses.Add();
                }
                int resp = bop.Add();
                if (resp != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);


                }
                //string sNoFactura;
                //B1company.GetNewObjectCode(out sNoFactura);
                return new Respuesta
                {
                    Mensaje = $"Cliente creado satisfactoriamente",
                    Codigo = cliente.CardCode,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public IBusinessPartners addCliente2(SAPbobsCOM.ICompany B1company)
        {
            try
            {
                SAPbobsCOM.BusinessPartners bop = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                SAPbobsCOM.Documents facturaDeudores = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                SAPbobsCOM.Documents notaCredito = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                string sNewObjCode = "";
                B1company.GetNewObjectCode(out sNewObjCode);
                notaCredito = facturaDeudores;
                //notaCredito.DocEntry = sNewObjCode;
                notaCredito.DocObjectCode = BoObjectTypes.oCreditNotes;
                int iCantidadLineas = notaCredito.Lines.Count;
                int i = 0;
                while (i < iCantidadLineas)
                {
                    notaCredito.Lines.SetCurrentLine(i);
                    notaCredito.Lines.BaseEntry = facturaDeudores.DocEntry;
                    notaCredito.Lines.BaseType = (int)SAPbobsCOM.BoObjectTypes.oInvoices;
                    notaCredito.Lines.BaseLine = i;
                    notaCredito.Lines.Add();
                    i++;
                }
                int respNota = notaCredito.Add();
                /*bop.CardCode = "HU1003";
                bop.CardName = "ABCD";
                bop.FederalTaxID = "000000000000";
                bop.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                int resp = bop.Add();*/
                if (respNota != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);
                }
                else
                {

                }
                return bop;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public Respuesta addClienteXML(ICompany B1company)
        {
            string fileName = "";
            int procesados = 0;
            bool validacion_err = false;

            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Clientes\Cola"); //Assuming Test is your Folder

                FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files


                foreach (FileInfo file in Files)
                {

                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    fileName = file.FullName;

                    SAPbobsCOM.BusinessPartners bop = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    bop.CardCode = GetElement(xmlConsulta, "//cliente/document/cardcode");
                    bop.CardName = GetElement(xmlConsulta, "//cliente/document/cardname");
                    bop.CardForeignName = GetElement(xmlConsulta, "//cliente/document/cardfname");
                    bop.UnifiedFederalTaxID = "";

                    bop.GroupCode = int.Parse(GetElement(xmlConsulta, "//cliente/document/groupcode"));
                    bop.FederalTaxID = "000000000000";
                    bop.Phone1 = GetElement(xmlConsulta, "//cliente/document/phone1");// cliente.Phone1;
                    bop.Cellular = GetElement(xmlConsulta, "//cliente/document/mobilphone");
                    bop.Fax = GetElement(xmlConsulta, "//cliente/document/fax");
                    bop.EmailAddress = GetElement(xmlConsulta, "//cliente/document/e_mail");
                    bop.Currency = GetElement(xmlConsulta, "//cliente/document/currency");
                    bop.Password = GetElement(xmlConsulta, "//cliente/document/password");


                    bop.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                    Cliente cliente = new Cliente();

                    XmlNodeList nodeList = GetElementNode(xmlConsulta, "//cliente/document/document_line/line");
                    foreach (XmlNode nodo in nodeList)
                    {
                        bop.Addresses.AddressName = "DIRECCION"; //nodo["address"].InnerText;//direccion.AddressName;
                        bop.Addresses.Street = nodo["address"].InnerText;//nodo["street"].InnerText; //direccion.Street;                       
                        bop.Addresses.City = nodo["city"].InnerText; //direccion.City;
                        bop.Addresses.State = nodo["state"].InnerText;// direccion.State;
                        bop.Addresses.Country = nodo["country"].InnerText;// direccion.Country;
                        bop.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                        bop.Addresses.Add();
                    }

                    int resp = bop.Add();
                    if (resp != 0)
                    {
                        validacion_err = true;
                        Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + B1company.GetLastErrorDescription(), fileName, B1company);
                    }
                    else
                    {
                        File.Move(file.FullName, @"C:\XML\Clientes\Finalizados\" + file.Name);
                        procesados++;
                        Log.logProc("CLIENTE", fileName, B1company);
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
