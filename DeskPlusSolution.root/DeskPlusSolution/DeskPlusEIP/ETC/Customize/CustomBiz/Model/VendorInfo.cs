using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model
{

    [Serializable]
    public class VendorInfo 
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorPostingGroup { get; set; }
        public string DepartmentCode { get; set; }
        public string BankAccountCode { get; set; }

        // BackAccount 관련
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankClearingID { get; set; }
        public string Code { get; set; }

        // G/L Account 관련
        public string GLAccountNo { get; set; }
        public string GLAccountName { get; set; }

        // Tax 관련
        public string TaxCD { get; set; }
        public string TaxName { get; set; }
        public string TaxGLAccountNo { get; set; }

        // 지급주기 
        public string PaymentTerm { get; set; }
    }
    
}