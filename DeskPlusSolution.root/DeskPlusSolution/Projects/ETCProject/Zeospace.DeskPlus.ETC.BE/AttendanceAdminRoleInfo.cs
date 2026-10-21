using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    [Serializable]
    public class AttendanceAdminRoleInfo
    {
        public AttendanceAdminRoleInfo() { }

        public int CompanyID { get; set; }          //회사ID
        public int UserID { get; set; }             //사용자ID
        public int GroupID { get; set; }            //적용부서ID
        public string IsLowRank { get; set; }       //하위부서포함여부
        public int WriteUserID { get; set; }        //입력자
            
        public string UserName { get; set; }        //사용자명
        public string DeptName { get; set; }        //부서명
        public string TitleName { get; set; }       //직급명
        public string DutyName { get; set; }        //직책명

        public string ViewDeptName { get; set; }    //적용부서명
    }
}
