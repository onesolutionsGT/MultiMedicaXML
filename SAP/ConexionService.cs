using SAP.Interfaces;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP
{
    public class ConexionService : IConexionService
    {
        public SAPbobsCOM.ICompany B1company;
        public ConexionService(Modelos.Conexion cnn)
        {
            try
            {
                SAPbobsCOM.Company B1company = new SAPbobsCOM.Company();
                B1company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                //B1company.Server = "LAPTOP-LHOLCLCG"; //cnn.Server;
                //B1company.UserName = "manager"; //cnn.UserName;
                //B1company.Password = "123456"; //cnn.Password;
                //B1company.CompanyDB = "MEGA_SP_FACE"; // cnn.CompanyDB;

                B1company.Server = cnn.Server.ToString(); // "LAPTOP-LHOLCLCG"; //
                B1company.UserName = cnn.UserName.ToString(); //"manager"; //
                B1company.Password = cnn.Password.ToString();// "123456"; //
                B1company.CompanyDB = cnn.CompanyDB.ToString(); //"MEGA_SP_FACE"; //
                B1company.DbUserName = cnn.DbUserName.ToString();
                B1company.DbPassword = cnn.DbPassword.ToString();
                //B1company.LicenseServer = System.Configuration.ConfigurationManager.AppSettings("License");
                B1company.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                B1company.UseTrusted = false;
                int ret = B1company.Connect();
                if (ret != 0)
                {
                    string errMsg = B1company.GetLastErrorDescription();
                    int ErrNo = B1company.GetLastErrorCode();
                    throw new Exception(" Error" + ErrNo + " Codigo" + errMsg);
                }

                this.B1company = B1company;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }
       

        public ICompany getConnect()
        {
            return B1company;
        }
    }
}
