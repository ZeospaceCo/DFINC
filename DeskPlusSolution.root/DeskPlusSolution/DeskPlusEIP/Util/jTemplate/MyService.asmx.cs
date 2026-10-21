using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;

namespace Zeospace.DeskPlus.Base.Web.Util.jTemplate
{
    /// <summary>
    /// Summary description for MyService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyService : System.Web.Services.WebService
    {
        private List<object> _customers;

        public MyService()
        {
            
        }

        [WebMethod(EnableSession = true)]
        public object GetCustomer(int customerID)
        {
            return _customers[customerID];
        }

        [WebMethod(EnableSession = true)]
        public List<object> GetCustomers(int gubun)
        {
            if (gubun == 1)
            {
                _customers = new List<object>() {
                    new { Forename = "Joe", Surname = "Stevens", DOB = "31/01/1983", Location = "Sydney" },
                    new { Forename = "Tom", Surname = "Male", DOB = "02/02/1977", Location = "Brisbane" },
                    new { Forename = "Emily", Surname = "Stevens", DOB = "14/01/1988", Location = "Melbourne" },
                    new { Forename = "Leroy", Surname = "Phipps", DOB = "05/12/1982", Location = "Sydney" },
                    new { Forename = "Saul", Surname = "Stevens", DOB = "02/08/1984", Location = "Perth" }
                };
            }
            else
            {

                _customers = new List<object>() {
                    new { Forename = "Joe1", Surname = "Stevens1", DOB = "31/01/1983", Location = "Sydney1" },
                    new { Forename = "Tom2", Surname = "Male1", DOB = "02/02/1977", Location = "Brisbane1" },
                    new { Forename = "Emily3", Surname = "Stevens1", DOB = "14/01/1988", Location = "Melbourne1" },
                    new { Forename = "Leroy4", Surname = "Phipps1", DOB = "05/12/1982", Location = "Sydney1" },
                    new { Forename = "Saul5", Surname = "Stevens1", DOB = "02/08/1984", Location = "Perth1" }
                };
            }

            return _customers;
        }
    }
}
