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
    /// ViewHistory의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewHistory : Base.BaseWSHR
    {
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-12-15
        /// 작성목적 : 출퇴근시간 히스토리 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAttendanceHistory(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			        //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Content.Popup.ViewHistory).Assembly;	    //어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);

                int attID = GetDecryptParam("attID", "0").ToNumber<int>();

                DAL.History dal = new DAL.History();
                DataTableReader dtr = dal.GetHistoryList(CurrentUserInfo.CompanyID, attID);

                List<object> obj = new List<object>();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttendanceInfo info = new BE.AttendanceInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.AttID = dtr["AttID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString();
                            info.WSTime = (dtr["WSTime"].ToString() == "") ? resMng.GetString("NoInput") : dtr["WSTime"].ToString();
                            info.WCTime = (dtr["WCTime"].ToString() == "") ? resMng.GetString("NoInput") : dtr["WCTime"].ToString();
                            if (dtr["WSConn"].ToString() == "IN")
                            {
                                info.WSConn = resMng.GetString("IN");
                            }
                            else if (dtr["WSConn"].ToString() == "OUT")
                            {
                                info.WSConn = resMng.GetString("OUT");
                            }
                            else
                            {
                                info.WSConn = "";
                            }

                            if (dtr["WCConn"].ToString() == "IN")
                            {
                                info.WCConn = resMng.GetString("IN");
                            }
                            else if (dtr["WCConn"].ToString() == "OUT")
                            {
                                info.WCConn = resMng.GetString("OUT");
                            }
                            else
                            {
                                info.WCConn = "";
                            }
                            info.bLate = dtr["bLate"].ToString();
                            info.bEarly = dtr["bEarly"].ToString();
                            info.LateRemark = dtr["LateRemark"].ToString().ToNull().ToReverseXSS();
                            info.EarlyRemark = dtr["EarlyRemark"].ToString().ToNull().ToReverseXSS();
                            info.ModifyUserID = dtr["ModifyUserID"].ToString();
                            info.ModifyNameBase = dtr["ModifyNameBase"].ToString();
                            info.ModifyReason = dtr["ModifyReason"].ToString().ToNull().ToReverseXSS();

                            DateTime dt;

                            if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                                info.ModifyDateStr = DateHelpers.GetCultureDate(dt, PreLangReal, true);
                            else
                                info.ModifyDateStr = string.Empty;

                            obj.Add(info);
                        }
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

    }
}
