using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Common.BLL;


namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class SendMailDeep : Base.BasePageBoard
    {

        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public string listID, categoryID;
        
        #endregion

        #region Page Process (페이지 처리)


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.SendMail).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                GetParam();
                SetLanguage();
                LoadData();
            }
        }



        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            string contents = "";
            string subjects = "";
            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());
            Zeospace.DeskPlus.Board.DAL.Lists dalList = new DAL.Lists();
            var list = dalList.GetList(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
            foreach (var info in list)
            {
                contents = info.Content;
                subjects = info.Title;

            }

            string MailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
            //URL_GateUrl = MailServerIP + AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_GateUrl", "/zeospace.ds");
            URL_GateUrl = MailServerIP + "/zeospace.ds";

            string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                string MailPassword = "";
                foreach (UserInfo info in al)
                {
                    MailPassword = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                }

                MailDeepFactory bll = new MailDeepFactory();
                if (bll.AddUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, CurrentUserInfo.Name, "", ""))
                {
                    MailDeep55 deep = new MailDeep55();

                    //act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}

                    subject.Value = subjects;
                    act.Value = "sso";
                    domain.Value = CurrentUserInfo.MailDomain;
                    id.Value = CurrentUserInfo.MailID;
                    pw.Value = MailPassword;
                    language.Value = AppSettingHelper.GetDescription(deep.GetLang(PreLang));
                    lp.Value = AppSettingHelper.GetDescription(MailDeep55.UserLP.MailWrite);
                    mbox.Value = string.Empty;
                    msgkey.Value = string.Empty;
                    to.Value = "";
                    content.Value = contents.Replace("localhost", FullDomain).Replace("\r\n", "");
                    hideTopMenu.Value = "1";

                    content.Value = contents.Replace("localhost", FullDomain).Replace("\r\n", "").Replace("\"", "'");


                }
            }

        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void GetParam()
        {
            listID = GetDecryptParam("listID", "");
            categoryID = GetDecryptParam("categoryID", "");

        }

        #endregion

        #region (속성)
        public string URL_GateUrl
        {
            get { return GetViewStateString("URL_GateUrl", ""); }
            set { ViewState["URL_GateUrl"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}