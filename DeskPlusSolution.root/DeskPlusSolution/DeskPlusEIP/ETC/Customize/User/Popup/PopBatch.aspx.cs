using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.ETC.Web.Customize.User.Popup
{
    public partial class PopBatch : Base.BasePageETC
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public int ListID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage;// = new string(); //게시판 리스트 개수   

        public string VendorCD;
        public string MasterDetailGubun;

        public string IsAdmin;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Customize.User.Popup.PopBatch).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }



        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            //ddlListCount.SelectedValue = RecordPerPage;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = "닫기";
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            //Order = (Order.Length == 0) ? "Lists.CompanyID, Lists.CategoryID, Lists.ListID, Lists.CRUDDate" : Order;
            //ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            MasterDetailGubun = GetDecryptParam("MasterDetailGubun", "M");
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            IsAdmin = GetDecryptParam("IsAdmin", "");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";

            VendorCD = GetDecryptParam("VendorCD", "");
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}