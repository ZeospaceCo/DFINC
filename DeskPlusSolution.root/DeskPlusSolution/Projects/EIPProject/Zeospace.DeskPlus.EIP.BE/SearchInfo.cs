using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class SearchInfo
    {
        public int ROW { get; set; }
        public int CompanyID { get; set; }
        public int SearchID { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public string Subjects { get; set; }
        public string Title { get; set; }
        public int AttachFileID { get; set; }
        public string FileName { get; set; }
        public string HeaderBase { get; set; }
        public string HeaderKo { get; set; }
        public string HeaderEn { get; set; }
        public string HeaderJa { get; set; }
        public string HeaderFr { get; set; }
        public string HeaderZhcn { get; set; }
        public string HeaderZhtw { get; set; }
        public string HeaderManyName { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public string DeptBase { get; set; }
        public string DeptKo { get; set; }
        public string DeptEn { get; set; }
        public string DeptJa { get; set; }
        public string DeptFr { get; set; }
        public string DeptZhcn { get; set; }
        public string DeptZhtw { get; set; }
        public string DeptManyName { get; set; }
        public string TitleBase { get; set; }
        public string TitleKo { get; set; }
        public string TitleEn { get; set; }
        public string TitleJa { get; set; }
        public string TitleFr { get; set; }
        public string TitleZhcn { get; set; }
        public string TitleZhtw { get; set; }
        public string TitleManyName { get; set; }
        public string DutyBase { get; set; }
        public string DutyKo { get; set; }
        public string DutyEn { get; set; }
        public string DutyJa { get; set; }
        public string DutyFr { get; set; }
        public string DutyZhcn { get; set; }
        public string DutyZhtw { get; set; }
        public string DutyManyName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public string RijndaelURL { get; set; }

        #region 게시판 전용

        /// <summary>
        /// 익명글여부
        /// </summary>
        public string OptionAnonymity { get; set; }
        /// <summary>
        /// 부서별게시판 여부
        /// </summary>
        public int OwnerDeptID { get; set; }
        /// <summary>
        /// 부서별 게시판의 부서아이디값
        /// </summary>
        public string OptionUseDept { get; set; }

        #endregion


    }
}
