using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Menu
{
    public partial class Tree : Base.BasePageApproval
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive
        public string CabinetID;
        public string IsEnforceAdmin = "0";
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Menu.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                LoadAutority();

                UCAppFolderTreeScript1.CabinetID = CabinetID;
                UCAppFolderTreeScript2.CabinetID = CabinetID;
                UCAppFolderTreeScript3.CabinetID = CabinetID;
                UCAppFolderTreeScript4.CabinetID = CabinetID;
                UCAppFolderTreeScript5.CabinetID = CabinetID;
            }
        }
        #endregion

        #region (함수)
        
        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
            CabinetID = GetDecryptParam("cabinetID", "");
        }

        private void LoadAutority()
        {
            // 공문관리자 확인
            DAL.EnforceAdmin dal = new DAL.EnforceAdmin();
            IsEnforceAdmin = (dal.GetEnforceAdminInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).HasRows) ? "1" : "0";
        }


        #endregion

        #region Property (프로퍼티)
        
        #endregion
    }
}