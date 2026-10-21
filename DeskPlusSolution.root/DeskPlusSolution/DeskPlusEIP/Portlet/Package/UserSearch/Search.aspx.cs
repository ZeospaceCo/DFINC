using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Portlet.Web.Package.UserSearch
{
    public partial class Search : Base.BasePagePortlet
    {
        public int CompanyID = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.UserSearch.Search).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        private void LoadData()
        {
            CbtnClose.Text = GetResource("Close");
            DeptSearchTree1.CompanyID = CurrentUserInfo.CompanyID;
            DeptSearchTree1.DeptID = 0;
            DeptSearchTree1.UseActiveDept = true;
            DeptSearchTree1.UseCheckBox = false;
            DeptSearchTree1.UseIcon = true;
            CompanyID = CurrentUserInfo.CompanyID;
        }

        private void SetLanguage()
        {

        }
    }
}