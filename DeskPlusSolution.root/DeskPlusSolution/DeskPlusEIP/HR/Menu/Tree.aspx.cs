using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.HR.Web.Menu
{
    public partial class Tree : Base.BasePageHR
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive
        public string DefaultMenuID = "";
        public string FolderStatusID = "";
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Menu.Tree).Assembly;	            //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                UCFolderStatusTreeScript1.FolderStatusID = FolderStatusID;

                bool isRoleHRAttendance = CurrentUserInfo.IsInRole("HRAttendance"); // 근태수정관리자
                if (AccessAuth || isRoleHRAttendance)
                {
                    MenuAdmin.Visible = true;
                    MenuSubAdmin.Visible = true;

                    ltlMenuAdmin.Text = GetResource("HRMod");

                    // 겸직부서관리 메뉴 슈퍼관리자 or 근태관리자만 보이게
                    if (AccessAuth)
                    {
                        MenuSubBaseDept.Visible = true;

                        DAL.UserDept dal = new DAL.UserDept();
                        int noCount = dal.GetUserDeptNoCount(CurrentUserInfo.CompanyID);

                        if (noCount > 0)
                        {
                            ltlMenuAdmin.Text = GetResource("HRMod") + " <span style='color: red'>(" + noCount + ")</span>";
                            ltlSubUserDept.Text = GetResource("BaseDeptManage") + " <span style='color: red'>(" + noCount + ")</span>";
                        }
                        else
                        {
                            ltlSubUserDept.Text = GetResource("BaseDeptManage");
                        }
                    }
                }
                else
                {
                    MenuAdmin.Visible = false;
                    MenuSubAdmin.Visible = false;
                    MenuSubBaseDept.Visible = false;
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            DefaultMenuID = GetDecryptParam("defaultMenuID", "");
        }
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}