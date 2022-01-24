using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP
{
    public static class Log
    {
        public static void logError(string mensaje, string fileName, ICompany B1company)
        {
            try
            {
                SAPbobsCOM.Recordset oRecordSet;
                oRecordSet = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery("select * from \"@ERROR_WS\" WHERE \"U_FECHA\" = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')");
                SAPbobsCOM.GeneralService oGeneralService;
                SAPbobsCOM.GeneralData oGeneralData;
                SAPbobsCOM.GeneralDataCollection oSons;
                SAPbobsCOM.GeneralData oSon;
                SAPbobsCOM.CompanyService sCmp;
                SAPbobsCOM.GeneralDataParams oGeneralParams;
                sCmp = B1company.GetCompanyService();
                oGeneralService = sCmp.GetGeneralService("ERR_WS");
                if (oRecordSet.RecordCount == 0)
                {
                    oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                    oGeneralData.SetProperty("Code", DateTime.Now.ToString("ERR-MM-dd-yyyy"));
                    oGeneralData.SetProperty("U_FECHA", DateTime.Now.ToString());
                    oGeneralData.SetProperty("U_CONTEO_ERRORES", 1);
                    oSons = oGeneralData.Child("ERROR_WS_DET");
                    oSon = oSons.Add();
                    oSon.SetProperty("U_HORA", DateTime.Now.ToString("hh:mm tt"));
                    oSon.SetProperty("U_ARCHIVO", fileName);
                    oSon.SetProperty("U_RETROALIMENTACION", mensaje);
                    oGeneralService.Add(oGeneralData);
                }
                else
                {
                    int conteo = oRecordSet.Fields.Item("U_CONTEO_ERRORES").Value;
                    conteo++;
                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", DateTime.Now.ToString("ERR-MM-dd-yyyy"));
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_CONTEO_ERRORES", conteo);
                    oSons = oGeneralData.Child("ERROR_WS_DET");
                    oSon = oSons.Add();
                    oSon.SetProperty("U_HORA", DateTime.Now.ToString("hh:mm tt"));
                    oSon.SetProperty("U_ARCHIVO", fileName);
                    oSon.SetProperty("U_RETROALIMENTACION", mensaje);
                    oGeneralService.Update(oGeneralData);
                }
            }
            catch (Exception a)
            {
                throw new Exception(a.Message, a.InnerException);
            }
        }
        public static void logProc(string tipo, string fileName, ICompany B1company)
        {
            try
            {
                SAPbobsCOM.Recordset oRecordSet;
                oRecordSet = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery("select * from \"@DOC_PROC\" WHERE \"U_FECHA\" = TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')");
                SAPbobsCOM.GeneralService oGeneralService;
                SAPbobsCOM.GeneralData oGeneralData;
                SAPbobsCOM.GeneralDataCollection oSons;
                SAPbobsCOM.GeneralData oSon;
                SAPbobsCOM.CompanyService sCmp;
                SAPbobsCOM.GeneralDataParams oGeneralParams;
                sCmp = B1company.GetCompanyService();
                oGeneralService = sCmp.GetGeneralService("PROC_XML");
                if (oRecordSet.RecordCount == 0)
                {
                    oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);
                    oGeneralData.SetProperty("Code", DateTime.Now.ToString("PROC-MM-dd-yyyy"));
                    oGeneralData.SetProperty("U_FECHA", DateTime.Now.ToString());
                    oGeneralData.SetProperty("U_CONTEO_DOCS", 1);
                    oSons = oGeneralData.Child("DET_PROC");
                    oSon = oSons.Add();
                    oSon.SetProperty("U_HORA", DateTime.Now.ToString("hh:mm tt"));
                    oSon.SetProperty("U_ARCHIVO", fileName);
                    oSon.SetProperty("U_TIPO", tipo);
                    oGeneralService.Add(oGeneralData);
                }
                else
                {
                    int conteo = oRecordSet.Fields.Item("U_CONTEO_DOCS").Value;
                    conteo++;
                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", DateTime.Now.ToString("PROC-MM-dd-yyyy"));
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_CONTEO_DOCS", conteo);
                    oSons = oGeneralData.Child("DET_PROC");
                    oSon = oSons.Add();
                    oSon.SetProperty("U_HORA", DateTime.Now.ToString("hh:mm tt"));
                    oSon.SetProperty("U_ARCHIVO", fileName);
                    oSon.SetProperty("U_TIPO", tipo);
                    oGeneralService.Update(oGeneralData);
                }
            }
            catch (Exception a)
            {
                throw new Exception(a.Message, a.InnerException);
            }
        }
    }
}
