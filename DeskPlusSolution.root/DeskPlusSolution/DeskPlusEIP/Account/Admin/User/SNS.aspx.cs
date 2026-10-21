using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class SNS : Base.BasePageAccountAdmin
    {
        #region 변수설정
        public int UserID = 0;
        public string Facebook;
        public string FacebookPwd;
        public string Twitter;
        public string TwitterPwd;
        public string Me2day;
        public string Me2dayPwd;
        public string Google;
        public string GooglePwd;
        public string NaverBlog;
        public string NaverBlogPwd;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.SNS).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     :
        /// 최초작성일 :
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ctbFacebook.Text = Facebook;
            ctbFacebookPwd.Attributes.Add("value", FacebookPwd);
            ctbTwitter.Text = Twitter;
            ctbTwitterPwd.Attributes.Add("value", TwitterPwd);
            ctbMe2day.Text = Me2day;
            ctbMe2dayPwd.Attributes.Add("value", Me2dayPwd);
            ctbGoogle.Text = Google;
            ctbGooglePwd.Attributes.Add("value", GooglePwd);
            ctbNaverBlog.Text = NaverBlog;
            ctbNaverBlogPwd.Attributes.Add("value", NaverBlogPwd);
        }


        /// <summary>
        /// 파라미터 설정
        /// </summary>
        private void LoadParam()
        {
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            Facebook = GetDecryptParam("facebook", "");
            FacebookPwd = GetDecryptParam("facebookPwd", "");
            Twitter = GetDecryptParam("twitter", "");
            TwitterPwd = GetDecryptParam("twitterPwd", "");
            Me2day = GetDecryptParam("me2day", "");
            Me2dayPwd = GetDecryptParam("me2dayPwd", "");
            Google = GetDecryptParam("google", "");
            GooglePwd = GetDecryptParam("googlePwd", "");
            NaverBlog = GetDecryptParam("NaverBlog", "");
            NaverBlogPwd = GetDecryptParam("NaverBlogPwd", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = this.resMng.GetString("BtnConfirm");
                CbtnClose.Text = this.resMng.GetString("BtnClose");
            }
        }
        
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}