using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Opinion.BE;

namespace Zeospace.DeskPlus.Opinion.Web.Base
{
    public class BasePageOpinion : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언
        protected string MenuID = string.Empty; //메뉴(질문:Unit or 예시:exam / 진행:qing or 마감:qclosing)
        protected int PageSize = 0;             //보여질 게시물수
        protected int PageNo = 0;               //현재페이지
        protected string SPeriod = string.Empty;    //기간검색
        protected string SPeriodVal = string.Empty; //기간검색어
        protected string SKey = string.Empty;   //검색키워드
        protected string SVal = string.Empty;   //검색어
        protected string Order = string.Empty;  //정렬
        protected string ListType = string.Empty; //리스트 형식 일반리스트(1) / 블로그(2) / 앨범(3)
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
        #endregion

        public BasePageOpinion()
        {
            this.Load += new EventHandler(this.BasePageOpinion_Load);
        }

        private void BasePageOpinion_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            GetAccessAuth();

            SetSkin();
        }

        #region 함수

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
        /// 파리미터처리
        /// </summary>
        private void BaseParam()
        {
            MenuID = GetDecryptParam("menuID", "IngVoteNon");  //기본페이지:미참여리스트
            PageNo = GetDecryptParam("pageNo", "1").ToNumber<int>();
            PageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();
            SPeriod = GetDecryptParam("sPeriod", "");
            SPeriodVal = GetDecryptParam("sPeriodValue", "");
            SKey = GetDecryptParam("sKey", "");
            SVal = GetDecryptParam("sVal", "");
            Order = GetDecryptParam("order", "");
            ListType = GetDecryptParam("listType", "1");
        }

        /// <summary>
        /// 게시판 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleOpinion = CurrentUserInfo.IsInRole("opinion");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleOpinion)
            {
                AccessAuth = true;
            }
        }
        #endregion
    }
}