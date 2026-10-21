using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class EnforceStampInfo : BE.DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int EnforceStampID { get; set; }
        public int DeptID { get; set; }
        public string ExpFirstBase { get; set; }
        public string ExpFirstKo { get; set; }
        public string ExpFirstEn { get; set; }
        public string ExpFirstJa { get; set; }
        public string ExpFirstFr { get; set; }
        public string ExpFirstZhcn { get; set; }
        public string ExpFirstZhtw { get; set; }
        public string ExpMidBase { get; set; }
        public string ExpMidKo { get; set; }
        public string ExpMidEn { get; set; }
        public string ExpMidJa { get; set; }
        public string ExpMidFr { get; set; }
        public string ExpMidZhcn { get; set; }
        public string ExpMidZhtw { get; set; }
        public string ExpEndBase { get; set; }
        public string ExpEndKo { get; set; }
        public string ExpEndEn { get; set; }
        public string ExpEndJa { get; set; }
        public string ExpEndFr { get; set; }
        public string ExpEndZhcn { get; set; }
        public string ExpEndZhtw { get; set; }
        public string ZipCode { get; set; }
        public string AddressFirstBase { get; set; }
        public string AddressFirstKo { get; set; }
        public string AddressFirstEn { get; set; }
        public string AddressFirstJa { get; set; }
        public string AddressFirstFr { get; set; }
        public string AddressFirstZhcn { get; set; }
        public string AddressFirstZhtw { get; set; }
        public string AddressEndBase { get; set; }
        public string AddressEndKo { get; set; }
        public string AddressEndEn { get; set; }
        public string AddressEndJa { get; set; }
        public string AddressEndFr { get; set; }
        public string AddressEndZhcn { get; set; }
        public string AddressEndZhtw { get; set; }
        public string PhoneBase { get; set; }
        public string PhoneKo { get; set; }
        public string PhoneEn { get; set; }
        public string PhoneJa { get; set; }
        public string PhoneFr { get; set; }
        public string PhoneZhcn { get; set; }
        public string PhoneZhtw { get; set; }
        public string FaxBase { get; set; }
        public string FaxKo { get; set; }
        public string FaxEn { get; set; }
        public string FaxJa { get; set; }
        public string FaxFr { get; set; }
        public string FaxZhcn { get; set; }
        public string FaxZhtw { get; set; }
        public string Homepage { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        /* 그외필드 */
        public int ROW { get; set; }
        public string IsStampStr { get; set; }
        public string IsAdminStr { get; set; }
        public string WriteDateStr { get; set; }
        public string ModifyDateStr { get; set; }
    }
}
