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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ComponentArt.Web.UI;

namespace Zeospace.DeskPlus.PMS.Web.Admin.Menu
{
    public partial class Add : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {

        #region Declaration (컨트롤 선언)
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Menu.Add).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

            #region 버튼 이미지
            //cbsAdd.Icon = BaseImgPath + "/PMS/Button/but_add.gif";
            //cbsMod.Icon = BaseImgPath + "/PMS/Button/but_modify.gif";
            //cbsDel.Icon = BaseImgPath + "/PMS/Button/but_delete.gif";
            #endregion
        }
        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   :  데이터가져오기
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //UserInfoHelper userInfo = new UserInfoHelper(HttpContext.Current.User.Identity.Name);
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion

            LoadMenuData();
        }


        /// <summary>
        /// 메뉴 데이타 가져오기
        /// </summary>
        private void LoadMenuData()
        {
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            DataTableReader dtr = bll.GetAdminMenuList(CompanyID, ProjectID);

            rpMenuList.DataSource = dtr;
            rpMenuList.DataBind();
        }


        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                //cbtnMove.Text = resMng.GetString("Orders");
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnAdd.Text = resMng.GetString("Add");
            }
        }

        /// <summary>
        /// 메뉴 정보 가져오기
        /// JSON 형식으로 리턴한다.
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <param name="_projectID">프로젝트아이디</param>
        /// <param name="_menuID">메뉴아이디</param>
        /// <param name="_prelang">The _prelang.</param>
        /// <returns>
        /// 메뉴정보 JSON
        /// </returns>
        [ComponentArtCallbackMethod]
        public string AajxGetMenuData(int _companyID, int _projectID, int _menuID, string _prelang)
        {
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            var al = bll.GetMenuList(_companyID, _projectID, _menuID);
            foreach (var menuInfo in al)
            {
                
                menuInfo.PreLang = _prelang;

                string menuName = UtilHelper.GetLangName(menuInfo.MenuBase, menuInfo.MenuKo, menuInfo.MenuEn,
                    menuInfo.MenuJa, menuInfo.MenuFr, menuInfo.MenuZhcn, menuInfo.MenuZhtw, _prelang);

                string MenuMultiLang = UtilHelper.GetMuiltiLang(menuInfo.MenuKo, menuInfo.MenuEn,
                    menuInfo.MenuJa, menuInfo.MenuFr, menuInfo.MenuZhcn, menuInfo.MenuZhtw);

                string response = "{"
              + "\"ProjectID\":\"" + menuInfo.ProjectID.ToString() + "\""
              + ", \"MenuID\":\"" + menuInfo.MenuID.ToString() + "\""
              + ", \"MenuBase\":\"" + menuName + "\""
              + ", \"MenuMultiLang\":\"" + MenuMultiLang + "\""
              + ", \"MenuBaseName\":\"" + System.Uri.EscapeDataString(menuInfo.MenuBase.ToReverseXSS()) + "\""
              + ", \"MenuType\":\"" + menuInfo.MenuType.ToString() + "\""
              + ", \"IsUse\":\"" + menuInfo.IsUse.ToString() + "\""
              + ", \"Orders\":\"" + menuInfo.Orders.ToString() + "\""
              + ", \"WriteDate\":\"" + menuInfo.WriteDate.ToString() + "\""
              + ", \"MenuDesc\":\"" + System.Uri.EscapeDataString(menuInfo.MenuDesc.ToNull().ToReverseXSS()) + "\""
              + "};";                
                
                return response;
            }

            return "";
        }

        /// <summary>
        /// 메뉴 순서 변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_projectID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_dir">up, down</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public bool SetMenuOrder(int _companyID, int _projectID, int _menuID, string _dir)
        {
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            return bll.SetMenuOrder(_companyID, _projectID, _menuID, _dir);
        }

        /// <summary>
        /// Alert 메시지를 보여주고 페이지를 리프래쉬한다.
        /// </summary>
        /// <param name="_msg"></param>
        private void RefreshPage(string _msg)
        {
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.ClientMessage.Alert(_msg);

            //Response.Write("<script language='javascript'>");
            //Response.Write("document.location.href=\"MenuManager.aspx?" + ProjParam + "\"");
            //Response.Write("</script>");


            string url = @"location='{0}';";            
            url = string.Format(url, "Add.aspx?" + ProjParam);
            
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", _msg, url);
        }

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "0"); }
            set { ViewState["CompanyID"] = value; }
        }
        #endregion


        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 삭제 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDel_Click(object sender, System.EventArgs e)
        {
            int menuID = hdMenuID.Value.ToNumber<int>();
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            bool result = bll.Del(CompanyID, ProjectID, menuID);

            string msg = (result) ? GetResource("MSG_SUCCESS") : GetResource("MSG_FAIL");
            RefreshPage(msg);
        }

        /// <summary>
        /// 수정 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnMod_Click(object sender, System.EventArgs e)
        {
            int menuID = hdMenuID.Value.ToNumber<int>();
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            BE.MenuInfo menuInfo = new Zeospace.DeskPlus.PMS.BE.MenuInfo();

            menuInfo.CompanyID = CompanyID;
            menuInfo.ProjectID = ProjectID;
            menuInfo.MenuID = menuID;


            //메뉴 다국어 개발 필요
            menuInfo.MenuBase = tbMenuName.Text;

            MultiName = this.CTBMultiName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                menuInfo.MenuKo = MultiName.Split(',')[0];
                menuInfo.MenuEn = MultiName.Split(',')[1];
                menuInfo.MenuJa = MultiName.Split(',')[2];
                menuInfo.MenuFr = MultiName.Split(',')[3];
                menuInfo.MenuZhcn = MultiName.Split(',')[4];
                menuInfo.MenuZhtw = MultiName.Split(',')[5];
            }

            menuInfo.MenuDesc = tbMenuDesc.Text;
            menuInfo.IsUse = (cbIsUse.Checked) ? 1 : 0;
            
            bool result = bll.Mod(menuInfo);

            string msg = (result) ? GetResource("MSG_SUCCESS") : GetResource("MSG_FAIL");
            RefreshPage(msg);
        }


        /// <summary>
        /// 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, System.EventArgs e)
        {
            DAL.Menu bll = new Zeospace.DeskPlus.PMS.DAL.Menu();
            BE.MenuInfo menuInfo = new Zeospace.DeskPlus.PMS.BE.MenuInfo();

            menuInfo.CompanyID = CompanyID;
            menuInfo.ProjectID = ProjectID;
            menuInfo.MenuType = 4;

            //메뉴 다국어 개발 필요
            menuInfo.MenuBase = tbMenuName.Text;

            MultiName = this.CTBMultiName.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                menuInfo.MenuKo = MultiName.Split(',')[0];
                menuInfo.MenuEn = MultiName.Split(',')[1];
                menuInfo.MenuJa = MultiName.Split(',')[2];
                menuInfo.MenuFr = MultiName.Split(',')[3];
                menuInfo.MenuZhcn = MultiName.Split(',')[4];
                menuInfo.MenuZhtw = MultiName.Split(',')[5];
            }

            menuInfo.MenuDesc = tbMenuDesc.Text;
            menuInfo.IsUse = (cbIsUse.Checked) ? 1 : 0;

            int result = bll.Add(menuInfo);

            string msg = (result > 0) ? GetResource("MSG_SUCCESS") : GetResource("MSG_FAIL");
            RefreshPage(msg);
        }


        #endregion
    }

    

}
