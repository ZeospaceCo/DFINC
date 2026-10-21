using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Opinion.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Opinion.Web.Content
{
    public partial class FavList : Opinion.Web.Base.BasePageOpinion
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int TotCnt = 0;    //총 게시물수
        public string ClassListTypeC1 = string.Empty;  //listType 버튼이미지 처리(일반)
        public string ClassListTypeC2 = string.Empty;  //listType 버튼이미지 처리(블로그)
        public string ClassListTypeC3 = string.Empty;  //listType 버튼이미지 처리(앨범)
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Content.FavList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();     // 파라미터처리
                SetLanguage();  // 다국어처리
                GetUserSetting(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, MenuID.ToUpper()); //사용자 메뉴별 환경설정
     

                this.ddlSchTerm.SelectedValue = SPeriod;
                this.ddlSrh.SelectedValue = SKey;
                this.ctbSrh.Text = SVal;
                this.ddlListCount.SelectedValue = PageSize.ToString();

                //총 리스트 수
                TotCnt = this.GetListCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SKey, SVal, SPeriod, SPeriodVal);
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            Order = (Order.Length.Equals(0)) ? "q.QuesID DESC" : Order;
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
                this.cbtnSrh.Text = this.GetResource("BtnSrh");
    
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
        /// 중요표시리스트(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <returns></returns>
        private int GetListCnt(int _companyID, int _userID, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            DAL.Fav fDAL = new DAL.Fav();
            return fDAL.GetFavList(_companyID, _userID, _sKey, _sVal, _sPeriod, _sPeriodVal);
        }
        /// <summary>
        /// 사용자 메뉴별 환경설정정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_menuInfo"></param>
        private void GetUserSetting(int _companyID, int _userID, string _menuInfo)
        {
            BLL.Setting sBLL = new BLL.Setting();
            List<SettingInfo> sList = sBLL.GetSetting(_companyID, _userID, _menuInfo);
            if (sList != null)
            {
                foreach (BE.SettingInfo sInfo in sList)
                {
                    PageSize = sInfo.PageSize;
                    ListType = sInfo.ListType;
                }
            }

            ClassListTypeC1 = (ListType.Equals("1")) ? "_on" : "";
            ClassListTypeC2 = (ListType.Equals("2")) ? "_on" : "";
            ClassListTypeC3 = (ListType.Equals("3")) ? "_on" : "";
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}