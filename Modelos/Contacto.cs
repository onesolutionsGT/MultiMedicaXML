using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Contacto
    {
        private string name;
        private string address;
        private string phone1;
        private string phone2;
        private string fax;
        private string mobilePhone;
        private string e_Mail;
        private string remarks1;
        private string remarks2;
        private GENDER gender;
        private ACTIVE active;
        private string firstName;
        private string middleName;
        private string lastName;
        
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string Phone1 { get => phone1; set => phone1 = value; }
        public string Phone2 { get => phone2; set => phone2 = value; }
        public string Fax { get => fax; set => fax = value; }
        public string MobilePhone { get => mobilePhone; set => mobilePhone = value; }
        public string E_Mail { get => e_Mail; set => e_Mail = value; }
        public string Remarks1 { get => remarks1; set => remarks1 = value; }
        public string Remarks2 { get => remarks2; set => remarks2 = value; }
        public GENDER Gender { get => gender; set => gender = value; }
        public ACTIVE Active { get => active; set => active = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; }
    }

    public enum GENDER
    {
        FEMALE = 0,
        MALE =1
    }

    public enum ACTIVE
    {
        YES = 1,
        NO =0
    }
}
