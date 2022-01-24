using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Conexion
    {

        private string server;
        private string userName;
        private string password;
        private string companyDB;
        private string sQL;
        private string dbUserName;
        private string dbPassword;

        public string Server { get => server; set => server = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string CompanyDB { get => companyDB; set => companyDB = value; }
        public string SQL { get => sQL; set => sQL = value; }
        public string DbUserName { get => dbUserName; set => dbUserName = value; }
        public string DbPassword { get => dbPassword; set => dbPassword = value; }
    }
}
