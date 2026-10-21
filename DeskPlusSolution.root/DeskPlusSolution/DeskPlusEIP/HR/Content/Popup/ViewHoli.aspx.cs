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
    public partial class ViewHoli : Base.BasePageHR
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
        public string bPage = string.Empty;

        public string TotalCount = string.Empty;    //총카운트

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";		        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.Popup.ViewPrivateEtc).Assembly;	//어셈블리 선언
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

            ltlHeader.Text = UserName + " " + resMng.GetString("HoliList") + " (" + StartDate + " ~ " + EndDate + ")";

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "HRHoli", CurrentUserInfo.UserID, "User");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage.ToString();

            GetListCount();

            if (bPage == "ListDept")
            {
                ddlUser.Visible = false;
                sec.Visible = false;
            }
            else
            {
                GetHoliUser();
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
            bPage = GetDecryptParam("bPage", "");
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 총 카운트
        /// </summary>
        private void GetListCount()
        {
            StartDate = DateHelper.GetCultureDate(StartDate, "yyyy-MM-dd", PreLangReal);
            EndDate = DateHelper.GetCultureDate(EndDate, "yyyy-MM-dd", PreLangReal);

            DAL.Holiday dal = new DAL.Holiday();
            TotalCount = dal.GetHoliListCount(CurrentUserInfo.CompanyID, UserID, DeptID, PathID, StartDate, EndDate, MinClosingID, MaxClosingID).ToString();
        }

        private void GetHoliUser()
        {
            ddlUser.Items.Add(new ListItem(resMng.GetString("ListType0"), "0"));

            DAL.Holiday dal = new DAL.Holiday();
            DataSet ds = dal.GetHoliUserInfo(CurrentUserInfo.CompanyID, 0, DeptID, PathID, StartDate, EndDate, MinClosingID, MaxClosingID);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ddlUser.Items.Add(new ListItem(dbRow["NameBase"].ToString(), dbRow["UserID"].ToString()));
            }

            if (UserID != 0)
            {
                ddlUser.SelectedValue = UserID.ToString();
            }
        }

        /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }

            return tmpVal;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}