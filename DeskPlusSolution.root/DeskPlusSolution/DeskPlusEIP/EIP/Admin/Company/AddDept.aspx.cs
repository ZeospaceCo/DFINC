using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.BLL;
using Zeospace.DeskPlus.Account.DAL;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Company
{
    public partial class AddDept : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        private string eventArgs = String.Empty;
        public int CompanyID = 0;
        public int DeptID = 0;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Company.AddDept).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Confirm");
                CbtnClose.Text = resMng.GetString("Closed");
            }
        }
        

        #endregion

        #region Event Handler (이벤트 핸들러)

        

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}