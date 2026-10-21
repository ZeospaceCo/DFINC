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
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
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

                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetProxyUser(CompanyID, UserID, DeptID, ProxyUserID.ToNumber<int>(), ProxyDeptID.ToNumber<int>(), 0);

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

                string startDateStr = DateHelper.GetCultureDate(startDate.Split(',')[0] + " " + sTime, "yyyy-MM-dd HH:mm:ss", PreLangReal);
                string endDateStr = DateHelper.GetCultureDate(endDate.Split(',')[0] + " " + eTime, "yyyy-MM-dd HH:mm:ss", PreLangReal);

                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetProxySelByDate(CompanyID, UserID, DeptID, startDateStr, endDateStr, 0);

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
        /// 휴가원에서 대결자 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddProxyUser(string p)
        {
            try
            {
                string rlt = "-1";

                SetDecryptString(p);
                
                string companyid = GetDecryptParam("companyid", "0");
                string userid = GetDecryptParam("userid", "0");
                string deptid = GetDecryptParam("deptid", "0");
                string proxyuserid = GetDecryptParam("proxyuserid", "0");
                string proxydeptid = GetDecryptParam("proxydeptid", "0");
                string proxystartdate = GetDecryptParam("proxystartdate", "");
                string proxyenddate = GetDecryptParam("proxyenddate", "");
                string WkItemCd = GetDecryptParam("WkItemCd", "");
                string reason = GetDecryptParam("reason", "");

                BE.UserInfo userinfo = new BE.UserInfo();

                Account.DAL.User dal = new Account.DAL.User();
                List<Account.BE.UserInfo> list = dal.GetUserInfo(companyid.ToNumber<int>(), userid.ToNumber<int>(), true, false);
                foreach (Account.BE.UserInfo info in list)
                {
                    if (deptid.ToNumber<int>() == info.DeptID)
                    {
                        userinfo.CompanyID = info.CompanyID;
                        userinfo.UserID = info.UserID;
                        userinfo.DeptID = info.DeptID;
                        userinfo.MailID = info.MailID;
                        userinfo.MailDomain = info.MailDomain;
                        userinfo.MobilePhone = info.MobilePhone;
                        userinfo.PreferredLanguage = info.PreferredLanguage;

                        #region 부서명 다국어 처리
                        userinfo.DeptName = UtilHelper.GetLangName(
                                info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                info.DeptZhTw.ToNull(), info.PreferredLanguage);
                        #endregion

                        #region 직급명 다국어 처리
                        userinfo.TitleName = UtilHelper.GetLangName(
                                        info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                        info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                        info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                        info.TitleZhTw.ToNull(), info.PreferredLanguage);
                        #endregion

                        #region 사용자명 다국어 처리
                        userinfo.Name = UtilHelper.GetLangName(
                                        info.NameBase.ToNull(), info.NameKo.ToNull(),
                                        info.NameEn.ToNull(), info.NameJa.ToNull(),
                                        info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                        info.NameZhTw.ToNull(), info.PreferredLanguage);
                        #endregion
                    }
                }


                BE.ProxyInfo be = new ProxyInfo();
                be.CompanyID = companyid.ToNumber<int>();
                be.UserID = userid.ToNumber<int>();
                be.DeptID = deptid.ToNumber<int>();
                be.ProxyUserID = proxyuserid.ToNumber<int>();
                be.ProxyDeptID = proxydeptid.ToNumber<int>();
                be.IsProxy = "N";

                // 부재기간
                be.ProxyStartDate = Convert.ToDateTime(proxystartdate);
                be.ProxyEndDate = Convert.ToDateTime(proxyenddate);
                be.AllDay = "N"; // 종일 체크 여부
                be.ProxyType = WkItemCd; // 부재유형
                be.ProxyContent = reason;  // 부재내용
                be.IsCall = "Y"; //대결 예약 대기
                be.WriteUserID = userid.ToNumber<int>();

                string proxyPreferredLanguage = "";
                Account.DAL.User userdal = new Account.DAL.User();
                List<Account.BE.UserInfo> ulist = userdal.GetUserInfo(be.CompanyID, be.ProxyUserID, true, true);
                foreach (Account.BE.UserInfo pinfo in ulist)
                {
                    proxyPreferredLanguage = pinfo.PreferredLanguage;
                }

                if (proxyPreferredLanguage == "")
                {
                    proxyPreferredLanguage = "ko";
                }

                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + be.ProxyType + "'";

                foreach (DataRowView dr in dv)
                {
                    be.ProxyTypeStr = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                        dr["NameZhTw"].ToString(), proxyPreferredLanguage);
                }

                be.ProxyDateStr = DateHelper.GetCultureDate(be.ProxyStartDate, proxyPreferredLanguage, true) + " ~ " + DateHelper.GetCultureDate(be.ProxyEndDate, proxyPreferredLanguage, true);

                BLL.Proxy pBll = new BLL.Proxy();
                if (pBll.Add(userinfo, be))
                {
                    rlt = "0";
                }

                return rlt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "-1";
            }
        }

    }
}
