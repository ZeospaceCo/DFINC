using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Opinion.Web.Folder
{
    public partial class GetUserDept : Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)


        #endregion

        #region 변수설정
        public int DeptID;
        public string DeptNm;
        public string Func;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Folder.GetUserDept).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptNm = GetDecryptParam("deptNm", "");
            Func = GetDecryptParam("func", "");
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

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = DeptID;

            this.DeptUserTreeScript1.UseCheckBoxGroup = (GetDecryptParam("NoGroup", "").Equals("")) ? true : false;
            this.DeptUserTreeScript1.UseCheckBoxMember = (GetDecryptParam("NoMember", "").Equals("")) ? true : false;
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}