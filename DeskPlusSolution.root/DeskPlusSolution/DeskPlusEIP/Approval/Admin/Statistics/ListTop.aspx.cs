using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Statistics
{
    public partial class ListTop : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int TotalItemCount;      //총카운트
        public int SelYears;
        public string TabMenu = "";    // 양식별(Form), 부서별(Dept), 개인별(User)
        public string sDate = "";
        public string eDate = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListTop).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ddlSearchField.SelectedValue = SearchField;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            GetTotInfo();
            GetListCount();
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //Button
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); //"검색";

                //DropDown Item
                this.ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ListType0"), ""));
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("StatTop10"), "term"));
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();

            SelYears = GetDecryptParam("SelYears", DateTime.Today.Year.ToString()).ToNumber<int>();
            TabMenu = GetDecryptParam("TabMenu", "Form");
            sDate = GetDecryptParam("sDate", "");
            eDate = GetDecryptParam("eDate", "");
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string tmpsDate = "", tmpeDate = "";
            if (sDate.Equals("") || eDate.Equals(""))
            {
                tmpsDate = SelYears.ToString() + "-01-01";         // 2012-01-01
                tmpeDate = (SelYears + 1).ToString() + "-01-01";   // 2013-01-01
            }
            else
            {
                tmpsDate = sDate;
                tmpeDate = eDate;
            }

            DAL.Statistics dal = new DAL.Statistics();
            switch (TabMenu.ToUpper())
            {
                case "Form":
                    TotalItemCount = dal.GetTopFormListCnt(CurrentUserInfo.CompanyID, tmpsDate, tmpeDate);
                    break;
                case "Dept":
                    TotalItemCount = dal.GetTopDeptListCnt(CurrentUserInfo.CompanyID, tmpsDate, tmpeDate);
                    break;
                case "User":
                    TotalItemCount = dal.GetTopUserListCnt(CurrentUserInfo.CompanyID, tmpsDate, tmpeDate);
                    break;
                default:
                    TotalItemCount = dal.GetTopFormListCnt(CurrentUserInfo.CompanyID, tmpsDate, tmpeDate);
                    break;
            }
        }
        /// <summary>
        /// 전체정보 가져오기
        /// </summary>
        private void GetTotInfo()
        {
            DAL.Statistics dal = new DAL.Statistics();
            DataTableReader dtr = dal.GetTopTotList(CurrentUserInfo.CompanyID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbTot.Text = dtr["TotCnt"].ToString();
                        lbC.Text = dtr["C"].ToString();
                        lbW.Text = (dtr["W"].ToString().ToNumber<int>() + dtr["I"].ToString().ToNumber<int>() + dtr["P"].ToString().ToNumber<int>()).ToString();
                        lbR.Text = dtr["R"].ToString();
                        lbD.Text = (dtr["D"].ToString().ToNumber<int>() + dtr["H"].ToString().ToNumber<int>()).ToString();
                    }
                }
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}