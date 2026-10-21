using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class WholeMonth : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리
        public int CategoryID;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string SearchUsable = string.Empty;
        public string TmpDate = String.Empty;
        protected DateTime dtTargetDate;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.WholeMonth).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-03-02
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //페이지 상단에 출력
            timeList.InnerText = Convert.ToDateTime(TmpDate).Year + "-" + Convert.ToDateTime(TmpDate).Month.ToString().PadLeft(2, '0');

            // 사용자 설정 상태 가져오기
            SearchUsable = "";
            DAL.Usable dal = new DAL.Usable();
            List<BE.UsableInfo> al = dal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            foreach (BE.UsableInfo _Info in al)
            {
                SearchUsable = _Info.Usable;
            }

            foreach (string list in SearchUsable.Split(','))
            {
                if (list == "0")
                {
                    cbxUsable.Items[0].Selected = true;
                }

                if (list == "1")
                {
                    cbxUsable.Items[1].Selected = true;
                }

                if (list == "2")
                {
                    cbxUsable.Items[2].Selected = true;
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-03-02
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbxUsable.Items.Clear();
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b1_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable0") + "</p>", "0"));
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b2_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable1") + "</p>", "1"));
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b3_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable2") + "</p>", "2"));
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-03-02
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
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