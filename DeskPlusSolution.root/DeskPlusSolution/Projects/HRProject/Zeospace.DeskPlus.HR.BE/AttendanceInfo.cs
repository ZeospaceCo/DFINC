using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.HR.BE
{
    [Serializable]
    public class AttendanceInfo
    {
        public AttendanceInfo() { }

        public int CompanyID { get; set; }
        public int AttID { get; set; }
        public string UserID { get; set; }            //아이디
        public string DeptID { get; set; }            //아이디
        public string PathID { get; set; }            //부서경로

        public string ParentDeptName { get; set; }      //상위부서명
        public string DeptName { get; set; }            //부서명
        public string DeptBase { get; set; }            //부서명
        public string UserName { get; set; }            //이름
        public string TitleName { get; set; }
        public string DutyName { get; set; }

        public string DeptState { get; set; }

        //개인 근태 현황표
        public string ROW { get; set; }
        public string WorkDate { get; set; }
        public string CalcDate { get; set; }
        public string Today { get; set; }
        public DateTime? WSTimeDate { get; set; }
        public DateTime? WCTimeDate { get; set; }
        public string WSTime { get; set; }
        public string WCTime { get; set; }
        public string WSConn { get; set; }
        public string WCConn { get; set; }
        public string bWS { get; set; }
        public string bWSStr { get; set; }
        public string bWC { get; set; }
        public string bWCStr { get; set; }
        public string bWSIN { get; set; }
        public string bWSINStr { get; set; }
        public string bWSOUT { get; set; }
        public string bWSOUTStr { get; set; }
        public string bWCIN { get; set; }
        public string bWCINStr { get; set; }
        public string bWCOUT { get; set; }
        public string bWCOUTStr { get; set; }
        public string Register { get; set; }
        public string RegisterStr { get; set; }
        public string AMRegister { get; set; }
        public string AMRegisterStr { get; set; }
        public string PMRegister { get; set; }
        public string PMRegisterStr { get; set; }
        public string StandardTime { get; set; }
        public string StandardTimePer { get; set; }
        public string PossTime { get; set; }

        public string TimeGap { get; set; }
        public string WorkTime { get; set; }
        public string WorkTimeCnt { get; set; }
        public string WorkTimeSUM { get; set; }
        public string HoliTime { get; set; }

        public string OverTime { get; set; }
        public string OverCount { get; set; }
        public string OverTimeCnt { get; set; }
        public string OverTimeSUM { get; set; }
        public string PRDocumentID { get; set; }
        public string bOver { get; set; }
        public string bOverGubun { get; set; }
        public string OverRemark { get; set; }
        public string OverRemarkParam { get; set; }
        public string AllWorkTime { get; set; }
        public string AllWorkTimePer { get; set; }
        public string AllWorkTimeCnt { get; set; }
        public string AllWorkTimeSUM { get; set; }

        public string bLate { get; set; }
        public string bLateMinute { get; set; }
        public string bLateGubun { get; set; }
        public string LateRemark { get; set; }
        public string LateRemarkParam { get; set; }
        public string bEarly { get; set; }
        public string bEarlyMinute { get; set; }
        public string bEarlyGubun { get; set; }
        public string EarlyRemark { get; set; }
        public string EarlyRemarkParam { get; set; }
        public string bHoli { get; set; }
        public string bHoliCnt { get; set; }
        public string bHoliAll { get; set; }
        public string HoliGubun { get; set; }
        public string HoliGubun2 { get; set; }
        public string HoliRemark { get; set; }
        public string HUDocumentID { get; set; }

        public string bAMHalf { get; set; }
        public string bPMHalf { get; set; }
        public string bEtcAMHalf { get; set; }
        public string bEtcPMHalf { get; set; }

        public string bSysHoli { get; set; }
        public string DayWeek { get; set; }
        public string SysHoliRemark { get; set; }
        public string bEtc { get; set; }
        public string bEtcCnt { get; set; }
        public string IsHoli { get; set; }

        public string SUMMARY { get; set; }
        public string TotalWorkTimeCnt { get; set; }
        public string TotalOverTimeCnt { get; set; }
        public string TotalAllWorkTimeCnt { get; set; }

        public string UserSUM { get; set; }
        public string BaseDeptUcnt0 { get; set; }
        public string BaseDeptUcnt1 { get; set; }
        public string pAvg { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string OverLap { get; set; }
        public string ModifyUserID { get; set; }
        public string ModifyNameBase { get; set; }
        public string ModifyReason { get; set; }
        public string ModifyDateStr { get; set; }
        public int HistoryCount { get; set; }
        public string VHA_CompanyID { get; set; } //겸직자 출근정보
        public string VW2_CompanyID { get; set; } //입사일,퇴사일 사이면 값이 있음
        public int MinClosingID { get; set; }
        public int MaxClosingID { get; set; }
        public string Sunday { get; set; }
        public string Past { get; set; }        
    }
}
