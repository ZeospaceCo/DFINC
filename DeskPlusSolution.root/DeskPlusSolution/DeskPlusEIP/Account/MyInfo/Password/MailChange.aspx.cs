using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Account.Web.MyInfo.Password
{
    public partial class MailChange : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID;
        public int UserID;

        public string PwdLength = "1";
        public string PwdLower = "0";
        public string PwdUpper = "0";
        public string PwdNumeric = "0";
        public string PwdSpecial = "0";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Password.MailChange).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
            LoadParam();

            PwdLength = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PwdLength", "PwdRegex", CurrentUserInfo.PreferredLanguage);
            PwdLower = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PwdLower", "PwdRegex", CurrentUserInfo.PreferredLanguage);
            PwdUpper = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PwdUpper", "PwdRegex", CurrentUserInfo.PreferredLanguage);
            PwdNumeric = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PwdNumeric", "PwdRegex", CurrentUserInfo.PreferredLanguage);
            PwdSpecial = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PwdSpecial", "PwdRegex", CurrentUserInfo.PreferredLanguage);
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
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
    }
}