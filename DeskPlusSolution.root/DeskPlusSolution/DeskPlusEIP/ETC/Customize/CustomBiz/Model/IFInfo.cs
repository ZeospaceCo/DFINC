using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model
{
     [Serializable]
    public class IFInfo
    {
        public string DocumentNo { get; set; }
        public int LineNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DueDate { get; set; }
        public int AccountType { get; set; }
        public string AccountNo { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string PostingGroup { get; set; }
        public string Dimension1Code { get; set; }
        public string Dimension2Code { get; set; }
        public string Dimension3Code { get; set; }
        public string Dimension4Code { get; set; }
        public string BankAcountCode { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAcountCearingID { get; set; }
        public string PONo { get; set; }
        public string CardApprovalNo { get; set; }
        public string AccountNM { get; set; }
        public string VendorNM { get; set; }
        public string Seq { get; set; }
        public string Kind { get; set; }

        public string TempleteName { get; set; }
        public string BatchName { get; set; }

    }
}