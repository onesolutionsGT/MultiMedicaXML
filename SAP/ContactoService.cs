using Modelos;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP
{
    public class ContactoService : Interfaces.IContactoService
    {
        public Respuesta addContacto(ICompany B1company, Contacto contacto, string CardCode)
        {
            try
            {
                SAPbobsCOM.BusinessPartners bop = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                
                bop.GetByKey(CardCode);
                bop.ContactEmployees.Name = contacto.Name;
                bop.ContactEmployees.Address = contacto.Address;
                bop.ContactEmployees.Phone1 = contacto.Phone1;
                bop.ContactEmployees.Phone2 = contacto.Phone2;
                bop.ContactEmployees.Fax = contacto.Fax;
                bop.ContactEmployees.MobilePhone = contacto.MobilePhone;
                bop.ContactEmployees.E_Mail = contacto.E_Mail;
                bop.ContactEmployees.Remarks1 = contacto.Remarks1;
                bop.ContactEmployees.Remarks2 = contacto.Remarks2;
                if (contacto.Gender == GENDER.FEMALE)
                    bop.ContactEmployees.Gender = BoGenderTypes.gt_Female;
                else
                    bop.ContactEmployees.Gender = BoGenderTypes.gt_Male;

                if (contacto.Active == ACTIVE.YES)
                    bop.ContactEmployees.Active = BoYesNoEnum.tYES;
                else
                    bop.ContactEmployees.Active = BoYesNoEnum.tNO;


                bop.ContactEmployees.FirstName = contacto.FirstName;
                bop.ContactEmployees.MiddleName = contacto.MiddleName;
                bop.ContactEmployees.LastName = contacto.LastName;
                bop.ContactEmployees.Add();
                return new Respuesta
                {
                    Mensaje = $"Contacto creado para ${CardCode}",
                    Codigo = "0",
                };
            }
            catch (Exception ex)
            {

                throw new Exception();
            }
            

            
        }
    }
}
