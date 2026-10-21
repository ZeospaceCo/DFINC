using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class TabMain : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        public string TabPortletTitle = "";
        public string TabPortletFirst = "";
        public string ParamValueList = "";
        public string UrlValueList = "";
        public string WebmailURL = "";

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.TabMain).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {
            LoadTabPortlet();

        }
        #endregion

        #region (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 탭포틀릿의 포틀릿 타이틀 출력하기
        /// </summary>
        private void LoadTabPortlet()
        {
            /*
            Zeospace.DeskPlus.EIP.DAL.PortletTab ptDal = new EIP.DAL.PortletTab();
            List<Zeospace.DeskPlus.EIP.BE.PortletTabInfo> alTabPortlet = ptDal.GetPortletTab(CurrentUserInfo.CompanyID, PortletID.ToNumber<int>());

            string targetPortletList = string.Empty;
            foreach (Zeospace.DeskPlus.EIP.BE.PortletTabInfo info in alTabPortlet)
            {
                targetPortletList += info.TargetPortletID + ",";
            }
            */

            string targetPortletList = TargetPortletID;

            if (!string.IsNullOrEmpty(targetPortletList)) targetPortletList = targetPortletList.TrimEnd(',');

            if (targetPortletList.Length > 0)
            {
                Zeospace.DeskPlus.EIP.DAL.Portlet dal = new EIP.DAL.Portlet();
                List<Zeospace.DeskPlus.EIP.BE.PortletInfo> alPortletList = dal.GetPortletMyRoleList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    targetPortletList, string.Join(",", CurrentUserInfo.RoleIDs), PortletID.ToNumber<int>());

                StringBuilder sb = new StringBuilder();
                StringBuilder pvSb = new StringBuilder();
                StringBuilder urlSb = new StringBuilder();

                string jsEvent = "LoadSelTab_" + TabPortletID + "_" + PortletID;
                for (int i = 0; i < alPortletList.Count; i++)
                {
                    if (alPortletList[i].TPortletID.ToString() == PortletID)
                    {
                        if (TabPortletFirst.Length == 0) TabPortletFirst = alPortletList[i].PortletID.ToString();

                        var firstTabCss = (TabPortletFirst == alPortletList[i].PortletID.ToString()) ? "btn-secondary" : "";

                        sb.Append(string.Format(@"<span><a id='{0}' class=""btn btn-sm {4} "" onclick='{1}({2})'>{3}</a></span>",
                            "seltemptab_" + TabPortletID + "_" + PortletID + "_" + alPortletList[i].PortletID.ToString(),
                            jsEvent,
                            alPortletList[i].PortletID,
                            this.GetLangName(alPortletList[i].NameBase, alPortletList[i].NameKo, alPortletList[i].NameEn,
                                alPortletList[i].NameJa, alPortletList[i].NameFr, alPortletList[i].NameZhcn, alPortletList[i].NameZhtw, PreLang),
                                firstTabCss));

                        //if (i != alPortletList.Count - 1) sb.Append("<li>|</li>");

                        pvSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].ParamValue);
                        if (i != alPortletList.Count - 1) pvSb.Append(",");

                        // PortletID, UIURL
                        urlSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].UIURL);
                        if (i != alPortletList.Count - 1) urlSb.Append(",");
                    }
                }

                TabPortletTitle = sb.ToString();
                ParamValueList = pvSb.ToString();
                UrlValueList = urlSb.ToString();

                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                string MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
                string URL_GateUrl = "";
                if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    URL_GateUrl = MailServerIP + Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_GateUrl", "/zeospace.ds");
                    Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                    List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                    string MailPassword = "";
                    foreach (UserInfo info in al)
                    {
                        MailPassword = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                    }

                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new Common.BLL.MailDeepFactory();
                    if (bll.AddUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, CurrentUserInfo.Name, "", ""))
                    {
                        if (PreLangReal.ToLower() == "ko") PreLangReal = "kr";
                        if (PreLangReal.ToLower() == "ja") PreLangReal = "jp";
                        if (PreLangReal.ToLower() == "zh-cn" || PreLangReal.ToLower() == "zh-tw") PreLangReal = "cn";

                        //string path = bll.SLORead(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, PreLang, "1");
                        string path = URL_GateUrl + "?act=sso&domain=" + CurrentUserInfo.MailDomain + "&id=" + CurrentUserInfo.MailID + "&pw=" + MailPassword + "&language=" + PreLangReal + "&lp=mailList&mbox=1&msgkey=&to=&content=&hideTopMenu=1&isPop=0";
                        //Response.Write(path);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("DeepFrame - SLO(DEEP호출URL) : " + path, AppSettingType.MAIL);
                        if (path.IndexOf("act=sso") > 0)
                        {
                            WebmailURL = path;
                        }
                    }
                }
                else if (mailProduct.ToUpper().IndexOf("ZETTA65").Equals(0))
                {
                    URL_GateUrl = MailServerIP + "/zmail/login.nvd";
                    string path = URL_GateUrl + "?cmd=receive_mailbox&user_account=" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                    WebmailURL = path;
                }
            }
        }
        #endregion

        #region (프로퍼티)

        #endregion
    }
}