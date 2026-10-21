using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class AddressBook : Base.BasePageCommon
    {
        #region 변수설정
        //to, cc, bc
        public string Gubun = String.Empty;
        public string MailServerIP;
        //메일 로그인
        public string MailLogin;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.AddressBook).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");

            //if (Request.Cookies["ss_webmail_id"]["Mail"] != null)
            //{
            //    Mail = Request.Cookies["ss_webmail_id"]["Mail"].ToString();
            //    LoginCheck();
            //}
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Alert_Ok");     //"확인";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                cbtnSearch.Text = resMng.GetString("Searh");       //"검색";
                cbtnAdd.Text = resMng.GetString("Add");        //"추가";


                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("UserName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FaxNumber"), "Fax"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetParm("type", "to");
            MailLogin = GetParm("loginID", "");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}