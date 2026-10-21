using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class MBOShareUserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MBOShareUserInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mBOID">The _m BOID.</param>
        /// <param name="_typeTargetNum">The _type target num.</param>
        /// <param name="_userName">Name of the _user.</param>
        /// <param name="_userMail">The _user mail.</param>
        public MBOShareUserInfo(
            int _companyID, int _mBOID, int _typeTargetNum, string _userName, string _userMail, int _TargetUserID) 
        {
	         this.CompanyID	=	_companyID;
	         this.MBOID	=	_mBOID;
	         this.TypeTargetNum	=	_typeTargetNum;
	         this.UserName	=	_userName;
	         this.UserMail	=	_userMail;
             this.TargetUserID = _TargetUserID;
        }

        public MBOShareUserInfo()
        {
        }

        public int CompanyID { get; set; }
        public int MBOID { get; set; }
        public int TypeTargetNum { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }

        public int TargetUserID { get; set; }
    }
}
