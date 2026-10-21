using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class ListMain : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리
        public int CategoryID;
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string TagGubun = String.Empty;
        public string TmpDate = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.ListMain).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-05
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
            
            DataTableReader dr = env.GetVisitTabMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["VisitTabMenu"].ToString() == "")
                        {
                            TagGubun = "day";
                        }
                        else
                        {
                            TagGubun = dr["VisitTabMenu"].ToString();
                        }
                    }
                }
            }

            // 관리자 권한 
            if (AccessAuth)
            {
                cbtnAdmin.Visible = true;
            }
            else
            {
                cbtnAdmin.Visible = false;
            }

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
        }

        #endregion 

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-05
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = resMng.GetString("Admin");         //"관리자";
                cbtnPrint.Text = resMng.GetString("Print");         //"인쇄";
                cbtnAdd.Text = resMng.GetString("Reservation");     //"예약";
                cbtnSearch.Text = this.resMng.GetString("Search");

                ddlSearchField.Items.Clear();                
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subscriber"), "ManyName"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-05
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", "");
            TagGubun = GetDecryptParam("tagGubun", "day");            
        }
     

        #endregion 


        #region (프로퍼티)

      

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}