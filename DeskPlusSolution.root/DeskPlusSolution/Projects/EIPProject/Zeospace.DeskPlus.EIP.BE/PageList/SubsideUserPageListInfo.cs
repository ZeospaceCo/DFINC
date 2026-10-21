using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE.PageList
{
    /// <summary>
    /// 계열사 관리자 리스트용
    /// </summary>
    [Serializable]
    public class SubsideUserPageListInfo
    {
        public SubsideUserPageListInfo() { }

        public int CompanyID {get;set;}
        public int RoleID {get;set;}
        public int UserID {get;set;}
        public string RoleCode {get;set;}
        public string ComBase {get;set;}
        public string LogInID {get;set;}
        public string NameBase {get;set;}
        public string DeptBase {get;set;}
        public string TitleBase {get;set;}
        public string Mail {get;set;}
        public string MobilePhone {get;set;}
        public string OfficePhone { get; set; }
    }
}
