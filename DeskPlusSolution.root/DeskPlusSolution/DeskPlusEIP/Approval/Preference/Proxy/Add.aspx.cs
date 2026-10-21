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
    public partial class Add : Base.BasePageApproval
    {
        #region 변수처리

        protected DateTime dtTargetDate;

        #endregion 변수처리

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Proxy.Add).Assembly;	//어셈블리 선언
            #endregion

            #region 날짜 세팅

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

            #endregion 날짜 세팅

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
            SetStartEndTime();

            lbUserInfo.Text = CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + "(" + CurrentUserInfo.TitleName + ")";
            tbxUserID.Text = CurrentUserInfo.UserID.ToString();
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
                cbtnConfirm.Text = resMng.GetString("Btn_Save");      //"확인";
                cbtnCancel.Text = resMng.GetString("Cancel");     //"취소";

                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                ddlAbsenceType.Items.Clear();
                ddlAbsenceType.Items.Add(new ListItem(this.resMng.GetString("MSG_SelItem"), ""));

                for (int i = 0; i < dv.Count; i++)
                {
                    string name = this.GetLangName(
                        dv[i]["NameBase"].ToString(),
                        dv[i]["NameKo"].ToString(),
                        dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                        dv[i]["NameZhTw"].ToString(), PreLang);

                    ddlAbsenceType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
                }
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 :
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            ddlstartTime.Items.Clear();
            ddlendTime.Items.Clear();

            DateTime dtFromTime = dtTargetDate;
            DateTime dtToTime = dtTargetDate.AddDays(1);

            while (dtFromTime < dtToTime)
            {
                //직접 바인딩
                ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlstartTime.Items.Add(tmpItemFrom);

                ListItem tmpItemTo = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlendTime.Items.Add(tmpItemTo);

                dtFromTime = dtFromTime.AddMinutes(30);
            }

            ddlstartTime.SelectedIndex = 18;
            ddlendTime.SelectedIndex = ddlstartTime.SelectedIndex + 1;
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private bool Addition()
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

            string userID = tbxProxyUserID.Text;
            string deptID = tbxProxyDeptID.Text;

            BE.ProxyInfo be = new ProxyInfo();

            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            be.DeptID = CurrentUserInfo.DeptID;

            be.ProxyUserID = Convert.ToInt32(userID);
            be.ProxyDeptID = Convert.ToInt32(deptID);

            be.IsProxy = "N";

            // 부재기간
            DateTime dtStart = Convert.ToDateTime(startDate.Value);
            DateTime dtEnd = Convert.ToDateTime(endDate.Value);

            be.ProxyStartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
            be.ProxyEndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);            

            be.AllDay = (cbxAllDay.Checked) ? "Y" : "N"; // 종일 체크 여부

            be.ProxyType = ddlAbsenceType.SelectedValue; // 부재유형
            be.ProxyContent = AbsenceContent.InnerText;  // 부재내용
            be.IsCall = "Y"; //대결 예약 대기

            be.WriteUserID = CurrentUserInfo.UserID;

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
            dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + ddlAbsenceType.SelectedValue + "'";

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
            return pBll.Add(userinfo, be);
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        ///
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = Addition();
                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Add.List();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_FAIL"), "Add.List();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }
        }
        #endregion
    }
}