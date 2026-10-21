using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Proposal.BE;

namespace Zeospace.DeskPlus.Proposal.Web.Base
{
    public class BasePageProposal : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언
        public int StartYears;
        public int StartMonths;
        public int EndYears;
        public int EndMonths;
        public int CurrentPage = 0;
        public int JudgeGubun = 0;  // 심사구분
        public string SearchField = string.Empty;
        public string SearchValue = string.Empty;
        public string ListCount = string.Empty;//페이지사이즈
        public string Order = string.Empty; //sort
        public string JudgeAuth = string.Empty;  // 1차 심사자 권한
        
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

        public BasePageProposal()
        {
            this.Load += new System.EventHandler(this.BasePaseProposal_Load);
        }

        private void BasePaseProposal_Load(object sender, System.EventArgs e)
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
            StartYears = GetDecryptParam("startYears", System.DateTime.Now.Year.ToString()).ToNumber<int>();
            StartMonths = GetDecryptParam("startMonths", System.DateTime.Now.Month.ToString()).ToNumber<int>();
            EndYears = GetDecryptParam("endYears", System.DateTime.Now.Year.ToString()).ToNumber<int>();
            EndMonths = GetDecryptParam("endMonths", System.DateTime.Now.Month.ToString()).ToNumber<int>();
            JudgeGubun = GetDecryptParam("judgeGubun", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "Name");
            SearchValue = SearchValue = GetDecryptParam("searchValue", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            ListCount = GetDecryptParam("listCount", "10");
            Order = GetDecryptParam("order", "");
            JudgeAuth = GetDecryptParam("judgeAuth", "FALSE").ToString();
        }

        /// <summary>
        /// 게시판 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleProposal = CurrentUserInfo.IsInRole("proposal");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleProposal)
            {
                AccessAuth = true;
            }

            //if (!AccessAuth)
            //{
            //    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            //    return;
            //}
        }

        #region 함수

        #endregion
    }
}