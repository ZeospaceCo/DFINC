using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model
{
    [Serializable]
    public class ExpenseUserInfo
    {
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public int Seq { get; set; }
        public string LegacyKey { get; set; }
        public string VoucherNo { get; set; }
        public string CardExKind { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DueDate { get; set; }
        public string VendorDescription { get; set; }
        public string ExDescription { get; set; }
        public decimal DebitAmount1 { get; set; }
        public decimal DebitAmount2 { get; set; }
        public decimal CreditAmount { get; set; }
        public string PostingGroup { get; set; }

        /// <summary>
        /// 관리항목 - 부서 (Dimension Code : DEPARTMENT)
        /// </summary>
        public string Dimension1Code { get; set; }

        /// <summary>
        /// 관리항목 - 사원 번호 (Dimension Code : EMP)
        /// </summary>
        public string Dimension2Code { get; set; }

        /// <summary>
        /// 관리항목 - 매입처 (Dimension Code : VENDOR)
        /// </summary>
        public string Dimension3Code { get; set; }

        /// <summary>
        /// 관리항목 - 제품 그룹 (Dimension Code : PRODUCT GROUP)
        /// </summary>
        public string Dimension4Code { get; set; }

        public string ExDimension1Code { get; set; }
        public string ExDimension2Code { get; set; }
        public string ExDimension3Code { get; set; }
        public string ExDimension4Code { get; set; }
        public string VendorNM { get; set; }
        public string ExVendorNM { get; set; }
        public string GLAccount { get; set; }
        public string GLAccountNM { get; set; }
        public string TaxCode { get; set; }
        public string TaxCodeNM { get; set; }
        public string ExBankAccountCode { get; set; }
        public string ExBankAccountNo { get; set; }
        public string ExBankAccountCearing { get; set; }
        public string ExPostingGroup { get; set; }
        

        public string BankAcountCode { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAcountCearingID { get; set; }
        public string PONo { get; set; }
        public string CardApprovalNo { get; set; }
        public int DocumentID { get; set; }

        /// <summary>
        /// 상태 = 비용저장 : T, 전자결재상신(진행) : I, 전자결재반려 : R  , 전자결재 완결 : C , 
        /// </summary>
        public string Status { get; set; }
        public DateTime WriteDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Etc1 { get; set; }
        public string Etc2 { get; set; }
        public string Etc3 { get; set; }
        public string Etc4 { get; set; }
        public string Etc5 { get; set; }
        public string Etc6 { get; set; }
        public string Etc7 { get; set; }
        public string Etc8 { get; set; }
        public string Etc9 { get; set; }
        public string Etc10 { get; set; }
    }
}