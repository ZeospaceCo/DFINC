using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    public class BaseWSEducation : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
    {
        public string Skin { get; set; }
        public string ImgPath { get; set; }
        public string CssPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseWSEducation()
		{
            SetSkin();
		}                     

        /// <summary>
        /// 
        /// </summary>
        private void SetSkin()
        {
            try
            {
                string skin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

                CssPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");
                ImgPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (!CssPath.EndsWith("/")) CssPath += "/";
                if (!ImgPath.EndsWith("/")) ImgPath += "/";

                CssPath = CssPath + skin + "/Css";
                ImgPath = ImgPath + skin + "/Img";

            }
            catch
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";
            }
        }
    }
}