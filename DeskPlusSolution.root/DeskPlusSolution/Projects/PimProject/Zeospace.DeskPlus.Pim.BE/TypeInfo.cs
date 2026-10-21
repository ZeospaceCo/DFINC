using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class TypeInfo
    {
        public TypeInfo(
            int _companyID, int _userID, int _typeID, int _openTypeID
            )
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.TypeID = _typeID;
            this.OpenTypeID = _openTypeID;
        }

        public TypeInfo()
        {
        }



        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int TypeID { get; set; }

        public int OpenTypeID { get; set; }

    }
}
