using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.Web.Admin.Share
{
    public partial class List : Base.BasePagePimAdmin
    {
        #region 변수설정
        public int CatID = 0;//최상위 트리 나올 카테고리
        public int CategoryID = 0;//포커스 설정할 트리 카테고리
        public int pCategoryID = 0;//상위부서 카테고리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public int CompanyID = 0;
        public int UserID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Share.List).Assembly;	//어셈블리 선언
            #endregion

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;



            if (!IsPostBack)
            {
                SetLanguage();

                #region 이미지 처리
                
                #endregion

                #region 초기화
                //DataLoad();

                UCUserDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
                UCUserDeptTreeScript1.DeptID = CurrentUserInfo.DeptID;
                UCUserDeptTreeScript1.UseActiveDept = true;
                UCUserDeptTreeScript1.UseCheckBoxMember = true;
                UCUserDeptTreeScript1.UseIcon = true;
                UCUserDeptTreeScript1.UseCheckBoxGroup = true;
                UCUserDeptTreeScript1.UseMyRootDept = false;
                UCUserDeptTreeScript1.UseVirtualTree = false;
                UCUserDeptTreeScript1.UseRootCheckBox = false;
                
                #endregion
            }
        }

        #endregion 

        #region Method (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = GetResource("SaveContinue");
                cbtnConfirm.Text = GetResource("Confirm");
                
            }
        }

      

        #endregion

        #region Event Handler (이벤트 핸들러)



        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            //AddOpenUser();
        }



        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}