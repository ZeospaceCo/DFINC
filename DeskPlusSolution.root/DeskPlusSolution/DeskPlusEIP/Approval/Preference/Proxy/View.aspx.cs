using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Proxy
{
    public partial class View : Base.BasePageApproval
    {
        #region 변수처리

        protected DateTime dtTargetDate;
        public string TargetProxyID = String.Empty;
        public string IsProxy = String.Empty;
        public string AllDay = String.Empty;

        #endregion 변수처리

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Proxy.View).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-10
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            lbUserInfo.Text = CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "(" + CurrentUserInfo.TitleName + ")";

            if (TargetProxyID != "")
            {
                GetProxyInfo();
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Btn_Edit");
                cbtnList.Text = resMng.GetString("List");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            TargetProxyID = GetDecryptParam("targetProxyID", "");
        }

        // <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void GetProxyInfo()
        {
            DAL.Proxy biz = new DAL.Proxy();

            DataTableReader dtr = biz.GetProxyInfoByProxyID(CurrentUserInfo.CompanyID, TargetProxyID.ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbDate.Text = lbDate.Text = DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortTimeString() + " ~ " +
                                    DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortTimeString();

                        ProxyStartDate = dtr["ProxyStartDate"].ToString();
                        ProxyEndDate = dtr["ProxyEndDate"].ToString();

                        cbxAllDay.Checked = (dtr["AllDay"].ToString() == "Y") ? true : false;
                        AllDay = (dtr["AllDay"].ToString() == "Y") ? "1" : "0";

                        ProxyType = dtr["ProxyType"].ToString();

                        DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                        dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + dtr["ProxyType"].ToString() + "'";

                        foreach (DataRowView dr in dv)
                        {
                            lbProxyType.Text = this.GetLangName(
                                dr["NameBase"].ToString(),
                                dr["NameKo"].ToString(),
                                dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                dr["NameZhTw"].ToString(), PreLang);
                        }

                        lbContent.Text = dtr["ProxyContent"].ToString().Replace(Convert.ToString((char)10), @"<br>");

                        #region 다국어 처리
                        string ProxyNameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(),
                                dtr["NameZhTw"].ToString(), PreLang);

                        string ProxyDeptBase = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(),
                                dtr["DeptZhTw"].ToString(), PreLang);

                        string ProxyTitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(),
                                dtr["TitleZhTw"].ToString(), PreLang);
                        #endregion

                        lbProxyInfo.Text = ProxyDeptBase + " " + ProxyNameBase + "(" + ProxyTitleBase + ")";

                        IsProxy = dtr["IsProxy"].ToString();

                        hdIsCall.Value = dtr["IsCall"].ToString();

                        tbxProxyUserID.Text = dtr["ProxyUserID"].ToString();
                        tbxProxyDeptID.Text = dtr["ProxyDeptID"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            bool rtnVal = false;

            if (TargetProxyID.Length > 0)
            {
                BE.UserInfo userinfo = new BE.UserInfo();
                userinfo.CompanyID = CurrentUserInfo.CompanyID;
                userinfo.LogInID = CurrentUserInfo.LogInID;
                userinfo.UserID = CurrentUserInfo.UserID;
                userinfo.DeptID = CurrentUserInfo.DeptID;
                userinfo.Name = CurrentUserInfo.Name;
                userinfo.DeptName = CurrentUserInfo.DeptName;
                userinfo.TitleName = CurrentUserInfo.TitleName;
                userinfo.MailID = CurrentUserInfo.MailID;
                userinfo.MailDomain = CurrentUserInfo.MailDomain;
                userinfo.MobilePhone = CurrentUserInfo.MobilePhone;
                userinfo.PreferredLanguage = CurrentUserInfo.PreferredLanguage;

                BE.ProxyInfo info = new BE.ProxyInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ProxyID = TargetProxyID.ToNumber<int>();
                info.ProxyUserID = tbxProxyUserID.Text.ToNumber<int>();
                info.ProxyDeptID = tbxProxyDeptID.Text.ToNumber<int>();
                info.IsCall = hdIsCall.Value;

                info.ProxyContent = lbContent.Text;  // 부재내용

                string proxyPreferredLanguage = "";
                Account.DAL.User userdal = new Account.DAL.User();
                List<Account.BE.UserInfo> ulist = userdal.GetUserInfo(CurrentUserInfo.CompanyID, info.ProxyUserID, true, true);
                foreach (Account.BE.UserInfo pinfo in ulist)
                {
                    proxyPreferredLanguage = pinfo.PreferredLanguage;
                }

                if (proxyPreferredLanguage == "")
                {
                    proxyPreferredLanguage = "ko";
                }

                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + ProxyType + "'";

                foreach (DataRowView dr in dv)
                {
                    info.ProxyTypeStr = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                        dr["NameZhTw"].ToString(), proxyPreferredLanguage);
                }

                info.ProxyDateStr = DateHelper.GetCultureDate(DateTime.Parse(ProxyStartDate), proxyPreferredLanguage, true) + " ~ " + DateHelper.GetCultureDate(DateTime.Parse(ProxyEndDate), proxyPreferredLanguage, true);

                BLL.Proxy pBll = new BLL.Proxy();
                rtnVal = pBll.Del(userinfo, info);
            }

            return rtnVal;
        }

        #endregion

        #region (프로퍼티)

        public string ProxyStartDate
        {
            get { return GetViewStateString("ProxyStartDate", ""); }
            set { ViewState["ProxyStartDate"] = value; }
        }

        public string ProxyEndDate
        {
            get { return GetViewStateString("ProxyEndDate", ""); }
            set { ViewState["ProxyEndDate"] = value; }
        }

        public string ProxyType
        {
            get { return GetViewStateString("ProxyType", ""); }
            set { ViewState["ProxyType"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)
        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                bool rtnval = Del();
                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "View.List();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_FAIL"), "View.List();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnDel_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }
        }
        #endregion
    }
}