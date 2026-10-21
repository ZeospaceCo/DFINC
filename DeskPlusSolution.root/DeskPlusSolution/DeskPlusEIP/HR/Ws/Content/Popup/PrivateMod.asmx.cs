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
    /// PrivateMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class PrivateMod : Base.BaseWSHR
    {
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-04-13
        /// 작성목적 : 출퇴근시간 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetAttendance(string p)
        {
            try
            {
                SetDecryptString(p);

                int attID = GetDecryptParam("attID", "0").ToNumber<int>();                
                string targetUserID = GetDecryptParam("targetUserID", "0"); //수정 대상자
                string targetDeptID = GetDecryptParam("targetDeptID", "0"); //수정 부서정보
                string pathID = GetDecryptParam("pathID", "0"); //수정 부서경로정보
                string workDate = GetDecryptParam("workDate", "");
                string wsTimeHour = GetDecryptParam("wsTimeHour", "");
                string wsTimeMinute = GetDecryptParam("wsTimeMinute", "");
                string wcTimeHour = GetDecryptParam("wcTimeHour", "");
                string wcTimeMinute = GetDecryptParam("wcTimeMinute", "");
                string bLate = GetDecryptParam("bLate", "0");
                string bEarly = GetDecryptParam("bEarly", "0");
                string lateRemark = GetDecryptParam("lateRemark", "");
                string earlyRemark = GetDecryptParam("earlyRemark", "");
                string wsConn = GetDecryptParam("wsConn", "");
                string wcConn = GetDecryptParam("wcConn", "");
                string isHoli = GetDecryptParam("isHoli", "0");
                string modReason = GetDecryptParam("modReason", "");
                string modifyUserID = GetDecryptParam("modifyUserID", "0"); //수정자                

                DateTime tmpWSTime = new DateTime(workDate.Split('-')[0].ToNumber<int>(), workDate.Split('-')[1].ToNumber<int>(), workDate.Split('-')[2].ToNumber<int>()
                    , wsTimeHour.ToNumber<int>(), wsTimeMinute.ToNumber<int>(), 0);

                DateTime tmpWCTime = new DateTime(workDate.Split('-')[0].ToNumber<int>(), workDate.Split('-')[1].ToNumber<int>(), workDate.Split('-')[2].ToNumber<int>()
                    , wcTimeHour.ToNumber<int>(), wcTimeMinute.ToNumber<int>(), 0);

                BE.AttendanceInfo info = new AttendanceInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.AttID = attID;
                info.UserID = targetUserID; //수정 대상자
                info.DeptID = targetDeptID;
                info.PathID = pathID;
                info.WorkDate = workDate;

                if (wsTimeHour != "")
                {
                    info.WSTimeDate = tmpWSTime;
                }
                else
                {
                    info.WSTimeDate = null;
                }

                if (wcTimeHour != "")
                {
                    info.WCTimeDate = tmpWCTime;
                }
                else
                {
                    info.WCTimeDate = null;
                }

                info.bLate = bLate;
                info.bEarly = bEarly;

                info.bLateMinute = "0";
                info.bEarlyMinute = "0";

                //지각이나 조퇴일때
                if (bLate == "1" || bEarly == "1")
                {
                    // 오늘 정규 출,퇴근시간 가져오기
                    DateTime BaseWSTime = new DateTime();
                    DateTime bWCTime = new DateTime();

                    DAL.Attendance biz = new DAL.Attendance();
                    DataTableReader dtrT = biz.GetAttendanceTime(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, workDate, isHoli);
                    if (dtrT != null)
                    {
                        if (dtrT.HasRows)
                        {
                            while (dtrT.Read())
                            {
                                if (dtrT["BaseWSTime"].ToString() != "") //출근시간(출근인정범위 미포함)
                                {
                                    BaseWSTime = Convert.ToDateTime(dtrT["BaseWSTime"]);
                                }

                                if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                                {
                                    bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                                }
                            }
                        }
                    }

                    if (bLate == "1")
                    {
                        TimeSpan ts1 = (tmpWSTime - BaseWSTime);
                        info.bLateMinute = ((ts1.Hours * 60) + ts1.Minutes).ToString();
                    }

                    if (bEarly == "1")
                    {
                        TimeSpan ts2 = (bWCTime - tmpWCTime);
                        info.bEarlyMinute = ((ts2.Hours * 60) + ts2.Minutes).ToString();
                    }
                }

                if (lateRemark != "")
                {
                    info.LateRemark = lateRemark;
                }
                else
                {
                    info.LateRemark = null;
                }

                if (earlyRemark != "")
                {
                    info.EarlyRemark = earlyRemark;
                }
                else
                {
                    info.EarlyRemark = null;
                }

                if (wsConn != "")
                {
                    info.WSConn = wsConn;
                }
                else
                {
                    info.WSConn = null;
                }

                if (wcConn != "")
                {
                    info.WCConn = wcConn;
                }
                else
                {
                    info.WCConn = null;
                }

                info.IsHoli = isHoli;
                info.ModifyUserID = modifyUserID;
                info.ModifyReason = modReason;

                DAL.Attendance dal = new DAL.Attendance();
                dal.Mod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
