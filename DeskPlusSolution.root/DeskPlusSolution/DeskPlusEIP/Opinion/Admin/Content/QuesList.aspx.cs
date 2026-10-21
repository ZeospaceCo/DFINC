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

namespace Zeospace.DeskPlus.Opinion.Web.Admin.Content
{
    public partial class QuesList : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion


        #region 변수설정
        public int TotCnt = 0;                  //총 게시물수
        public string PageTitle = string.Empty; //페이지제목
        public string Section = string.Empty;   //메뉴(진행:0 or 마감:1)
        #endregion


        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();     // 파라미터처리
                SetLanguage();  // 다국어처리

                this.ddlSchTerm.SelectedValue = SPeriod;
                this.ddlSrh.SelectedValue = SKey;
                this.ctbSrh.Text = SVal;
                this.ddlListCount.SelectedValue = PageSize.ToString();

                //총 리스트 수
                TotCnt = this.GetListCnt(CurrentUserInfo.CompanyID, Section, SKey, SVal, SPeriod, SPeriodVal);
            }
        }
        #endregion


        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion


        #region (함수)

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //Title
                this.PageTitle = (Section.Equals("0")) ? this.GetResource("TitIng") : this.GetResource("TitClose");

                //Button
                this.cbtnAdd.Text = this.GetResource("Add");
                this.cbtnDel.Text = this.GetResource("BtnDel");
                this.cbtnSrh.Text = this.GetResource("BtnSrh");
                this.cbtnActive.Text = this.GetResource("BtnActive");

                //DropDown Item
                this.ddlSrh.Items.Clear();
                this.ddlSrh.Items.Add(new ListItem(this.GetResource("TitTitle"), "Title"));
                this.ddlSrh.Items.Add(new ListItem(this.GetResource("TitSDate"), "StartDate"));
                this.ddlSrh.Items.Add(new ListItem(this.GetResource("TitEDate"), "EndDate"));

                this.ddlSchTerm.Items.Clear();
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitTotal"), ""));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitWeek"), "week"));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitMonth"), "month"));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitDay"), "period"));
            }
        }

        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            Section = (MenuID.ToUpper().Equals("QING")) ? "0" : "1";     //구분(진행:0 or 마감:1)
            Order = (Order.Length.Equals(0)) ? "q.QuesID DESC" : Order;

            //진행중인설문만 처리가능.
            cbtnAdd.Visible = (MenuID.ToUpper().Equals("QING")) ? true : false;
            cbtnActive.Visible = (MenuID.ToUpper().Equals("QING")) ? true : false;
            PageSize = GetDecryptParam("pageSize", "10").ToNumber<int>();
        }


        /// <summary>
        /// 리스트 총개수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        private int GetListCnt(int _companyID, string _section, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            DAL.Ques udal = new DAL.Ques();
            return udal.GetQuesList(_companyID, _section, _sKey, _sVal, _sPeriod, _sPeriodVal);
        }

        #endregion


        #region (프로퍼티)
        #endregion


        #region (이벤트핸들러)
        #endregion
    }
}