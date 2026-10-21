using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class ListInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ContentText { get; set; }
        public string ItemID { get; set; }        
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExcessDay { get; set; }
        public string PopUpYN { get; set; }
        /// <summary>
        /// UI의 전체선택 요청값이며 DB 컬럼이 아닙니다. 전체선택 원본은 DP_EDU_ListTO의 A행으로 저장합니다.
        /// </summary>
        public string AllTargetYN { get; set; }
        public int AlarmDay { get; set; }
        public int DeptCodeID { get; set; }
        public string NameBase { get; set; }
        public string DeptName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public DateTime ModifyDate { get; set; }

        public int ROW { get; set; }
        public string ItemName { get; set; }

        public int FileCount { get; set; }
        public int ViewerCount { get; set; }
                
        public int TOUserID { get; set; }
        public string TONameBase { get; set; }
        public string TODeptName { get; set; }
        public string ToUserList { get; set; }

        public string BeforeHiddenPath { get; set; }
        public string HiddenSpoolPath { get; set; }
        public string MsgContents { get; set; }
        public string PreLang { get; set; }
        public string SenderName { get; set; }

        public string DateStr { get; set; }
        public string StatusStr { get; set; }
        public string CompleteYN { get; set; }

    }
}

