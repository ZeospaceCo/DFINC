using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.PMS.Web.Base
{
    public class BasePagePMS : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        #region 변수 선언        
        public int CurrentPage = 0;
        public int RecordPerPage = 0;
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값        
        public string Order = string.Empty; //sort

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

        /// <summary>
        /// 0:권한없음, 1:게스트, 2:사용자, 3:관리자 
        /// </summary>
        private int authority;
        
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="BasePagePMS"/> class.
        /// </summary>
        public BasePagePMS()
        {
            this.Load += new System.EventHandler(this.BasePagePMS_Load);
            this.Error += new System.EventHandler(this.BasePagePMS_Error);
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePagePMS_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 권한 체크
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BasePagePMS_Load(object sender, System.EventArgs e)
        {
            BaseParam();
            SetSkin();
            GetAccessAuth();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');

            if (ProjectID == 0 && RqProjectID == 0) return;

            authority = 0;//0:권한없음, 1:게스트, 2:사용자, 3:관리자 
            string srcipt = "";
            bool response = false;
            Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();

            if (ProjectID > 0 && RqProjectID == 0) //프로젝트
            {
                response = lib.UseProject(CurrentUserInfo.CompanyID, ProjectID, CurrentUserInfo.UserID);
                if (response) authority = 2; //사용자
            }
            else if (ProjectID > 0 && RqProjectID > 0) //연관프로젝트
            {
                srcipt = "top.close();";

                //연관 프로젝트 여부
                bool linkResponse = lib.LinkProject(CurrentUserInfo.CompanyID, ProjectID, RqProjectID);

                if (linkResponse)
                {
                    response = lib.UseProject(CurrentUserInfo.CompanyID, RqProjectID, CurrentUserInfo.UserID);
                    if (response) authority = 1; //게스트
                }
            }

            if (!response)
            {
                ErrorMSG("no authority", srcipt);
            }
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
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");

            if(RecordPerPage == 0)  RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10").ToNumber<int>();//페이지리스트수

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-10
        /// 작성목적 : 자원관리 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRolePMS = CurrentUserInfo.IsInRole("pms");//pms admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRolePMS)
            {
                AccessAuth = true;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : ErrorMSG
        /// 작성자     : 
        /// 최초작성일 : 2009-02-04
        /// </summary>
        /// <param name="_msg">The _MSG.</param>
        private void ErrorMSG(string _msg, string _script)
        {
            if (_script == "") _script = "history.back();";

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", _msg, _script);

            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.ClientMessage.Alert(_msg);
            //Response.Write("<script language='javascript'>");
            //Response.Write(_script);
            //Response.Write("</script>");
            //Response.End();
        }

        #region Property (프로퍼티)
        /// <summary>
        /// ProjectID
        /// </summary>
        /// <value>The project ID.</value>
        public int ProjectID
        {
            get {
                return GetDecryptParam("projectID", "0").ToNumber<int>(); }
        }
        /// <summary>
        /// MenuID
        /// </summary>
        /// <value>The menu ID.</value>
        public int MenuID
        {
            get {
                return GetDecryptParam("menuID", "0").ToNumber<int>();
            }
        }

        /// <summary>
        /// MenuType
        /// </summary>
        /// <value>The type of the menu.</value>
        public int MenuType
        {
            get {
                return GetDecryptParam("menuType", "0").ToNumber<int>();
            }
        }

        /// <summary>
        /// 요청 프로젝트 아이디
        /// </summary>
        public int RqProjectID
        {
            get
            {
                return GetDecryptParam("rqProjectID", "0").ToNumber<int>();
            }
        }
                   
        /// <summary>
        /// Gets the proj param.
        /// </summary>
        /// <value>The proj param.</value>
        public string ProjParam
        {
            get {

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("projectID", ProjectID.ToString());
                paraDic.Add("menuID", MenuID.ToString());
                paraDic.Add("menuType", MenuType.ToString());
                paraDic.Add("rqProjectID", RqProjectID.ToString());

                string url = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                return "p=" + url;
            }
        }        


        /// <summary>
        /// 권한
        /// 0:권한없음, 1:게스트, 2:사용자, 3:관리자 
        /// </summary>
        public int Authority
        {
            get { return authority; }
        }
        #endregion
    }
}
