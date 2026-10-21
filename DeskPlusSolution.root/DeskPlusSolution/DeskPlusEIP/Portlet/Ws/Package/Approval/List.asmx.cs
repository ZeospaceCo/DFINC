using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Portlet.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Approval
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPortlet
    {
        #region 변수
        #endregion

        /// <summary>
        /// 결재대상 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetApproveList(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();

            try
            {
                Zeospace.DeskPlus.Portlet.DAL.Approval dalApp = new Zeospace.DeskPlus.Portlet.DAL.Approval();

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");
                DataTableReader dtr = null;
                if (version.ToLower() == "3.3")
                {
                    dtr = dalApp.GetApproveList33(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                }
                else
                {
                    dtr = dalApp.GetApproveList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, 1);
                }

                List<object> appObj = new List<object>();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ApprovalInfo appInfo = new ApprovalInfo();
                            appInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            appInfo.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            appInfo.Number2 = dtr["Number2"].ToString();
                            appInfo.NumberFull = dtr["NumberFull"].ToString();
                            appInfo.Subject = dtr["Subject"].ToString();
                            appInfo.WriteTime = Convert.ToDateTime(dtr["WriteTime"].ToString()).ToShortTimeString(); 
                            appInfo.AccountUserName = dtr["AccountUserName"].ToString();
                            appInfo.AccountDeptName = dtr["AccountDeptName"].ToString();
                            appInfo.AccountUser = dtr["AccountUser"].ToString();
                            appInfo.ApprovalState = dtr["ApprovalState"].ToString();
                            appInfo.AccessTitleID = dtr["AccessTitleID"].ToString().ToNumber<int>();
                            appInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                            appInfo.AccountTag = dtr["AccountTag"].ToString();
                            appInfo.Sequences = dtr["Sequences"].ToString();
                            appInfo.AccountUserID = dtr["AccountUserID"].ToString().ToNumber<int>();
                            appInfo.AccountDeptID = dtr["AccountDeptID"].ToString().ToNumber<int>();
                            appInfo.CPDeptID = dtr["CPDeptID"].ToString().ToNumber<int>();
                            appInfo.CPDeptName = dtr["CPDeptName"].ToString();
                            appObj.Add(appInfo);
                        }
                    }
                }
                //return new List<object>();

                return appObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetApproveList] : " + ex.Message);
                return null;
                //throw;
            }
        }



        /// <summary>
        /// 상신문서 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReportList(string p)
        {
            SetDecryptString(p);

            int pageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();

            try
            {
                Zeospace.DeskPlus.Portlet.DAL.Approval dalApp = new Zeospace.DeskPlus.Portlet.DAL.Approval();

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");
                DataTableReader dtr = null;
                if (version.ToLower() == "3.3")
                {
                    dtr = dalApp.GetReportList33(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                }
                else
                {
                    dtr = dalApp.GetReportList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, 1);
                }

                List<object> appObj = new List<object>();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ApprovalInfo appInfo = new ApprovalInfo();
                            appInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            appInfo.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            appInfo.Number2 = dtr["Number2"].ToString();
                            appInfo.NumberFull = dtr["NumberFull"].ToString();
                            appInfo.Subject = dtr["Subject"].ToString();
                            appInfo.WriteTime = Convert.ToDateTime(dtr["WriteTime"].ToString()).ToShortTimeString(); 
                            appInfo.AccountUserName = dtr["AccountUserName"].ToString();
                            appInfo.AccountDeptName = dtr["AccountDeptName"].ToString();
                            appInfo.AccountUser = dtr["AccountUser"].ToString();
                            appInfo.ApprovalState = dtr["ApprovalState"].ToString();
                            appInfo.AccessTitleID = dtr["AccessTitleID"].ToString().ToNumber<int>();
                            appInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                            appInfo.AccountTag = dtr["AccountTag"].ToString();
                            appInfo.Sequences = dtr["Sequences"].ToString();
                            appInfo.AccountUserID = dtr["AccountUserID"].ToString().ToNumber<int>();
                            appInfo.AccountDeptID = dtr["AccountDeptID"].ToString().ToNumber<int>();
                            appInfo.CPDeptID = dtr["CPDeptID"].ToString().ToNumber<int>();
                            appInfo.CPDeptName = dtr["CPDeptName"].ToString();
                            appObj.Add(appInfo);
                        }
                    }
                }
                return appObj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetApproveList] : " + ex.Message);
                throw;
            }
        }
    }
}
