using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class OpenListInfo
    {
        public OpenListInfo(
            int _companyID, int _userID, int _openListID, int _openUserID, int _openDeptID, string _gubun
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.OpenListID = _openListID;
            this.OpenUserID = _openUserID;
            this.OpenDeptID = _openDeptID;
            this.Gubun = _gubun;
        }

        public OpenListInfo()
        {
        }



        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int OpenListID { get; set; }

        public int OpenUserID { get; set; }

        public int OpenDeptID { get; set; }

        public string Gubun { get; set; }        

    }
}
