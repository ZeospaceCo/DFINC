using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Report.BE
{
    [Serializable]
    public class ListInfo : Common.BE.DeptCodeInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public string YYYYMM { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        //
        
        public string NowJob { get; set; }
        public string NextJob { get; set; }

        public string Issue { get; set; }

        /// <summary>
        /// 상태(0:-,1:저장;2:요청;3:확인)
        /// </summary>
        public string Flag { get; set; }

        
        public int DeptCodeID { get; set; }
        public int DeptID { get; set; }

        
        /// <summary>
        /// 공개 : 1; 비공개 : 2
        /// </summary>
        public string IsClose { get; set; }

        /// <summary>
        /// 0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
        /// </summary>
        public string IsLinked { get; set; }

        /// <summary>
        /// 폴더 정보
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        public int FolderID { get; set; }

        #region  승인자

        public int CompUserID { get; set; }
        public string CompNameBase { get; set; }
        public string CompNameKo { get; set; }
        public string CompNameEn { get; set; }
        public string CompNameJa { get; set; }
        public string CompNameFr { get; set; }
        public string CompNameZhcn { get; set; }
        public string CompNameZhtw { get; set; }
        public string CompNameName { get; set; }

        public string CompDeptBase { get; set; }
        public string CompDeptKo { get; set; }
        public string CompDeptEn { get; set; }
        public string CompDeptJa { get; set; }
        public string CompDeptFr { get; set; }
        public string CompDeptZhcn { get; set; }
        public string CompDeptZhtw { get; set; }
        public string CompDeptName { get; set; }

        public string CompTitleBase { get; set; }
        public string CompTitleKo { get; set; }
        public string CompTitleEn { get; set; }
        public string CompTitleJa { get; set; }
        public string CompTitleFr { get; set; }
        public string CompTitleZhcn { get; set; }
        public string CompTitleZhtw { get; set; }
        public string CompTitleName { get; set; }

        public string CompDutyBase { get; set; }
        public string CompDutyKo { get; set; }
        public string CompDutyEn { get; set; }
        public string CompDutyJa { get; set; }
        public string CompDutyFr { get; set; }
        public string CompDutyZhcn { get; set; }
        public string CompDutyZhtw { get; set; }
        public string CompDutyName { get; set; }
        public DateTime CompletDate { get; set; }

        #endregion

        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        
        public int FileCount { get; set; }
        public int ReplyCount { get; set; }
        public string HiddenSpoolPath { get; set; }
        public string StartDateYMD { get; set; }
        public string EndDateYMD { get; set; }
        public string ShowNew { get; set; }
        /// <summary>
        /// 이름별 홀짝 색깔 칠하는 용도
        /// </summary>
        public string HolChk { get; set; }
        public string WriteUserName { get; set; }
        public string AlarmContent { get; set; }
        public string Title { get; set; }
        public string WriteDateDT { get; set; }
        public int ViewCount { get; set; }

    }
}
