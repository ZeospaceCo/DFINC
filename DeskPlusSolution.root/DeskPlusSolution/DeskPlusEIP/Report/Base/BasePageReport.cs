using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Base
{
    public class BasePageReport : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언

        public int Years;
        public int Months;
        public int Days;
        public int ListID;

        /// <summary>
        /// 접속할 업무일자 사용자아이디
        /// </summary>
        public int TargetUserID;

        /// <summary>
        /// 접속할 업무일자 사용자 부서아이디
        /// </summary>
        public int TargetDeptID;

        public string Weeks = string.Empty;//몇주(전체:0;1주:1~~)
        public string Flag = string.Empty;// 요청확인 구분자(전체:0;요청~)
        public string SearchField = string.Empty; //검색키
        public string SearchValue = string.Empty; //검색값
        public string ListType = string.Empty; //리스트 형식
        public int FolderID;

        #endregion 변수 선언

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

        #endregion 속성 선언

        public BasePageReport()
        {
            this.Load += new System.EventHandler(this.BasePageReport_Load);
        }

        private void BasePageReport_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            GetAccessAuth();
            SetSkin();
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
            //MenuCatID = GetDecryptParam("menuCatID", "0").ToNumber<int>();
            Years = GetDecryptParam("years", System.DateTime.Now.Year.ToString()).ToNumber<int>();
            Months = GetDecryptParam("months", System.DateTime.Now.Month.ToString()).ToNumber<int>();
            Days = GetDecryptParam("days", System.DateTime.Now.Day.ToString()).ToNumber<int>();
            TargetUserID = GetDecryptParam("targetUserID", CurrentUserInfo.UserID.ToString()).ToNumber<int>();
            TargetDeptID = GetDecryptParam("targetDeptID", CurrentUserInfo.DeptID.ToString()).ToNumber<int>();
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();

            Weeks = GetDecryptParam("weeks", "0");
            Flag = GetDecryptParam("flag", "0");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            ListType = GetDecryptParam("listType", "PrivateList");
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
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
    }
}