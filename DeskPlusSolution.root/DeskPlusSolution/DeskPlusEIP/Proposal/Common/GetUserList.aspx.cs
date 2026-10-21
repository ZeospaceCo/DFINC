using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.Web.Common
{
    public partial class GetUserList : Base.BasePageProposal
    {
        public string Gubun = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Common.GetUserList).Assembly;	//어셈블리 선언
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
            Gubun = GetDecryptParam("gubun", "1");
            //bool group = false;
            //bool member = false;

            //if (Gubun == "1")
            //{
            //    group = true;
            //}
            //else if (Gubun == "2")
            //{
            //    member = true;
            //}
            //else
            //{
            //    group = true;
            //    member = true;
            //}

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;


            DeptUserTreeScript1.UseCheckBoxGroup = false;//group;
            DeptUserTreeScript1.UseCheckBoxMember = true;//member;
        }

        private void SetLanguage()
        {
            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }
    }
}