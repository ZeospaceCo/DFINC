using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Proxy
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSApproval
    {
        /// <summary>
        /// 지정한 대결자 등록되어있는지 조회
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetProxyByUserID(string p)
        {
            try
            {
                SetDecryptString(p);

                int CompanyID = CurrentUserInfo.CompanyID;
                int UserID = CurrentUserInfo.UserID;
                int DeptID = CurrentUserInfo.DeptID;
                string ProxyUserID = GetDecryptParam("proxyUserID", "0");
                string ProxyDeptID = GetDecryptParam("proxyDeptID", "0");
                string ProxyID = GetDecryptParam("targetProxyID", "0");

                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetProxyUser(CompanyID, UserID, DeptID, ProxyUserID.ToNumber<int>(), ProxyDeptID.ToNumber<int>(), ProxyID.ToNumber<int>());

                bool rlt = true;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        rlt = false;
                    }
                }

                return rlt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 대결자 중복 기간 체크
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetProxySelByDate(string p)
        {
            try
            {
                SetDecryptString(p);

                int CompanyID = CurrentUserInfo.CompanyID;
                int UserID = CurrentUserInfo.UserID;
                int DeptID = CurrentUserInfo.DeptID;
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string sTime = GetDecryptParam("sTime", "");
                string eTime = GetDecryptParam("eTime", "");
                string ProxyID = GetDecryptParam("targetProxyID", "0");

                string startDateStr = DateHelper.GetCultureDate(startDate.Split(',')[0] + " " + sTime, "yyyy-MM-dd HH:mm:ss", PreLangReal);
                string endDateStr = DateHelper.GetCultureDate(endDate.Split(',')[0] + " " + eTime, "yyyy-MM-dd HH:mm:ss", PreLangReal);

                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetProxySelByDate(CompanyID, UserID, DeptID, startDateStr, endDateStr, ProxyID.ToNumber<int>());

                bool rlt = true;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        rlt = false;
                    }
                }

                return rlt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

    }
}
