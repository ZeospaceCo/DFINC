using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Opinion.Web.Base
{
    public class BasePageOpinionAdmin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언
        protected string MenuID = string.Empty;     //메뉴(질문:Unit or 예시:exam / 진행:qing or 마감:qclosing)
        protected int PageSize      = 0;            //보여질 게시물수
        protected int PageNo        = 0;            //현재페이지
        protected string SPeriod    = string.Empty; //기간검색
        protected string SPeriodVal = string.Empty; //기간검색어
        protected string SKey       = string.Empty; //검색키워드
        protected string SVal       = string.Empty; //검색어
        protected string Order      = string.Empty; //정렬
        protected string ListType   = string.Empty; //리스트 형식
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

        public BasePageOpinionAdmin()
        {
            this.Load += new EventHandler(this.BasePageOpinionAdmin_Load);
        }

        private void BasePageOpinionAdmin_Load(object sender, System.EventArgs e)
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
            MenuID = GetDecryptParam("menuID", "exam");         //기본페이지:예시, 메뉴(질문:Unitor예시:exam)
            PageNo = GetDecryptParam("pageNo", "1").ToNumber<int>();
            PageSize = GetDecryptParam("pageSize", "5").ToNumber<int>();
            SPeriod = GetDecryptParam("sPeriod", "");
            SPeriodVal = GetDecryptParam("sPeriodValue", "");
            SKey = GetDecryptParam("sKey", "");
            SVal = GetDecryptParam("sVal", "");
            Order = GetDecryptParam("order", "");
            ListType = GetDecryptParam("listType", "");
        }

        /// <summary>
        /// 설문조사 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");       //administrator
            bool isRoleOpinion = CurrentUserInfo.IsInRole("opinion");   //opinion admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleOpinion)
            {
                AccessAuth = true;
            }

            if (!AccessAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
        }
        
        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-01 
        /// 작성목적 : Xml에서 해당 정보 가져오기
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public string GetAdminMenuByID(string _id, string _name)
        {
            try
            {
                DataRow[] dr = GetAdminMenu().Tables[0].Select("ID = '" + _id + "'");
                return dr[0][_name].ToString();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAdminMenuByID[Menu] : " + ex);
                return "";
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-01 
        /// 작성목적 : 다국어에 맞게 XML 가져오기
        /// </summary>
        private DataSet GetAdminMenu()
        {
            DataSet dsAdminMenu = (DataSet)Session["dsOpinionAdminMenu"];

            if (dsAdminMenu == null)
            {
                dsAdminMenu = new DataSet();

                try
                {
                    string xmlPath = "";
                    try
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/Opinion/App_Data/AdminMenu/" + PreLang + ".xml");
                    }
                    catch
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/Board/App_Data/AdminMenu/en.xml");
                    }
                    dsAdminMenu.ReadXml(xmlPath);
                    Session["dsOpinionAdminMenu"] = dsAdminMenu;
                }
                finally
                {
                }
            }

            return dsAdminMenu;
        }
        #endregion
    }
}