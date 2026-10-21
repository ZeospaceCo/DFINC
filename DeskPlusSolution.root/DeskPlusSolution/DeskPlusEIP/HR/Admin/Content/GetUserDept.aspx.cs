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

namespace Zeospace.DeskPlus.HR.Web.Admin.Content
{
    public partial class GetUserDept : Base.BasePageHRAdmin
    {
        #region Declaration (컨트롤 선언)


        #endregion

        #region 변수설정
        public int DeptID;
        public string DeptNm;
        public string Func;
        protected DateTime dtTargetDate;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Admin.Content.GetUserDept).Assembly;	    //어셈블리 선언
            #endregion

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

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
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptNm = GetDecryptParam("deptNm", "");
            Func = GetDecryptParam("func", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = this.GetResource("BtnConfirm");
                CbtnClose.Text = this.GetResource("BtnClose");
            }
        }

        private void LoadData()
        {
            LoadParam();

            SetStartEndTime();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = DeptID;

            this.DeptUserTreeScript1.UseCheckBoxGroup = (GetDecryptParam("NoGroup", "").Equals("")) ? true : false;
            this.DeptUserTreeScript1.UseCheckBoxMember = (GetDecryptParam("NoMember", "").Equals("")) ? true : false;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            ddlEtcWSTime.Items.Clear();
            ddlEtcWCTime.Items.Clear();

            DateTime dtFromTime = dtTargetDate;
            DateTime dtToTime = dtTargetDate.AddDays(1);

            while (dtFromTime < dtToTime)
            {
                //유연근무대상 출근시간
                ListItem tmpItemEtcFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlEtcWSTime.Items.Add(tmpItemEtcFrom);

                //유연근무대상 퇴근시간
                ListItem tmpItemEtcTo = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlEtcWCTime.Items.Add(tmpItemEtcTo);

                dtFromTime = dtFromTime.AddMinutes(30);
            }

            ddlEtcWSTime.SelectedIndex = 18;
            ddlEtcWCTime.SelectedIndex = ddlEtcWSTime.SelectedIndex + 18;
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

        /// <summary>
        ///
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}