using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Base
{
    public class BasePageBoard : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언

        /// <summary>
        /// 메뉴에서 호출한 카테고리값 최상단 트리 아이디
        /// </summary>
        public int MenuCatID = 0;

        /// <summary>
        /// 트리에서 선택한 카테고리 값 (그룹용 또는 폴더용)
        /// </summary>
        public int CatID = 0;

        /// <summary>
        /// 리스트 목록에서 선택한 리스트 실제 카테고리값 (폴더용)
        /// </summary>
        public int CategoryID = 0;

        public int CurrentPage = 0;
        public string SearchGroup = string.Empty; //분류
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값
        public string Order = string.Empty; //sort
        public string ListType = string.Empty; //리스트 형식
        public string Dcparm = string.Empty; //메뉴의 dcparm 값
        public string PGubun = string.Empty; //모아보기 구분값

        /// <summary>
        /// 즐겨찾기 및 카테고리 구분
        /// </summary>
        public string Types = "";

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

        /// <summary>
        /// 다국어 입력 필드 설정값
        /// </summary>
        public string MultiLang { get; set; }

        #endregion 속성 선언

        public BasePageBoard()
        {
            this.Load += new System.EventHandler(this.BasePageBoard_Load);
        }

        private void BasePageBoard_Load(object sender, System.EventArgs e)
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
            MenuCatID = GetDecryptParam("menuCatID", "0").ToNumber<int>();
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchGroup = GetDecryptParam("searchGroup", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Types = GetDecryptParam("types", "");
            Order = GetDecryptParam("order", "");
            ListType = GetDecryptParam("listType", "");//리스트형식
            Dcparm = GetDecryptParam("dcparm", ""); //메뉴의 dcparm (포틀릿 추가용도)
            PGubun = GetDecryptParam("pGubun", "N"); //모아보기에서 접속했는 지 구분용도
        }

        /// <summary>
        /// 게시판 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleBoard)
            {
                AccessAuth = true;
            }

            //if (!AccessAuth)
            //{
            //    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            //    return;
            //}
        }

        /// <summary>
        /// 게시판 회사 아이디
        /// </summary>
        public int BoaComID
        {
            get { return GetViewStateInt("BoaComID", "0"); }
            set { ViewState["BoaComID"] = value; }
        }
    }
}