using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Content.Popup
{
    public partial class ViewLate : Base.BasePageHR
    {
        #region Declaration (컨트롤 선언)

        public int UserID = 0;
        public int DeptID = 0;
        public string PathID = string.Empty;
        public string UserName = string.Empty;
        public string DeptBase = string.Empty;
        public int MinClosingID = 0;
        public int MaxClosingID = 0;
        public string StartDate = string.Empty;
        public string EndDate = string.Empty;
        public string Gubun = string.Empty;
        public string bPage = string.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";		        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.Popup.ViewLate).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = resMng.GetString("BtnClose");
            }
        }

        private void LoadData()
        {
            LoadParam();

            ltlHeader.Text = UserName + " " + " (" + StartDate + " ~ " + EndDate + ")";

            if (bPage == "ListDept")
            {
                ddlUser.Visible = false;
                sec.Visible = false;
            }
            else
            {
                GetLateUser();
            }
        }

        private void LoadParam()
        {
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            PathID = GetDecryptParam("pathID", "");
            UserName = GetDecryptParam("userName", "").Replace("&", "＆");
            DeptBase = GetDecryptParam("deptBase", "").Replace("&", "＆");
            StartDate = GetDecryptParam("startDate", "");
            EndDate = GetDecryptParam("endDate", "");
            MinClosingID = GetDecryptParam("minClosingID", "0").ToNumber<int>();
            MaxClosingID = GetDecryptParam("maxClosingID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "1");
            bPage = GetDecryptParam("bPage", "");
        }

        private void GetLateUser()
        {
            ddlUser.Items.Add(new ListItem(resMng.GetString("ListType0"), "0"));

            DAL.Attendance dal = new DAL.Attendance();
            DataSet ds = dal.GetDeptHolyInfo(CurrentUserInfo.CompanyID, 0, DeptID, PathID, StartDate, EndDate, "", "", 1, MinClosingID, MaxClosingID, Gubun);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ddlUser.Items.Add(new ListItem(dbRow["NameBase"].ToString(), dbRow["UserID"].ToString()));
            }

            if (UserID != 0)
            {
                ddlUser.SelectedValue = UserID.ToString();
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}