using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ListMain : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리
        public int CategoryID;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string TagGubun = String.Empty;
        public string TmpDate = String.Empty;
        public string GroupGubun = String.Empty;
        public string CntYN = "N";
        public int RmCnt = 0;
        public string SiteMap = String.Empty;//사이트맵
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ListMain).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-02-20
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //사이트맵
            GetSiteMap(); 

            Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
            DataTableReader dr = env.GetRMTabMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID); //사용자 일간,주간,월간 가져오기

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["RMTabMenu"].ToString() == "")
                        {
                            TagGubun = "day";
                        }
                        else
                        {
                            TagGubun = dr["RMTabMenu"].ToString();
                        }
                    }
                }
            }

            DataTableReader dr2 = env.GetRMTypeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID); //사용자 list, blog보기 가져오기

            if (dr2 != null)
            {
                if (dr2.HasRows)
                {
                    while (dr2.Read())
                    {
                        if (dr2["RMTypeMenu"].ToString() == "")
                        {
                            GroupGubun = "1";
                        }
                        else
                        {
                            GroupGubun = dr2["RMTypeMenu"].ToString();
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

            if (CntYN == "Y")
            {
                GetRMCnt();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-03
        /// 작성목적 : 자원 주 담당자의 대기 예약 카운트 가져오기
        /// </summary>
        public void GetRMCnt()
        {
            DAL.List dal = new DAL.List();
            int tmpCnt = dal.GetListUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (tmpCnt > 0)
            {
                DAL.Reverse redal = new DAL.Reverse();
                RmCnt = redal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
            }
        }

        #endregion 

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-20
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
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ResourcesTitle"), "Title"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subscriber"), "ManyName"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", "");
            TagGubun = GetDecryptParam("tagGubun", "week");
            GroupGubun = GetDecryptParam("groupGubun", "1");
            CntYN = GetDecryptParam("cntYN", "N");
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (CategoryID > 0)
            {                
                DAL.Category Dal = new DAL.Category();
                DataSet ds = Dal.GetFolder(CurrentUserInfo.CompanyID, CategoryID);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SiteMap = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                }              
            }

            if (SiteMap == "") SiteMap = resMng.GetString("RM");
        }
     

        #endregion 

        #region (프로퍼티)      
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}