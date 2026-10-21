using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws.BE
{
    public class CompanyInfo
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string EstablishDate { get; set; }
        public string InstitutorName { get; set; }
        public string InstitutorEnglishName { get; set; }
        public string InstitutorRegistrationNumber { get; set; }
        public string IndustryClass { get; set; }
        public string BusinessCondition { get; set; }
        public string HomePage { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string EnglishAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int CompanyQuota { get; set; }
        public string ActiveCompany { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public int UserCnt { get; set; }
    }   
}