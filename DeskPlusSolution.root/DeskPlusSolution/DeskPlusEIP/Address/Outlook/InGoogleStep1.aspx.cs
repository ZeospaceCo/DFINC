using System;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth;
using System.Configuration;


namespace Zeospace.DeskPlus.Address.Web.Outlook
{
    public partial class InGoogleStep1 : Base.BasePageAddress
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Outlook.InGoogleStep2).Assembly;	//어셈블리 선언
            #endregion

            if (this.TokenManager != null)
            {
                if (!IsPostBack)
                {
                    var google = new WebConsumer(GoogleConsumer.ServiceDescription, this.TokenManager);

                    // Is Google calling back with authorization?
                    var accessTokenResponse = google.ProcessUserAuthorization();
                    if (accessTokenResponse != null)
                    {
                        AccessToken = accessTokenResponse.AccessToken;
                    }
                    else if (AccessToken == null)
                    {
                        // If we don't yet have access, immediately request it.
                        GoogleConsumer.RequestAuthorization(google, GoogleConsumer.Applications.Contacts);
                    }


                    SetLanguage();
                    LoadData();
                }
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                             
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            
        }

        #endregion

        #region (함수)

        private InMemoryTokenManager TokenManager
        {
            get
            {
                var tokenManager = (InMemoryTokenManager)Application["GoogleTokenManager"];
                if (tokenManager == null)
                {
                    string consumerKey = ConfigurationManager.AppSettings["googleConsumerKey"];
                    string consumerSecret = ConfigurationManager.AppSettings["googleConsumerSecret"];
                    if (!string.IsNullOrEmpty(consumerKey))
                    {
                        tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
                        Application["GoogleTokenManager"] = tokenManager;
                    }
                }

                return tokenManager;
            }
        }

        #endregion

        #region (속성)

        /// <summary>
        /// 
        /// </summary>
        public string AccessToken
        {
            get { return (string)Session["GoogleAccessToken"]; }
            set { Session["GoogleAccessToken"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        

        #endregion
        
    }
}