using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.PopUp
{
    public partial class List : Base.BasePageETC
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string Order = string.Empty; //sort
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.PopUp.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            ddlListCount.SelectedValue = RecordPerPage;
            GetListCount();
        }

        #endregion

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {                
                cbtnAdd.Text = this.resMng.GetString("List_Register");                
            }
        }

        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            Order = (Order.Length == 0) ? " ManagerID desc" : Order;

            RecordPerPage = GetDecryptParam("recordPerPage", "20");
        }

        /// <summary>
        /// 작성자   : shine
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Manager biz = new DAL.Manager();
            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID).ToString();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region 이벤트 헨들러
        #endregion
    }
}