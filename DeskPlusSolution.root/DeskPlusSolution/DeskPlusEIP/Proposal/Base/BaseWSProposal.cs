using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Proposal.Web.Base
{
    public class BaseWSProposal : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
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

        #region Construct (생성자)
		/// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 생성자 생성
		/// </summary>
        public BaseWSProposal()
		{
            SetSkin();
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
        /// 게시판 관리자 여부 확인하기
        /// </summary>
        /// <returns></returns>
        protected bool GetAccessAuth()
        {
            bool response = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleProposal = CurrentUserInfo.IsInRole("proposal");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleProposal) response = true;

            return response;
        }

        /// <summary>
        /// 작성자 : shine
        /// 진행상황 값 가져오기
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            string status = "";

            switch (_status)
            {
                case "1":
                    status = "제안 신청중";
                    break;
                case "2":
                    status = "1차 심사중";
                    break;
                case "3":
                    status = "1차 평가완료";
                    break;
                case "4":
                    status = "1차 등급처리";
                    break;
                case "9":
                    status = "제안 기각";
                    break;
                default:
                    status = "";
                    break;

            }

            return status;
        }
    }
}