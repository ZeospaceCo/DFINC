using System;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Outlook
{
    public partial class InGoogleStep2 : Base.BasePageAddress
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        #region 변수설정
        public string CntAll = "0";
        public string CntExcept = "0";
        public string CntOverlap = "0";
        #endregion

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
                cbtnAdd.Text = resMng.GetString("Confirm");         // "확인";                
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            DAL.CSV dal = new DAL.CSV();
            DataTableReader dtr = dal.GetCSVCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        if (i == 1) CntAll = dtr["CNT"].ToString();
                        if (i == 2) CntExcept = dtr["CNT"].ToString();
                        if (i == 3) CntOverlap = dtr["CNT"].ToString();
                        i += 1;
                    }
                }
            }
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
        /// 접근 토큰
        /// </summary>
        private string AccessToken
        {
            get { return GetViewStateString("GoogleAccessToken", ""); }
            set { ViewState["GoogleAccessToken"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string csvIDs = tbxCSVID.Text.Trim();
                if (csvIDs.Length > 0)
                {
                    #region 글자수가 길어서, bundle 숫자만큼 잘라서 저장하자. shimpark

                    int indexStart = 0; //분리 시작위치
                    int indexEnd = 0; //분리 끝 위치
                    int bundle = 100; //100 개 단위로 잘라서 쓰자
                    char splitChr = ','; //글자 마지막 자를 특수문자
                    string result = ""; //자른 문자열 값
                    int totLen = csvIDs.Length; //총 원본 글자글 총수
                    int forCnt = totLen / bundle; //몫

                    DAL.CSV dal = new DAL.CSV();

                    for (int i = 0; i < forCnt + 1; i++)
                    {
                        if (indexStart + bundle > totLen) indexEnd = totLen - indexStart;
                        else indexEnd = csvIDs.Substring(indexStart, bundle).LastIndexOf(splitChr);

                        result = csvIDs.Substring(indexStart, indexEnd);
                        indexStart += indexEnd + 1;

                        dal.AddReceived(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, result);
                    }

                    #endregion 글자수가 길어서, bundle 숫자만큼 잘라서 저장하자. shimpark

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    paraDic.Add("alertMsg", GetResource("GoPageStep3"));
                    paraDic.Add("goURL", "/DeskplusEIP/Address/Outlook/InGoogleStep3.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion
    }
}