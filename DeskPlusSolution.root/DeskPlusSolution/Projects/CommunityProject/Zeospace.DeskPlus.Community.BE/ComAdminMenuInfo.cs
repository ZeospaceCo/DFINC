using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class ComAdminMenuInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComAdminMenuInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentID">The _parent ID.</param>
        /// <param name="_adminMenuID">The _admin menu ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_url">The _url.</param>
        public ComAdminMenuInfo(
            int	_companyID ,int	_parentID ,int	_adminMenuID ,string	_name ,string	_url ) 
        {
	         this.CompanyID	=	_companyID;
	         this.ParentID	=	_parentID;
	         this.AdminMenuID	=	_adminMenuID;
	         this.Name	=	_name;
	         this.Url	=	_url;
        } 

        public int CompanyID { get; set; }
        public int ParentID { get; set; }
        public int AdminMenuID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
