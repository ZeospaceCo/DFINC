using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Zeospace.DeskPlus.Statistics.Web.Content.Login
{
    public partial class List : Base.BasePageStatistics
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public string TabMenu = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Statistics.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Statistics.Web.Content.Login.List).Assembly;	//어셈블리 선언
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

            RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();

            // 최상단 리스트를 가져온다.
            GetTotalCount();
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 언어 설정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //cbtnExcel.Text = this.resMng.GetString("ExcelDown"); // "엑셀다운로드";
                cbtnSearch.Text = this.resMng.GetString("Search");   // "검색";
                cbtnExcel.Text = this.resMng.GetString("StatExcel");

                
                // 현재년도 기준 앞뒤 1년치 바인딩 해준다. 
                string LastYear = DateTime.Now.AddYears(-1).Year.ToString();
                string CurrentYear = DateTime.Now.Year.ToString();
                string AddYear = DateTime.Now.AddYears(1).Year.ToString();

                ddlSearchYear.Items.Clear();
                ddlSearchYear.Items.Add(new ListItem(LastYear, LastYear));
                ddlSearchYear.Items.Add(new ListItem(CurrentYear, CurrentYear));
                ddlSearchYear.Items.Add(new ListItem(AddYear, AddYear));

                ddlSearchYear.SelectedValue = CurrentYear;

                ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "NameBase"));    //UserName

            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "userID  " : Order;
            TabMenu = GetDecryptParam("TabMenu", "LOGINS");
        }


        /// <summary>
        /// 상단 전체 토탈 카운트를 가져온다.
        /// </summary>
        private void GetTotalCount()
        {
            BE.LoginTotalInfo LoginTotalInfo = new BE.LoginTotalInfo();
            DAL.Login Login = new DAL.Login();

            LoginTotalInfo.CompanyID = CurrentUserInfo.CompanyID; 
            LoginTotalInfo.TargetYear = ddlSearchYear.SelectedValue.ToString();

            DataTableReader dtr = Login.GetTotalTopList(LoginTotalInfo);

            // 가벼운 사유라 할지라도... 1Row 가져오는데. 이 방식은.. seo 
            // ds 나 dt 를 해도 상관 없지 않을까..
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lblLoginTotal.Text = dtr["LoginTotalCount"].ToString();
                        lblLoginFailTotal.Text = dtr["LoginFailTotalCount"].ToString();
                        lblMLoginTotal.Text = dtr["MLoginTotalCount"].ToString();
                        lblMLoginFailTotal.Text = dtr["MLoginFailTotalCount"].ToString();  
                    }
                }
            }
            
        }

        /// </summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 :해당 카테고리 총 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// <summary>
        private void GetListCount()
        {
            BE.LoginInfo LoginInfo = new BE.LoginInfo();
            DAL.Login Login = new DAL.Login();

            LoginInfo.CompanyID = CurrentUserInfo.CompanyID; // 계열사를 선택해서 가져오는게 맞지 않나?;; seo
            LoginInfo.TargetYear = ddlSearchYear.SelectedValue.ToString();
            string srhField = ddlSearchField.SelectedValue;
            string srhValue = ctbSearch.Text;


            switch (TabMenu.ToUpper())
            {
                case "LOGINS":
                    TotalCount = Login.GetPageLSCount(LoginInfo).ToString();
                    break;
                case "LOGINF":
                    TotalCount = Login.GetPageLSFCount(LoginInfo).ToString();
                    break;
                case "MLOGINS":
                    TotalCount = Login.GetPageLSMCount(LoginInfo).ToString();
                    break;
                case "MLOGINF":
                    TotalCount = Login.GetPageLSMFCount(LoginInfo).ToString();
                    break;
                default:
                    TotalCount = Login.GetPageLSCount(LoginInfo).ToString();
                    break;
            }
            
        }
        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}