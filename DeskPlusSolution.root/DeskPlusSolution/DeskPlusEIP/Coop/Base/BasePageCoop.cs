using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Coop.Web.Base
{
    public class BasePageCoop : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언                
       
        public int ListID;
        public int GroupID;
        public int CurrentPage = 0;        
        public string SearchField = string.Empty; //검색키
        public string SearchValue = string.Empty; //검색값        
        public string SearchStatus = string.Empty;//상태값
        public string SearchDateValue = string.Empty;//검색날짜(시작,종료)
        public string ListCount = string.Empty;//페이지사이즈
        public string Order = string.Empty; //sort

        public int ItemID = 0; //1차분류
        public int ItemSubID = 0;//2차분류


        #endregion

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


        #endregion

        public BasePageCoop()
		{
            this.Load += new System.EventHandler(this.BasePageCoop_Load);
		}

        private void BasePageCoop_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            GetAccessAuth();
            SetSkin();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
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

        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void BaseParam()
        {  
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "0"); 
            SearchField = GetDecryptParam("searchField", "");            
            SearchValue = GetDecryptParam("searchValue", "");
            SearchDateValue = GetDecryptParam("searchDateValue", "");
            Order = GetDecryptParam("order", "");
            ListCount = GetDecryptParam("listCount", "5");
            ItemID = GetDecryptParam("itemID", "0").ToNumber<int>(); //1차분류
            ItemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>(); //2차분류
        }

        /// <summary>
        /// 게시판 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            //bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator

            ////나의 일지 경우
            //if (TargetUserID == CurrentUserInfo.UserID) AccessAuth = true;

            //if (!AccessAuth)
            //{
            //    //보고승인자 확인하기
            //}

            //if (!AccessAuth)
            //{
            //    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            //    return;
            //}

            
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
            if (_status.Length == 0) return "";
            string response = "";

            switch (_status)
            {
                case "1":
                    response = GetResource("StandBy");
                    break;
                case "2":
                    response = GetResource("Receipt");
                    break;
                case "3":
                    response = GetResource("Processing");
                    break;
                case "4":
                    response = GetResource("Complete");
                    break;
                case "5":
                    response = GetResource("Reject");
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