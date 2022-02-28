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
    public class SalidaService : ISalidaService
    {

        public Respuesta addSalidaXML(ICompany B1company)
        {
            string fileName = "";
            int procesados = 0;
            bool validacion_err = false;
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Salidas\Cola"); //Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
                foreach (FileInfo file in Files)
                {
                    fileName = file.FullName;
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));

                    SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                    documento.DocDate = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);                

                    //documento.CardCode = GetElement(xmlConsulta, "//items/document/CardCode").ToString();
                    //Campos de usuario.
                    //documento.UserFields.Fields.Item("U_DATE_SALUS").Value = DateTime.ParseExact(GetElement(xmlConsulta, "//items/document/DocDateSalus"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    //documento.UserFields.Fields.Item("U_tipoReference1").Value = GetElement(xmlConsulta, "//items/document/tipoReference1");
                    //documento.UserFields.Fields.Item("U_Reference1").Value = GetElement(xmlConsulta, "//items/document/Reference1");
                    //documento.UserFields.Fields.Item("U_Expediente").Value = GetElement(xmlConsulta, "//items/document/Expediente");
                    //documento.UserFields.Fields.Item("U_ContactId").Value = GetElement(xmlConsulta, "//items/document/ContactId");
                    //documento.UserFields.Fields.Item("U_Eliminar").Value = GetElement(xmlConsulta, "//items/document/Eliminar");
                    //documento.UserFields.Fields.Item("U_Linea_documento").Value = GetElement(xmlConsulta, "//items/document/Linea_documento");
                    XmlNodeList nodeList = GetElementNode(xmlConsulta, "//items/document/document_lines");
                    foreach (XmlNode nodo in nodeList)
                    {  
                        documento.Lines.ItemCode = nodo["ItemCode"].InnerText;//detalle.ItemCode;
                        documento.Lines.Quantity = double.Parse(nodo["Quantity"].InnerText);
                        //documento.Lines.Price = double.Parse(nodo["Price"].InnerText);
                        //CAMPOS DE USUARIO
                        //documento.Lines.UserFields.Fields.Item("U_BATCH") = nodo["U_BATCH"].InnerText;
                        //documento.Lines.UserFields.Fields.Item("U_QuantityBatch") = double.Parse(nodo["QuantityBatch"].InnerText);
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
                        File.Move(file.FullName, @"C:\XML\Salidas\Finalizados\" + file.Name);
                        Log.logProc("PAGO", fileName, B1company);
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
