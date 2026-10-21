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

namespace Zeospace.DeskPlus.ETC.Web.Holiday
{
    public partial class GetUserInfo : Base.BasePageETC
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";		     	//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.GetUserInfo).Assembly;	    //어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
            DeptUserTreeScript1.UseCheckBoxGroup = false;
            DeptUserTreeScript1.UseCheckBoxMember = true;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = resMng.GetString("Confirm");
                cbtnCancel.Text = resMng.GetString("Close");
                cbtnClean.Text = resMng.GetString("DelPerson");

                ltlReceiveMan.Text = resMng.GetString("SelectPerson");
                ltlReceiveManSelect.Text = resMng.GetString("PleaseSelectPerson");
                lblSelectedReceiptor.Text = resMng.GetString("Target");
            }
        }
    }
}