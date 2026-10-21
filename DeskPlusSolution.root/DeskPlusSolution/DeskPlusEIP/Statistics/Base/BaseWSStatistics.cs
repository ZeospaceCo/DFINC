using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;


namespace Zeospace.DeskPlus.Statistics.Web.Base
{
    public class BaseWSStatistics : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
    {
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
        /// 전체도메인(url)정보
        /// </summary>
        public string FullUrl { get; set; }

        /// <summary>
        /// 관리자 여부
        /// </summary>
        public bool IsAdmin { get; set; }

        #region Construct (생성자)
        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 생성자 생성
        /// </summary>
        public BaseWSStatistics()
        {
            SetSkin();
            GetAccessAuth();
            FullUrl = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "http://") + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "localhost") + ":" + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttpPort", "80");
        }


        #endregion

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
            catch
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 결재 관리자 여부 확인하기
        /// </summary>
        /// <returns></returns>
        protected void GetAccessAuth()
        {
            IsAdmin = false;

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleStatistics = CurrentUserInfo.IsInRole("Statistics");//Statistics admin

            // 별도의 통계 Role 이 없음으로 그냥 통과 시켜준다.
            isRoleStatistics = true;

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleStatistics) IsAdmin = true;
        }


    }
}