using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.HR.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Ws.Content.Popup
{
    /// <summary>
    /// ViewLate의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewLate : Base.BaseWSHR
    {
        #region 리스트

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 근태 날짜별 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetLateList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			        //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Content.Popup.ViewLate).Assembly;	    //어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string pathID = GetDecryptParam("pathID", "");                  //부서경로
                string deptBase = GetDecryptParam("deptBase", "");              //부서명
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string minclosingID = GetDecryptParam("minClosingID", "");
                string maxclosingID = GetDecryptParam("maxClosingID", "");
                string gubun = GetDecryptParam("gubun", "");

                startDate = DateHelper.GetCultureDate(startDate, "yyyy-MM-dd", PreLangReal);
                endDate = DateHelper.GetCultureDate(endDate, "yyyy-MM-dd", PreLangReal);

                DAL.Attendance biz = new DAL.Attendance();

                DataTableReader dtr = biz.GetDeptHolyList(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), pathID, startDate, endDate, "", "", 1, minclosingID.ToNumber<int>(), maxclosingID.ToNumber<int>(), gubun);                

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.AttendanceInfo info = new BE.AttendanceInfo();

                            DAL.Closing dalclo = new DAL.Closing();

                            info.UserID = dtr["UserID"].ToString();
                            info.DeptID = dtr["DeptID"].ToString();
                            info.PathID = dtr["PathID"].ToString();

                            info.DeptName = deptBase;

                            DataSet ds = dalclo.GetClosingUserInfoMax(CurrentUserInfo.CompanyID, info.UserID.ToNumber<int>(), info.DeptID.ToNumber<int>(), info.PathID, deptBase);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                info.TitleName = dr["TitleBase"].ToString();
                                info.UserName = dr["NameBase"].ToString();
                            }

                            /*
                            // 겸직자면 (겸) 표시
                            int basedept = dalclo.GetClosingBaseDeptCount(CurrentUserInfo.CompanyID, info.UserID.ToNumber<int>(), info.DeptID.ToNumber<int>(), info.PathID, deptBase);
                            if (basedept > 0)
                            {
                                info.UserName = info.UserName + "(" + this.resMng.GetString("BaseDept") + ")";
                            }
                            */

                            DateTime dtS;
                            if (DateTime.TryParse(dtr["CalcDate"].ToString(), out dtS))
                            {
                                info.CalcDate = DateHelpers.GetCultureDate(dtS, PreLangReal).Substring(0, 10);

                                DateTime sDate = DateTime.Parse(info.CalcDate);

                                if (sDate.DayOfWeek == DayOfWeek.Monday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Monday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Tuesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Wednesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Thursday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Thursday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Friday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Friday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Saturday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Saturday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Sunday) { info.CalcDate = info.CalcDate + " (" + resMng.GetString("Day_Sunday") + ")"; }
                            }
                            else
                            {
                                info.CalcDate = string.Empty;
                            }

                            info.WSTime = dtr["WSTime"].ToString();
                            info.WCTime = dtr["WCTime"].ToString();

                            info.bLate = dtr["bLate"].ToString(); //지각
                            info.bLateMinute = TimeUnit(dtr["bLateMinute"].ToString()); //지각시간
                            info.bLateGubun = resMng.GetString("Attendance_Tardy"); //지각
                            info.LateRemark = dtr["LateRemark"].ToString().ToNull(); //지각 사유

                            info.bEarly = dtr["bEarly"].ToString(); //조퇴
                            info.bEarlyMinute = TimeUnit(dtr["bEarlyMinute"].ToString()); //조퇴시간
                            info.bEarlyGubun = resMng.GetString("Attendance_Early"); //조퇴
                            info.EarlyRemark = dtr["EarlyRemark"].ToString().ToNull(); //조퇴 사유

                            info.bOver = dtr["bOver"].ToString(); //연장근무여부
                            info.bOverGubun = resMng.GetString("Attendance_Over"); //연장                            
                            info.OverTime = TimeUnit(dtr["OverTime"].ToString()); //연장근무시간
                            info.PRDocumentID = dtr["PRDocumentID"].ToString();
                            info.OverRemark = dtr["OverRemark"].ToString(); //연장근무 사유

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 시간 단위로 변경 10:00
        /// </summary>
        /// <param name="sourceHour"></param>
        /// <returns></returns>
        public string TimeUnit(string sourceMinute)
        {
            bool minus = false;
            if (sourceMinute.IndexOf('-') > -1)
            {
                sourceMinute = sourceMinute.Replace("-", "");
                minus = true;
            }

            int s_minute = Int32.Parse(sourceMinute);

            int hour = s_minute / 60;
            int minute = s_minute % 60;

            string hourStr = string.Empty;
            if (hour < 10)
            {
                hourStr = "0" + hour;
            }
            else
            {
                hourStr = hour.ToString();
            }

            string minuteStr = string.Empty;
            if (minute < 10)
            {
                minuteStr = "0" + minute;
            }
            else
            {
                minuteStr = minute.ToString();
            }

            if (minus)
            {
                return "-" + hourStr + ":" + minuteStr;
            }

            return hourStr + ":" + minuteStr;
        }

        #endregion
    }
}
