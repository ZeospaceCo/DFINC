using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.PMS.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class Menu : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {


            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.Main).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                if (!this.resMng.Equals(null))
                {
                    
                }

                LoadData();
                LoadMenuData();
            }
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion            

            liMenuRelated.Visible = (Authority == 1) ? false : true;
            AdminMenu.Visible = (IsAdmin == 1 && Authority != 1) ? true : false;
            AdminMenusub.Visible = (IsAdmin == 1 && Authority != 1) ? true : false;
            
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// Gets the menu URL.
        /// </summary>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="menuType">Type of the menu.</param>
        /// <returns></returns>
        public string GetMenuUrl(object _menuID, object menuType)
        {
            Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
            string url = lib.GetMenuUrl(menuType.ToString().ToNumber<int>());
            //url += "?projectID=" + ProjectID + "&menuID=" + _menuID + "&menuType=" + menuType + "&rqProjectID=" + RqProjectID.ToString();

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", _menuID.ToString());
            paraDic.Add("menuType", menuType.ToString());
            paraDic.Add("rqProjectID", RqProjectID.ToString());
            
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            url = url + "?p=" + encParams;


            return url;
        }



        /// <summary>
        /// Loads the menu data.
        /// </summary>
        private void LoadMenuData()
        {
            //프로젝트명 출력하기
            DAL.Project pDal = new Zeospace.DeskPlus.PMS.DAL.Project();
            var alList = pDal.GetProject(CompanyID, ProjectID);
            foreach (var be in alList)
            {
                if (be != null)
                {
                    string title = this.GetLangName(
                                    be.SubjectBase,
                                    be.SubjectKo, be.SubjectEn, be.SubjectJa,
                                    be.SubjectFr, be.SubjectZhcn, be.SubjectZhtw, PreLang).ToNull().ToReverseXSS();

                    if (title.Length >= 15) title = title.Substring(0, 14) + "..";

                    ltlTitle.Text = title;
                }
            }

            //메뉴 출력하기
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            DataTableReader dtr = bll.GetMenuList(CompanyID, ProjectID);

            rpMenuList.DataSource = dtr;
            rpMenuList.DataBind();
        }


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 폐쇄 신청
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-29
        /// </summary>
        /// <returns></returns>
        [ComponentArt.Web.UI.ComponentArtCallbackMethod]
        public string AjaxRequestClosed(int _projectID)
        {
            try
            {
                string loginID = HttpContext.Current.User.Identity.Name;
                Helpers.Auth.UserInfo userInfo = Helpers.Auth.Authenticator.GetInstance().AuthenticatorInfo(loginID);

                //string loginID = HttpContext.Current.User.Identity.Name;
                //UserInfoHelper userInfo = new UserInfoHelper(loginID);

                DAL.Project bll = new Zeospace.DeskPlus.PMS.DAL.Project();
                bll.Mod(userInfo.CompanyID, _projectID, "1", userInfo.UserID);

                return "1";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("error : " + ex.Message);
                return "0";
            }
            
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 총평을 쓸 수 있는 권한이 있는지 확인
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-03
        /// </summary>
        /// <returns></returns>
        [ComponentArt.Web.UI.ComponentArtCallbackMethod]
        public string AjaxIsAuth(int _projectID)
        {
            string response = "0";

            string loginID = HttpContext.Current.User.Identity.Name;
            Helpers.Auth.UserInfo userInfo = Helpers.Auth.Authenticator.GetInstance().AuthenticatorInfo(loginID);

            DAL.Project proj = new Zeospace.DeskPlus.PMS.DAL.Project();
            var al = proj.GetProject(userInfo.CompanyID, _projectID);

            foreach (var info in al)
            {
                if (info.WriteUserID == userInfo.UserID)
                {
                    response = "1";
                }
            }

            return response;
        }
        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// 프로젝트 관리자 여부
        /// </summary>
        /// <value>The is admin.</value>
        public int IsAdmin
        {
            get { return GetViewStateInt("IsAdmin", "1"); }
            set { ViewState["IsAdmin"] = value; }
        }
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
        }
        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion

    }
}
