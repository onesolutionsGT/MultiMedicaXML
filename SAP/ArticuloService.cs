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
    public class ArticuloService : IArticuloService
    {

        public Respuesta addArticulo(ICompany B1company, Articulo articulo)
        {
            try
            {
                B1company.StartTransaction();
                SAPbobsCOM.UserTable oUserTable = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataCollection oChildren = null;
                SAPbobsCOM.GeneralDataParams oGeneralParams = null; 

                SAPbobsCOM.Documents documento = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                oUserTable = B1company.UserTables.Item("ARTICULO_REF_COD");
                int iRet = 0;

                oUserTable.Name = articulo.Name;
                oUserTable.UserFields.Fields.Item("U_OLD_COD").Value = articulo.Old_code;
                oUserTable.UserFields.Fields.Item("U_NEW_COD").Value = articulo.New_code;
                oUserTable.UserFields.Fields.Item("U_CODEBARS").Value = articulo.Codebars;
                oUserTable.UserFields.Fields.Item("U_FAMILY").Value = articulo.Family;
                oUserTable.UserFields.Fields.Item("U_SUBFAMILY").Value = articulo.Subfamily;

                //VALIDACION DE EXISTENTE REGISTRO 



                iRet = oUserTable.Add();


                //oCompany.StartTransaction();
                //oUserTable.Code = code;
                //oUserTable.Name = name;

                if (iRet != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error " + ErrNo + " Codigo " + errMsg);


                }
                string sNoRegistro;
                B1company.GetNewObjectCode(out sNoRegistro);

                B1company.EndTransaction(BoWfTransOpt.wf_Commit);
                return new Respuesta
                {
                    Mensaje = $"Referencia creada satisfactoriamente",
                    Codigo = sNoRegistro,
                };
            }
            catch (Exception ex)
            {
                B1company.EndTransaction(BoWfTransOpt.wf_RollBack);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public Respuesta addArticuloXML(ICompany B1company)
        {
            string fileName = "";
            int procesados = 0;
            int contador = 0;
            bool validacion_err = false;
            SAPbobsCOM.UserTable oUserTable = null;
            try
            {
                B1company.StartTransaction();
                DirectoryInfo d = new DirectoryInfo(@"C:\XML\Articulos\Cola"); //Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
                foreach (FileInfo file in Files)
                {

                    contador += 1;
                    fileName = file.FullName;
                    XmlDocument xmlConsulta = new XmlDocument();
                    xmlConsulta.LoadXml(System.IO.File.ReadAllText(file.FullName));
                    SAPbobsCOM.Recordset RecSet = null;

                    RecSet = ((SAPbobsCOM.Recordset)(B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    string QryStr = "select Count(*) from \"@ARTICULO_REF_COD\" t where  t.\"U_OLD_COD\" = '" + GetElement(xmlConsulta, "//item/document/itemcode") + "'";
                    RecSet.DoQuery(QryStr);

                    var documenREF = Convert.ToInt32(RecSet.Fields.Item(0).Value);
                    RecSet = null;
                    GC.Collect();
                    if(documenREF == 0)
                    {
                        oUserTable = B1company.UserTables.Item("ARTICULO_REF_COD");

                        oUserTable.Code = GetElement(xmlConsulta, "//item/document/itemcode");
                        oUserTable.Name = "(" + GetElement(xmlConsulta, "//item/document/itemcode") + ")" + GetElement(xmlConsulta, "//item/document/itemname");
                        oUserTable.UserFields.Fields.Item("U_OLD_COD").Value = GetElement(xmlConsulta, "//item/document/itemcode");
                        oUserTable.UserFields.Fields.Item("U_NEW_COD").Value = GetElement(xmlConsulta, "//item/document/itmsgrpcod");
                        oUserTable.UserFields.Fields.Item("U_CODEBARS").Value = GetElement(xmlConsulta, "//item/document/codebars");
                        oUserTable.UserFields.Fields.Item("U_FAMILY").Value = GetElement(xmlConsulta, "//item/document/familia");
                        oUserTable.UserFields.Fields.Item("U_SUBFAMILY").Value = GetElement(xmlConsulta, "//item/document/subfamilia");

                        int iRet = oUserTable.Add();
                        if (iRet != 0)
                        {
                            validacion_err = true;
                            Log.logError("(" + B1company.GetLastErrorCode().ToString() + ") " + B1company.GetLastErrorDescription(), fileName, B1company);
                        }
                        else
                        {
                            File.Move(file.FullName, @"C:\XML\Articulos\Finalizados\" + file.Name);
                            //Log.logProc("ARTICULO", fileName, B1company);
                            procesados++;
                        }

                        /*if ((contador % 150) == 0)
                        {
                            B1company.EndTransaction(BoWfTransOpt.wf_Commit);
                            B1company.StartTransaction();
                        }*/


                    }
                    else{
                        Log.logError("ESTE ARCHIVO YA ESTA REGISTRADO", fileName, B1company);
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
                        B1company.EndTransaction(BoWfTransOpt.wf_Commit);
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
                B1company.EndTransaction(BoWfTransOpt.wf_RollBack);
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

