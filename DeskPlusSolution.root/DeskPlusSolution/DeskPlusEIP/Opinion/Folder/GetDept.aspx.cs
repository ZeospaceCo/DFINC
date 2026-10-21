using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.Web.Folder
{
    public partial class GetDept : Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int DeptID;
        public string DeptNm;
        #endregion
        
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Folder.GetDept).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptNm = GetDecryptParam("deptNm", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = this.GetResource("BtnConfirm");
                CbtnClose.Text = this.GetResource("BtnClose");
            }
        }

        private void LoadData()
        {
            LoadParam();

            UCDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptTreeScript1.DeptID = DeptID;
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion

    }
}