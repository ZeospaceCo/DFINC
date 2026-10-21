using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class ListUpDown : Base.BasePageRMAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정

        public string FolderList = "";
        public int CategoryID = 0;
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort

        #endregion
        
        
        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.ListUpDown).Assembly;	//어셈블리 선언
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

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("ChangeOrders"); //순서변경
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {   
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            StringBuilder sb = new StringBuilder();                        
            DAL.List dal = new DAL.List();
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CategoryID);

            //string name = "";
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<li id='");
                        sb.Append(dtr["ListID"] + "|" + dtr["CategoryID"]);
                        sb.Append("'>");                        
                        sb.Append(dtr["Title"]);
                        sb.Append("</li>");
                    }                    
                }                               
            }
            FolderList = sb.ToString();            
        }

        #endregion

        #region (속성)

        #endregion
    }
}