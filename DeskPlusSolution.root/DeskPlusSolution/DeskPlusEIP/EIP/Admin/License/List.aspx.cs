using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.EIP.Web.Admin.License
{
    public partial class List : Base.BasePageEIPAdmin
    {
        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.License.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                //슈퍼 관리자
                if (!CurrentUserInfo.IsSysAdmin)
                {
                    Response.Redirect("/DeskPlusEIP/");
                    return;
                }

                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 이현수
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            WebServerIP = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "WebServerIP", "127.0.0.1");
            MailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MailServerIP", "127.0.0.1");
            FullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "www.deskplus.co.kr");

            ActServerURL = String.Format("http://{0}/las/activation.asp", AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ActiveServer", "act.deskplus.co.kr"));

        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnComInfo.Text = resMng.GetString("CompanyManagement");
            }
        }
        #endregion



        #region (프로퍼티)
        /// <summary>
        /// ActServer URL
        /// </summary>
        public string ActServerURL
        {
            get { return GetViewStateString("ActServerURL", ""); }
            set { ViewState["ActServerURL"] = value; }
        }

        /// <summary>
        /// ActServer WebServerIP
        /// </summary>
        public string WebServerIP
        {
            get { return GetViewStateString("WebServerIP", ""); }
            set { ViewState["WebServerIP"] = value; }
        }

        /// <summary>
        /// ActServer MailServerIP
        /// </summary>
        public string MailServerIP
        {
            get { return GetViewStateString("MailServerIP", ""); }
            set { ViewState["MailServerIP"] = value; }
        }

        /// <summary>
        /// ActServer FullDomain
        /// </summary>
        public new string FullDomain
        {
            get { return GetViewStateString("FullDomain", ""); }
            set { ViewState["FullDomain"] = value; }
        }
        #endregion
    }
}