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
    public partial class UnitList : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int TotCnt = 0;                  //총 게시물수
        public string PageTitle = string.Empty; //페이지제목
        public string Section = string.Empty;   //메뉴(질문:0 or 예시:1)
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.UnitList).Assembly;	//어셈블리 선언
            #endregion

            //PageSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.OPINION, "ListCount", "10").ToNumber<int>();
            //PageSize = 10;

            if (!IsPostBack)
            {
                LoadParam();    // 파라미터처리
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
        /// 파라미터처리
        /// </summary>
        private void LoadParam()
        {
            Section = (MenuID.ToUpper().Equals("UNIT")) ? "0" : "1";     //구분(질문:0 or 예시:1)
            TotCnt = GetDecryptParam("totCnt", "0").ToNumber<int>();
            Order = (Order.Length.Equals(0)) ? "UnitID DESC" : Order;
            PageSize = GetDecryptParam("pageSize", "10").ToNumber<int>();
        }

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //Title                
                this.PageTitle = (Section.Equals("0")) ? this.GetResource("TitUnitListSub1") : this.GetResource("TitUnitListSub2");
                
                //Button
                this.cbtnAdd.Text = this.GetResource("Add");
                this.cbtnDel.Text = this.GetResource("Del");
                this.cbtnSrh.Text = this.GetResource("BtnSrh");
                this.cbtnActive.Text = this.GetResource("BtnActive");

                this.ddlSrh.Items.Clear();
                this.ddlSrh.Items.Add(new ListItem(this.GetResource("TitTitle"), "Title"));
                //this.ddlSrh.Items.Add(new ListItem("등록일", "InputDate"));
                //this.ddlSrh.Items.Add(new ListItem("작성자", "WriteUserName"));

                this.ddlSchTerm.Items.Clear();
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitTotal"), ""));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitWeek"), "week"));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitMonth"), "month"));
                this.ddlSchTerm.Items.Add(new ListItem(this.GetResource("TitDay"), "period"));
            }
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
            DAL.Unit udal = new DAL.Unit();
            return udal.GetUnitList(_companyID, _section, _sKey, _sVal, _sPeriod, _sPeriodVal);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}