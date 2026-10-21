using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule
{
    public partial class List : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int MenuID = 0;
        public string TmpDate = String.Empty;
        public string MenuName = string.Empty; // 메뉴 이름
        public int Grade = 0;
        public string CheckAuth = "";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.List).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-05-31
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            if (CommunityID > 0 && MenuID > 0)
            {
                GetMenuName();
            }
            else
            {
                MenuName = GetResource("NoTitle");
            }

            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int WriteAuth = 9;
                int ReadAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade <= WriteAuth)
                    {
                        cbtnAdd.Visible = true;
                    }
                    else
                    {
                        CheckAuth = "Y";
                        cbtnAdd.Visible = false;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 

            //페이지 상단에 출력
            timeList.InnerText = Convert.ToDateTime(TmpDate).Year + "-" + Convert.ToDateTime(TmpDate).Month.ToString().PadLeft(2, '0');
            TargetYear = Convert.ToDateTime(TmpDate).Year;
            TargetMonth = Convert.ToDateTime(TmpDate).Month;
            LoadYear(TargetYear);
           
            ddlMonth.Items.Clear();
            for (int i = 1; i < 13; i++)
            {
                ddlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            //ddlYear.SelectedValue = TargetYear.ToString();
            ddlMonth.SelectedValue = TargetMonth.ToString();
        }      


        /// <summary>
        /// 년도 셋팅
        /// </summary>
        /// <param name="_startYear"></param>
        private void LoadYear(int _startYear)
        {
            ddlYear.Items.Clear();
            ListItem li = new ListItem("▲", "0000");
            this.ddlYear.Items.Add(li);

            for (int i = _startYear; i < _startYear + 10; i++)
            {
                li = new ListItem(i.ToString(), i.ToString());
                this.ddlYear.Items.Add(li);
            }

            li = new ListItem("▼", "9999");
            this.ddlYear.Items.Add(li);

            this.ddlYear.SelectedValue = _startYear.ToString();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");           //추가;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
        }

        /// <summary>
        /// 현재 년월일 가져오기 [2012-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMon = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMon = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMon = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            return nowYearMon;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 메뉴 이름 가져오기
        /// </summary>
        private void GetMenuName()
        {
            DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenuName(CurrentUserInfo.CompanyID, CommunityID, MenuID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MenuName = GetLangName(dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(),
                        dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(),
                        dr["NameZhcn"].ToString(),
                        dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                }
            }
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// 
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion

    }
}