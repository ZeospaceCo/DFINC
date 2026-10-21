using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.ETC.BE
{
    [Serializable]
    public class AttendanceAdminStatusInfo
    {
        public AttendanceAdminStatusInfo() { }

        public string UserID { get; set; }            //아이디

        public string DeptName { get; set; }            //부서명
        public string UserName { get; set; }            //이름
        public string TitleName { get; set; }
        public string DutyName { get; set; }

        public string DeptCount { get; set; }           //부서인원
        public string HoliCount { get; set; }           //연차인원
        public string HoliPer { get; set; }             //연차 %

        public string TodayCount { get; set; }          //금일일원
        public string TodayMIN { get; set; }            //금일일원MIN
        public string TodayMAX { get; set; }            //금일일원MAX
        public string TodayAVG { get; set; }            //금일일원AVG
        public string TodayPer { get; set; }            //금일일원 %

        public string WSTimeCount { get; set; }         //출근인원
        public string WSTimeMIN { get; set; }           //출근시간MIN
        public string WSTimeMAX { get; set; }           //출근시간MAX
        public string WSTimeAVG { get; set; }           //출근시간AVG
        public string WSTimePer { get; set; }           //출근 %

        public string WSTimeCountLate { get; set; }     //퇴근인원
        public string WSTimeMINLate { get; set; }       //퇴근시간MIN
        public string WSTimeMAXLate { get; set; }       //퇴근시간MAX
        public string WSTimeAVGLate { get; set; }       //퇴근시간AVG
        public string WSTimePerLate { get; set; }       //퇴근 %

        public string WCTimeCount { get; set; }         //지각인원
        public string WCTimeMIN { get; set; }           //지각시간MIN
        public string WCTimeMAX { get; set; }           //지각시간MAX
        public string WCTimeAVG { get; set; }           //지각시간AVG
        public string WCTimePer { get; set; }           //지각 %

        public string AbsenteeismCount { get; set; }    //결근인원
        public string AbsenteeismPer { get; set; }      //결근 %
        public string NowCount { get; set; }            //현재인원
        public string NowPer { get; set; }              //현재인원 %

        //개인근태현황용 년별 
        public string YTodayCount { get; set; }          //금일일원
        public string YTodayMIN { get; set; }            //금일일원MIN
        public string YTodayMAX { get; set; }            //금일일원MAX
        public string YTodayAVG { get; set; }            //금일일원AVG
        public string YTodayPer { get; set; }            //금일일원 %

        public string YWSTimeCount { get; set; }         //출근인원
        public string YWSTimeMIN { get; set; }           //출근시간MIN
        public string YWSTimeMAX { get; set; }           //출근시간MAX
        public string YWSTimeAVG { get; set; }           //출근시간AVG
        public string YWSTimePer { get; set; }           //출근 %

        public string YWSTimeCountLate { get; set; }     //퇴근인원
        public string YWSTimeMINLate { get; set; }       //퇴근시간MIN
        public string YWSTimeMAXLate { get; set; }       //퇴근시간MAX
        public string YWSTimeAVGLate { get; set; }       //퇴근시간AVG
        public string YWSTimePerLate { get; set; }       //퇴근 %

        public string YWCTimeCount { get; set; }         //지각인원
        public string YWCTimeMIN { get; set; }           //지각시간MIN
        public string YWCTimeMAX { get; set; }           //지각시간MAX
        public string YWCTimeAVG { get; set; }           //지각시간AVG
        public string YWCTimePer { get; set; }           //지각 %

    }
}
