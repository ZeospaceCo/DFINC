using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : BaseControl
    /// </summary>
    public abstract class BaseControl : UserEnvironmentBaseControl
    {
        private string decryString = string.Empty;//복고화 값

        #region Construct (생성자)

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-08
        /// 작성목적 : 생성자 생성
        /// </summary>
        public BaseControl()
        {
            this.Load += new System.EventHandler(this.BaseControl_Load);
            this.Error += new System.EventHandler(this.BaseControl_Error);
        }

        #endregion Construct (생성자)

        /// <summary>
        ///   <see cref="E:System.Web.UI.Control.Init"/> 이벤트를 발생시킵니다.
        /// </summary>
        /// <param name="e">이벤트 데이터가 들어 있는 <see cref="T:System.EventArgs"/> 개체입니다.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-08
        /// 작성목적 : M : 현재 Client의 Culture 정보 가져오기 메소드
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseControl_Load(object sender, System.EventArgs e)
        {
            base.HaveEnvAuthUser();
            SetDecryptString();
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-08
        /// 작성목적 : M : 에러 정보쓰기 메소드
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseControl_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-08
        /// 작성목적 : 사용자 정보 가져오기
        /// </summary>
        /// <value>The current user info.</value>
        protected UserInfo CurrentUserInfo
        {
            get { return userInfo; }
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
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        /// <param name="_encParam"></param>
        /// <returns></returns>
        protected void SetDecryptString()
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

        /// <summary>
        /// 작성자 : 심재운
        /// 환경정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_keyID">The _key ID.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetEnvKeyValue(int _companyID, AppSettingType _type, string _keyID, string _errorReturn)
        {
            if (_companyID > 0 && _keyID.Length > 0)
            {
                return Helper.AppSettingHelper.GetEnvKeyValue(_companyID, _type, _keyID);
            }
            else
            {
                return _errorReturn;
            }
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
        /// 작성자 : 심재운
        /// 멀티 언어 문자열 반환(콤마)
        /// </summary>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        protected string GetMuiltiLang(params object[] _list)
        {
            return UtilHelper.GetMuiltiLang(_list);
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

        #endregion Page Process (페이지 처리)
    }
}