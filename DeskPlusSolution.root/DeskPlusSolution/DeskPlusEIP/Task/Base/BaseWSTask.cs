using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Base
{
    public class BaseWSTask : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
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
        /// 작성목적 : 생성자 생성
		/// </summary>
        public BaseWSTask()
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

        #region 함수

        /// <summary>
        /// Recs the status.
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려;6:지연
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Request.List).Assembly;	//어셈블리 선언

            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = "<div class=\"w_ready\"></div>" + GetResource("StandBy");
                    break;
                case "2":
                    response = "<div class=\"w_ing\"></div>" + GetResource("Receipt");
                    break;
                case "3":
                    response = "<div class=\"w_ing\"></div>" + GetResource("Processing");
                    break;
                case "4":
                    response = "<div class=\"w_end\"></div>" + GetResource("Complete");
                    break;
                case "5":
                    response = "<div class=\"w_end\"></div>" + GetResource("Reject");
                    break;
                case "6":
                    response = "<div class=\"w_delay\"></div>" + GetResource("Delay");
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
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Request.List).Assembly;	//어셈블리 선언

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