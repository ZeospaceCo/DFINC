using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Address
{
    public partial class View : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string gubun = "";
        public string ownerID = "";
        public string searchValue = "";
        public int pageNo = 1;
        public string fastPath = "";
        public string firstChar = "";
        public string mailCheck = "N";
        public string myTalkCheck = "N";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Address.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

                LoadAuth();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            gubun = GetDecryptParam("gubun", "G");
            ownerID = GetDecryptParam("ownerID", "");
            searchValue = GetDecryptParam("sv", "");

            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            fastPath = GetDecryptParam("FastPath", "0");
            firstChar = GetDecryptParam("FirstChar", "");
        }

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAuth()
        {
            if (UserInfo.MenuRoles != null)
            {
                for (int i = 0; i < UserInfo.MenuRoles.Length; i++)
                {
                    if (UserInfo.MenuRoles[i] == "1")
                    {
                        string useMail = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USEMAIL");
                        if (useMail.ToUpper() == "TRUE")
                        {
                            mailCheck = "Y";
                        }
                    }

                    if (UserInfo.MenuRoles[i] == "8")
                    {
                        myTalkCheck = "Y";
                    }
                }
            }
        }

        #endregion
    }
}