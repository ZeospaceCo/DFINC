using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Approval.BE;

namespace Zeospace.DeskPlus.Approval.Web.Base
{
    public class BaseWSApproval : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
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
        public BaseWSApproval()
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
            bool isRoleApproval = CurrentUserInfo.IsInRole("approval");//approval admin
            bool isRoleForm = CurrentUserInfo.IsInRole("formadmin");//approval admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleApproval || isRoleForm) IsAdmin = true;
        }

        /// <summary>
        /// 사용자(알람용도:발송자정보,첨언포함)
        /// </summary>
        /// <param name="_approvalType"></param>
        /// <param name="_opinion"></param>
        /// <returns></returns>
        public UserInfo GetSendUserInfo(string _approvalType, string _opinion)
        {
            UserInfo info = new UserInfo();
            info.UserID = CurrentUserInfo.UserID;
            info.Name = CurrentUserInfo.Name;
            info.DeptID = CurrentUserInfo.DeptID;
            info.DeptName = CurrentUserInfo.DeptName;
            info.TitleName = CurrentUserInfo.TitleName;
            info.DutyName = CurrentUserInfo.DutyName;
            info.Mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            info.MobilePhone = CurrentUserInfo.MobilePhone;
            info.PreLang = CurrentUserInfo.PreferredLanguage;
            info.ApprovalType = _approvalType;
            info.Opinion = _opinion;

            return info;
        }
    }
}