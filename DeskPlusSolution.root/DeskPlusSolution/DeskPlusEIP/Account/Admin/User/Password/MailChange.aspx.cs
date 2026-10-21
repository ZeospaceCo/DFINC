using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.Admin.User.Password
{
    public partial class MailChange : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID;
        public int UserID;
        public string MailID;
        public string MailDomain;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.Password.MailChange).Assembly;	//어셈블리 선언
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

            DAL.User dal = new DAL.User();
            List<BE.UserInfo> al = dal.GetUserInfo(CompanyID, UserID, true, true);

            foreach (BE.UserInfo info in al)
            {
                MailID = info.MailID;
                //관리자페이지에서 패스워드 변경시 메일도메인도 필요함..
                MailDomain = info.MailDomain;
            }

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
                CbtnConfirm.Text = resMng.GetString("BtnConfirm");
                CbtnClose.Text = resMng.GetString("BtnClose");
            }
        }
        #endregion
    }
}