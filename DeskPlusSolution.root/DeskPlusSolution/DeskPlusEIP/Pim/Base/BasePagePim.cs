using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Base
{
    public class BasePagePim : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 속성 선언   
        protected bool AccessAuth { get; set; } //권한 접근 여부

        /// <summary>
        /// 스킨정보
        /// </summary>
        public string Skin { get; set; }
        /// <summary>
        /// 이미지 경로 프로퍼티
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// css 경로 프로퍼티
        /// </summary>
        public string CssPath { get; set; }

        /// <summary>
        /// 다국어 입력 필드 설정값
        /// </summary>
        public string MultiLang { get; set; }

        //목록
        public string PageScope = string.Empty;
        public string Date = string.Empty;
        public string Opentype = string.Empty; 
        
        #endregion

        public BasePagePim()
		{
            this.Load += new System.EventHandler(this.BasePagePim_Load);
		}


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            //목록
            PageScope = GetDecryptParam("PageScope", "");
            Date = GetDecryptParam("Date", "");
            Opentype = GetDecryptParam("Opentype", "");
        }


        private void BasePagePim_Load(object sender, System.EventArgs e)
        {
            SetSkin();
            GetAccessAuth();
            LoadParam();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
        }

        /// <summary>
        /// pim 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRolePim = CurrentUserInfo.IsInRole("pim");//pim admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRolePim)
            {
                AccessAuth = true;
            }

            //if (!AccessAuth)
            //{
            //    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            //    return;
            //}
        }


        /// <summary>
        /// 스킨 설정하기
        /// </summary>
        private void SetSkin()
        {
            try
            {
                // 이미지파일 경로 설정하기
                // ex) DeskPlusEIP/Design/[LANGUAGE]/[TYPE]/images 
                string skin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

                CssPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");
                ImgPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (!CssPath.EndsWith("/")) CssPath += "/";
                if (!ImgPath.EndsWith("/")) ImgPath += "/";

                CssPath = CssPath + skin + "/Css";
                ImgPath = ImgPath + skin + "/Img";

            }
            catch (Exception ex)
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";

                //로그에 남기기				
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSkin : " + Request.Url.ToString() + " : " + ex.Message);
            }
        }
    }
}