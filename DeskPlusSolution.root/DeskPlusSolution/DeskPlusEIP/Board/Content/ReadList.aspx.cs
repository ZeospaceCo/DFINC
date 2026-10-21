using System;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class ReadList : Base.BasePageBoard
    {
        #region 변수설정

        public int ListID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //게시판 리스트 개수

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.ReadList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            ddlListCount.SelectedValue = RecordPerPage;

            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            string optionUseDept = "";
            string optionShowViewer = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    optionUseDept = dr["OptionUseDept"].ToString();
                    optionShowViewer = dr["OptionShowViewer"].ToString();
                }
            }

            //조회 활성화 여부
            if (optionShowViewer == "1")
            {
                BLL.Category bizCat = new BLL.Category();
                NameValueCollection nvcAuth = new NameValueCollection();

                //읽기 이상 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                    CurrentUserInfo.DeptID, CategoryID, optionUseDept, false, BE.BoardAuth.Read, true, false);

                if (nvcAuth.AllKeys.Length == 0)
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }

                GetListCount();
            }
            else
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

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
                CbtnClose.Text = resMng.GetString("Closed");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "Lists.CompanyID, Lists.CategoryID, Lists.ListID, Lists.CRUDDate" : Order;
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.ListViewers list = new DAL.ListViewers();
            TotalCount = list.GetPageListCount(BoaComID, CategoryID, ListID);
        }

        #endregion (함수)
    }
}