using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    public class BasePageEducation : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        public int CurrentPage = 0;
        public int ListID = 0;
        public string SearchItem = String.Empty;
        public string SearchStatus = String.Empty;
        public string SearchComplete = String.Empty;        
        public string SearchDateField = String.Empty;
        public string SearchDateValue = String.Empty;
        public string SearchField = String.Empty;
        public string SearchValue = String.Empty;

        public string Skin { get; set; }
        public string ImgPath { get; set; }
        public string CssPath { get; set; }
        public string MultiLang { get; set; }
        protected bool AccessAuth { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public BasePageEducation()
        {
            this.Load += new System.EventHandler(this.BasePageWebHard_Load);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePageWebHard_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            SetSkin();
            GetAccessAuth();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
        }

        /// <summary>
        /// 
        /// </summary>
        private void BaseParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchItem = GetDecryptParam("searchItem", "");
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchComplete = GetDecryptParam("searchComplete", "");
            SearchDateField = GetDecryptParam("searchDateField", "");
            SearchDateValue = GetDecryptParam("searchDateValue", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
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
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");
            bool isRoleEducation = CurrentUserInfo.IsInRole("Education");

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleEducation)
            {
                AccessAuth = true;
            }
        }
    }
}