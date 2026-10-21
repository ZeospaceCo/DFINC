using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base
{
    /// <summary>
    /// 인증자 정보 설정
    /// </summary>
    public abstract class BaseService : UserEnvironmentBaseService
    {
        private string decryptString = string.Empty;//복고화 파리미터값

        #region Construct (생성자)

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 작성목적 : 생성자 생성
        /// </summary>
        public BaseService()
        {
            base.HaveEnvAuthUser();
        }

        #endregion Construct (생성자)

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 작성목적 : 사용자 정보 가져오기
        /// </summary>
        /// <value>The current user info.</value>
        protected UserInfo CurrentUserInfo
        {
            get { return userInfo; }
        }

        /// <summary>
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        /// <param name="_encParam"></param>
        /// <returns></returns>
        protected void SetDecryptString(string _encParam)
        {
            if (!string.IsNullOrEmpty(_encParam))
            {
                _encParam = Server.UrlDecode(_encParam);
                decryptString = Utility.DecryptString(_encParam);
                decryptString = Server.UrlDecode(decryptString);
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
            if (!string.IsNullOrEmpty(decryptString))
            {
                return UtilHelper.GetParmforParameter(decryptString, _keyName, _errorReturn);
            }

            return _errorReturn;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 다국어별 언어 데이터 가져오기
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        protected string GetLangName(string _name, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            return UtilHelper.GetLangName(_name, _ko, _en, _ja, _fr, _zhcn, _zhtw, _preLang);
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// HTTP, HTTPS (http://)
        /// </summary>
        /// <returns></returns>
        protected string BaseHttp
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "http://"); }
        }

        /// <summary>
        /// 도메인 (ep.zeospace.com)
        /// </summary>
        /// <returns></returns>
        protected string FullDomain
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", ""); }
        }

        /// <summary>
        /// 포트 (80)
        /// </summary>
        public string BaseHttpPort
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttpPort", "80"); }
        }

        /// <summary>
        /// 도메인 전체 경로 (포트 포함) 80 은 생략
        /// </summary>
        public string FullDomainPath
        {
            get
            {
                if (BaseHttpPort != "80")
                {
                    return BaseHttp + FullDomain + ":" + BaseHttpPort;
                }

                return BaseHttp + FullDomain;
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
    }
}