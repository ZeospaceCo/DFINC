using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Xml;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Attendance의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Attendance : Base.BaseWSPortlet
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttendanceData(string p)
        {
            string rtnAttendanceData = "|||";

            try
            {
                SetDecryptString(p);
                string selDate = GetDecryptParam("selDate", "").ToString();

                List<object> weatherList = new List<object>();

                DAL.Attendance dal = new DAL.Attendance();
                DataTableReader dtr = dal.GetAttendanceData(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, selDate);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnAttendanceData = dtr["bWS"].ToString() + "|" + dtr["WSTime"].ToString() + "|" + dtr["bWC"].ToString() + "|" + dtr["WCTime"].ToString();
                        }
                    }
                }

                return rtnAttendanceData;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-04-24
        /// 작성목적 : 출근설정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetGotoWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string reason = GetDecryptParam("reason", "").ToString();

                DAL.Attendance biz = new DAL.Attendance();

                biz.SetGotoWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, reason);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetGotoWork : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-04-24
        /// 작성목적 : 퇴근설정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetOffWork(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string reason = GetDecryptParam("reason", "").ToString();

                DAL.Attendance biz = new DAL.Attendance();

                biz.SetOffWork(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, reason);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetGotoWork : " + ex);
                return false;
            }
        }
    }
}
