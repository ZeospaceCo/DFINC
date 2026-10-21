using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Base.Web.Site.Auth
{
    public partial class ModPassword : System.Web.UI.Page
    {
        #region 변수설정

        protected System.Resources.ResourceManager resMng = null;

        public string ResourceName { get; set; }

        public Assembly AssemblyName { get; set; }

        private string decryString = string.Empty;//복고화 값

        public string PwdLength = "1";
        public string PwdLower = "0";
        public string PwdUpper = "0";
        public string PwdNumeric = "0";
        public string PwdSpecial = "0";
        public string PwdFirst = "0";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                PreLangs = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);
            }

            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Site.Auth.ModPassword).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            GetResourceManager();

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            PwdLength = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdLength", "PwdRegex", "ko");
            PwdLower = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdLower", "PwdRegex", "ko");
            PwdUpper = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdUpper", "PwdRegex", "ko");
            PwdNumeric = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdNumeric", "PwdRegex", "ko");
            PwdSpecial = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdSpecial", "PwdRegex", "ko");
            PwdFirst = AppSettingHelper.GetSysCode(GetViewStateString("CompanyID", "0").ToNumber<int>(), "PwdFirst", "PwdRegex", "ko");
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            ViewState["CompanyID"] = GetDecryptParam("companyID", "0").ToNumber<int>();
            ViewState["LoginID"] = GetDecryptParam("loginID", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                this.cbtnConfirm.Text = resMng.GetString("Alert_Ok");
                this.cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

        private void SetDecryptString()
        {
            string param = GetParm("p", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)

        protected void lbtnSubmit_Click(object sender, System.EventArgs e)
        {
            string nowPwd = ctbPassword.Text.Trim().ToNull();
            string newPwd = ctbNewPassword.Text.Trim().ToNull();
            string comparePwd = ctbNewPassword1.Text.Trim().ToNull();

            if (nowPwd.Length == 0 || newPwd.Length == 0 || comparePwd.Length == 0)
            {
                ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "COMPARE_PWD", GetResource("InputRequest"), "");
                return;
            }

            if (newPwd != comparePwd)
            {
                //신규 패스워드 서로 다름
                ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "COMPARE_PWD", GetResource("InputComparePwd"), "");
                return;
            }
            else
            {
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                int userID = dal.SignIn(GetViewStateString("CompanyID", "0").ToNumber<int>(), GetViewStateString("LoginID", ""), nowPwd, false);
                if (userID > 0)
                {
                    string mailPassword = newPwd;


                    //메일 패스워드 변경
                    if (mailPassword.Length > 0)
                    {
                        string mailID = "";
                        string mailDomain = "";
                        string nameBase = "";
                        string mailDesign = "";

                        var uList = dal.GetUserByLoginID(GetViewStateString("CompanyID", "0").ToNumber<int>(), GetViewStateString("LoginID", ""), true);
                        foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in uList)
                        {
                            mailID = info.MailID;
                            mailDomain = info.MailDomain;
                            nameBase = info.NameBase;
                            mailDesign = info.MailDesign;
                        }

                        if (mailID.Length > 0 &&
                            mailDomain.Length > 0 &&
                            nameBase.Length > 0)
                        {
                            #region 메일 패스워드 변경하기

                            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

                            if (mailProduct.ToUpper() == "ZETTA65")
                            {
                                Zeospace.DeskPlus.Common.BLL.MailZettaFactory bizMail = new DeskPlus.Common.BLL.MailZettaFactory();

                                string encryptType = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "EncryptType", "");
                                if (encryptType.ToUpper() == "SHA1")
                                {
                                    //메일 패스워드를 다시 암호화 해야함..
                                    string encPwd = HashManager.GetHash(mailPassword.ToNull());
                                    string mailAddress = mailID + "@" + mailDomain;
                                    //관리자 선택한 사용자의 user id 를 가지고 mailAddress 를 가져옴
                                    bizMail.ChangePwdUser(GetViewStateString("CompanyID", "0").ToNumber<int>(), mailAddress, encPwd);
                                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메일 패스워드업데이트(관리자) mailAddress, mailPassword: " + mailAddress + ", " + mailPassword, AppSettingType.MAIL);
                                }
                            }
                            else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                            {
                                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();

                                Zeospace.DeskPlus.Account.DAL.UserMailDomain dalMail = new Zeospace.DeskPlus.Account.DAL.UserMailDomain();
                                var mailList = dalMail.GetUserMailDomain(GetViewStateString("CompanyID", "0").ToNumber<int>(), mailID, mailDomain);

                                string oldpw = "";
                                foreach (var info in mailList) { oldpw = info.MailPassword; }
                                string encPwd = HashManager.GetHash(mailPassword.ToNull());

                                bool rtnval = bizMail.ModUser(GetViewStateString("CompanyID", "0").ToNumber<int>(), mailDomain, mailID, oldpw, encPwd, nameBase, "", "", mailDesign, true);
                                if (!rtnval)
                                {
                                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/WS/Admin/User/Password/Change.asmx (비밀번호 변경 실패) : mailID(" + mailID + "), mailDomain(" + mailDomain + ")" + "oldPW(" + oldpw + "), newPW(" + mailPassword + ")");
                                }

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/WS/Admin/User/Password/Change.asmx : mailID(" + mailID + "), mailDomain(" + mailDomain + ")" + "oldPW(" + oldpw + "), newPW(" + mailPassword + ")", AppSettingType.MAIL);
                            }

                            #endregion 메일 패스워드 변경하기
                        }
                    }

                    dal.ModPwd(GetViewStateString("CompanyID", "0").ToNumber<int>(), userID, newPwd, mailPassword);

                    List<UserInfo> userList = dal.GetUserInfo(GetViewStateString("CompanyID", "0").ToNumber<int>(), userID, false, true);

                    #region 타 제품 패스워드 연동 (ERP, BPM)

                    //타 제품 패스워드 연동 (ERP, BPM)
                    Account.BLL.SLOPwdChange biz = new Account.BLL.SLOPwdChange();
                    biz.ModPwdChange(GetViewStateString("CompanyID", "0").ToNumber<int>(), userID, userList[0].LogInID, newPwd);

                    UpdateMessengerUserInfo();
                    #endregion 타 제품 패스워드 연동 (ERP, BPM)

                    ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "SUCCESS", GetResource("LoginNewPwd"), "ModPassword.Close();");
                }
                else if (userID == -1)
                {
                    // 접속명 혹은 비밀번호를 확인해 주십시오.
                    ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH1", resMng.GetString("CheckIDandPwd"), "");
                    return;
                }
                else if (userID == -2)
                {
                    // 부서 비활성화로 인증할 수 없습니다. 관리자에게 문의하시기 바랍니다.
                    ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH2", resMng.GetString("CheckAuthDept"), "");
                    return;
                }
            }
        }

        public void UpdateMessengerUserInfo()
        {
            
            string QueryCode = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "QueryCode", "Y");
            string Messengersubmit = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "Messengersubmit", "Y");
            string MessengerURL = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessengerURL", "Y");

            //보류... 기존 코드 주석..
            // string[] paramNameArr = new string[] { "CMD", "UserIDS", "QueryCode", "submit" };
            // string[] paramValueArr = new string[] { "UPDATE_USER", CurrentUserInfo.LogInID, QueryCode, Messengersubmit };
            string[] paramNameArr = new string[] { "CMD", "UserIDS", "CMD_Option", "Contents", "Contents_Encode", "submit" };
            string[] paramValueArr = new string[] { "LOGOUT_USER", GetViewStateString("LoginID", ""), "EXIT=N,PWD=CLEAR", "Messenger Logout", "UTF-8", "메신저로그아웃" };

            string actionURL = MakeParam(MessengerURL, paramNameArr, paramValueArr);
            SendPost(actionURL);
        }

        /// <summary>
        /// Post 형식으로 발송 하기위한 작업
        /// </summary>
        /// <param name="_targetPath"></param>
        /// <param name="_paramName"></param>
        /// <param name="_paramValue"></param>
        /// <returns></returns>
        public string MakeParam(string _targetPath, string[] _paramName, string[] _paramValue)
        {
            System.Text.StringBuilder targetURL = new System.Text.StringBuilder();
            targetURL.Append("http://");
            targetURL.Append(_targetPath);

            for (int i = 0; i < _paramName.Length; i++)
            {
                if (i == 0)
                {
                    targetURL.Append("?");
                }
                else
                {
                    targetURL.Append("&");
                }

                targetURL.Append(_paramName[i] + "=" + System.Web.HttpUtility.UrlEncode(_paramValue[i], System.Text.Encoding.GetEncoding("euc-kr")));
            }

            return targetURL.ToString();
        }

        private void SendPost(string _mURL)
        {
            try
            {
                string[] url = _mURL.Split('?');
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url[0]);
                req.Method = "POST";
                req.Accept = "application/xml";
                req.ContentType = "application/x-www-form-urlencoded";

                System.Uri surl = new Uri(_mURL);

                string query = surl.Query.Substring(1);
                byte[] buf = System.Text.Encoding.GetEncoding(949).GetBytes(query);
                req.ContentLength = buf.Length;
                req.GetRequestStream().Write(buf, 0, buf.Length);
                req.GetRequestStream().Close();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ModPassword 메신저 비밀번호 변경 완료 " + surl, AppSettingType.MESSENGER);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModPassword 포스트 발송 에러 (change.asmx.cs) : " + ex.ToString());
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("_mURL : " + _mURL);
                throw;
            }
        }

        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        private string PreLangs
        {
            get { return ViewState["PreLangs"].ToString(); }
            set
            {
                string lang = "";

                if (value == "")
                {
                    lang = Request.UserLanguages[0].ToLower();
                }
                else
                {
                    lang = value.ToLower();
                }

                if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans" || lang == "zh-sg")  //zh-Hans(중국어간체)
                {
                    lang = "zh-cn";
                }
                else if (lang == "zh-tw" || lang == "zh-hant" || lang == "zh-mo")  //번체
                {
                    lang = "zh-tw";
                }
                else if (lang == "ja" || lang == "ja-jp") //일본어
                {
                    lang = "ja";
                }
                else if (lang == "ko" || lang == "ko-kr") //한국어
                {
                    lang = "ko";
                }
                else //나머지는 영어
                {
                    lang = "en";
                }

                ViewState["PreLangs"] = lang;
                //ViewState["PreLangs"] = value;
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE STRING 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetViewStateString(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (string)(ViewState[_viewStateName] ?? _errorReturn);
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToString(ViewState[_viewStateName]) : Convert.ToString(_errorReturn);
            }
        }

        /// <summary>
        /// 공통 CSS
        /// </summary>
        public string BaseCssPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssPath", "/DeskPlusEIP/Design/C/Css"); }
        }

        /// <summary>
        /// 공통 Img
        /// </summary>
        public string BaseImgPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImgPath", "/DeskPlusEIP/Design/C/Img"); }
        }

        #endregion Property (프로퍼티)
    }
}