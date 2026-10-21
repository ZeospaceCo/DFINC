using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class ComAdminInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComAdminInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_adminMenuID">The _admin menu ID.</param>
        public ComAdminInfo(
            int	_companyID ,int	_communityID ,int	_userID ,int	_adminMenuID ) 
        {
	         this.CompanyID	=	_companyID;
	         this.CommunityID	=	_communityID;
	         this.UserID	=	_userID;
	         this.AdminMenuID	=	_adminMenuID;
        } 

        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public int UserID { get; set; }
        public int AdminMenuID { get; set; }
    }
}
