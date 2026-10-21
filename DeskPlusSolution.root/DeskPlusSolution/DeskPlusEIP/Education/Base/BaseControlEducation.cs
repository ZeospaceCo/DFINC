using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    public class BaseControlEducation : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseControl
    {
        public string Skin { get; set; }
        public string ImgPath { get; set; }
        public string CssPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseControlEducation()
        {
            this.Load += new System.EventHandler(this.BaseControlTask_Load);
            this.Error += new System.EventHandler(this.BaseControlTask_Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseControlTask_Load(object sender, System.EventArgs e)
        {
            SetSkin();
        }

        /// <summary>
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
            catch (Exception ex)
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";
	
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSkin : " + Request.Url.ToString() + " : " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseControlTask_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }
    }
}