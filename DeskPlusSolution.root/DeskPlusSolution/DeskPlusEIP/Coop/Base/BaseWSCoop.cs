using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Base
{
    public class BaseWSCoop : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
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
        public BaseWSCoop()
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
            //bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin) response = true;

            return response;
        }

        #region 함수

        /// <summary>
        /// Recs the status.
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.Mod).Assembly;	//어셈블리 선언

            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = "<p class=\"b_cwr\">&nbsp;" + GetResource("StandBy") + "</p>";
                    break;
                case "2":
                    response = "<p class=\"b_cwi\">&nbsp;" + GetResource("Receipt") + "</p>";
                    break;
                case "3":
                    response = "<p class=\"b_cwi\">&nbsp;" + GetResource("Processing") + "</p>";
                    break;
                case "4":
                    response = "<p class=\"b_cwe\">&nbsp;" + GetResource("Complete") + "</p>";
                    break;
                case "5":
                    response = "<p class=\"b_cwre\">&nbsp;" + GetResource("Reject") + "</p>";
                    break;
            }


            return response;
        }

        /// <summary>
        /// 난이도
        /// </summary>
        /// <param name="diff"></param>
        /// <returns></returns>
        public string GetDifficulty(string diff)
        {
            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.Mod).Assembly;	//어셈블리 선언

            if (diff.Length == 0) return "";
            string response = "";

            switch (diff)
            {
                case "1":
                    response = GetResource("Top");
                    break;
                case "2":
                    response = GetResource("Medium");
                    break;
                case "3":
                    response = GetResource("Bad");
                    break;
            }


            return response;
        }




        #endregion
    }
}