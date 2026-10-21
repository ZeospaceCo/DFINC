using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Account.BE;


namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class DeepFrame : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수설정

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.DeepFrame).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {

            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            string MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
            string URL_GateUrl = MailServerIP + Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_GateUrl", "/zeospace.ds");

            if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                string MailPassword = "";
                foreach (UserInfo info in al)
                {
                    MailPassword = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                }

                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new Common.BLL.MailDeepFactory();
                if (bll.AddUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, CurrentUserInfo.Name, "",""))
                {
                    if (PreLangReal.ToLower() == "ko") PreLangReal = "kr";
                    if (PreLangReal.ToLower() == "ja") PreLangReal = "jp";
                    if (PreLangReal.ToLower() == "zh-cn" || PreLangReal.ToLower() == "zh-tw") PreLangReal = "cn";

                    //string path = bll.SLORead(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, PreLang, "1");
                    string path = URL_GateUrl + "?act=sso&domain=" + CurrentUserInfo.MailDomain + "&id=" + CurrentUserInfo.MailID + "&pw=" + MailPassword + "&language=" + PreLangReal + "&lp=mailList&mbox=1&msgkey=&to=&content=&hideTopMenu=1&isPop=0";
                    Response.Write(path);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("DeepFrame - SLO(DEEP호출URL) : " + path, AppSettingType.MAIL);
                    if (path.IndexOf("act=sso") > 0)
                    {
                        Response.Redirect(path);
                    }
                }
            }


        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}