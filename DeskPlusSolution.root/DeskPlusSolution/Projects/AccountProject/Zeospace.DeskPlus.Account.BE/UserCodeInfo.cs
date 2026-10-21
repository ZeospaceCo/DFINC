using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserCodeInfo
    {
        public UserCodeInfo(
            int _companyID, int _userCodeID, int _parentUserCodeID, string _keyID, string _keyText
            , int _orders, int _writeUserID, DateTime _writeDate, int _modifyUserID, DateTime _modifyDate
            )
        {
            this.CompanyID = _companyID;
            this.UserCodeID = _userCodeID;
            this.ParentUserCodeID = _parentUserCodeID;
            this.KeyID = _keyID;
            this.KeyText = _keyText;
            this.Orders = _orders;
            this.WriteUserID = _writeUserID;
            this.WriteDate = _writeDate;
            this.ModifyUserID = _modifyUserID;
            this.ModifyDate = _modifyDate;
        }

        ///
        //CompanyID
        ///
        public int CompanyID { get; set; }

        ///
        //UserCodeID
        ///
        public int UserCodeID { get; set; }

        ///
        //ParentUserCodeID
        ///
        public int ParentUserCodeID { get; set; }

        ///
        //KeyID
        ///
        public string KeyID { get; set; }

        ///
        //KeyText
        ///
        public string KeyText { get; set; }

        ///
        //Orders
        ///
        public int Orders { get; set; }

        ///
        //WriteUserID
        ///
        public int WriteUserID { get; set; }

        ///
        //WriteDate
        ///
        public DateTime WriteDate { get; set; }

        ///
        //ModifyUserID
        ///
        public int ModifyUserID { get; set; }

        ///
        //ModifyDate
        ///
        public DateTime ModifyDate { get; set; }
    }
}