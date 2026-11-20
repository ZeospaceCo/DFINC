using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class List : Base.BasePageEducationAdmin
    {
        #region 변수설정
        public string RecordPerPage = String.Empty;
        public string InitialSearchJson = "{}";
        public string ReturnDashBoardUrl = "/DeskPlusEIP/Education/Admin/DashBoard.aspx";
        public string DefaultSearchMonth
        {
            get { return DashBoardSearch.GetCurrentMonth().ToString("yyyy-MM", CultureInfo.InvariantCulture); }
        }
        public string DefaultSearchStartMonth
        {
            get { return DefaultSearchMonth.Substring(0, 4) + "-01"; }
        }
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.List).Assembly;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!AccessAuth)
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                form1.Visible = false;
                return;
            }

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
            ddlItem.Items.Clear();
            ddlItem.Items.Add(new ListItem("전체", ""));
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
            foreach (DataRowView code in dv)
            {
                if (code["ActiveCode"].ToString() != "1") continue;
                string name = GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                    code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                    code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                ddlItem.Items.Add(new ListItem(name, code["CodeID"].ToString()));
            }

            #region PageSize 설정하기
            RecordPerPage = "20";
            DAL.ListUser dalUser = new DAL.ListUser();
            foreach (BE.ListUserInfo userInfo in dalUser.GetList(CurrentUserInfo.CompanyID, "AL", CurrentUserInfo.UserID))
            {
                if (ddlListCount.Items.FindByValue(userInfo.PageSize.ToString()) != null)
                    RecordPerPage = userInfo.PageSize.ToString();
            }
            string requestPageSize = GetDecryptParam("pageSize", RecordPerPage);
            if (ddlListCount.Items.FindByValue(requestPageSize) != null) RecordPerPage = requestPageSize;
            ddlListCount.SelectedValue = RecordPerPage;
            #endregion

            SetSelectedValue(ddlItem, GetDecryptParam("searchItem", ""));
            SetSelectedValue(ddlSearchTarget, GetDecryptParam("searchTarget", "A"));
            SetSelectedValue(ddlSearchUserField, GetDecryptParam("searchUserField", "Name"));
            string searchEmployment = GetDecryptParam("searchEmployment", "");
            if (searchEmployment != "1" && searchEmployment != "0") searchEmployment = "";
            string searchComplete = GetDecryptParam("searchComplete", "");
            if (searchComplete != "3" && searchComplete != "0") searchComplete = "";
            string searchStatus = GetDecryptParam("searchStatus", "");
            if (searchStatus != "1" && searchStatus != "2" && searchStatus != "3" && searchStatus != "4") searchStatus = "";
            SetSelectedValue(ddlEmployment, searchEmployment);
            SetSelectedValue(ddlComplete, searchComplete);
            SetSelectedValue(ddlStatus, searchStatus);
            string searchConfirm = GetDecryptParam("searchConfirm", "");
            if (searchConfirm != "Y" && searchConfirm != "N") searchConfirm = "";
            SetSelectedValue(ddlConfirm, searchConfirm);
            ctbSearchUser.Text = GetDecryptParam("searchUserValue", "");
            ctbSearchTitle.Text = GetDecryptParam("searchTitle", "");

            int searchListID = ParseSearchListID(GetDecryptParam("searchListID", ""));
            string searchListTitle = "";
            if (searchListID > 0)
            {
                using (DataTableReader reader = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                    searchListID.ToString(CultureInfo.InvariantCulture)))
                {
                    if (reader == null || !reader.Read())
                        throw new HttpException(404, "조회할 내용을 확인할 수 없습니다.");
                    searchListTitle = reader["Title"].ToString().ToReverseXSS();
                }
                ctbSearchTitle.Text = searchListTitle;
            }

            var search = new Dictionary<string, object>();
            string dashBoardSearch = GetDecryptParam("dashBoardSearch", "");
            if (!String.IsNullOrEmpty(dashBoardSearch))
                ReturnDashBoardUrl += "?p=" + HttpUtility.UrlEncode(dashBoardSearch);
            search.Add("dashBoardSearch", dashBoardSearch);
            search.Add("searchItem", ddlItem.SelectedValue);
            search.Add("searchDateField", "StartDate");
            search.Add("startDate", GetSearchMonth(GetDecryptParam("startDate", ""), DefaultSearchStartMonth));
            search.Add("endDate", GetSearchMonth(GetDecryptParam("endDate", ""), DefaultSearchMonth));
            search.Add("searchTarget", ddlSearchTarget.SelectedValue);
            search.Add("targetIDs", GetDecryptParam("targetIDs", ""));
            search.Add("targetNames", GetDecryptParam("targetNames", ""));
            search.Add("searchUserField", ddlSearchUserField.SelectedValue);
            search.Add("searchUserValue", ctbSearchUser.Text);
            search.Add("searchTitle", ctbSearchTitle.Text);
            search.Add("searchEmployment", searchEmployment);
            search.Add("searchComplete", searchComplete);
            search.Add("searchStatus", searchStatus);
            search.Add("searchConfirm", searchConfirm);
            search.Add("searchListID", searchListID);
            search.Add("searchListTitle", searchListTitle);
            search.Add("currentPage", GetDecryptParam("currentPage", "1"));
            search.Add("pageSize", RecordPerPage);
            // 검색어가 script 태그를 종료하지 않도록 JSON의 HTML 관련 문자를 이스케이프합니다.
            InitialSearchJson = new JavaScriptSerializer().Serialize(search)
                .Replace("<", "\\u003c").Replace(">", "\\u003e").Replace("&", "\\u0026")
                .Replace("\u2028", "\\u2028").Replace("\u2029", "\\u2029");
        }
        #endregion

        #region (함수)
        private static int ParseSearchListID(string value)
        {
            value = (value ?? "").Trim();
            if (value == "") return 0;
            int listID;
            if (!Int32.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out listID) || listID < 0)
                throw new HttpException(400, "검색조건이 올바르지 않습니다.");
            return listID;
        }

        private string GetSearchMonth(string value, string defaultMonth)
        {
            DateTime date;
            value = (value ?? "").Trim();
            // 상세 보기에서 돌아올 때 전달되는 날짜 검색값도 월 단위로 복원합니다.
            if (DateTime.TryParseExact(value, new[] { "yyyy-MM", "yyyy-MM-dd", "yyyyMMdd" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date) && date.Year >= 1900)
                return date.ToString("yyyy-MM", CultureInfo.InvariantCulture);
            string cultureDate = DateHelpers.GetCultureDate(value, "yyyyMMdd", PreLangReal);
            if (DateTime.TryParseExact(cultureDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date) && date.Year >= 1900)
                return date.ToString("yyyy-MM", CultureInfo.InvariantCulture);
            return defaultMonth;
        }

        private void SetLanguage()
        {
            this.GetResourceManager();
        }

        private void SetSelectedValue(DropDownList control, string value)
        {
            if (control.Items.FindByValue(value) != null) control.SelectedValue = value;
        }
        #endregion
    }
}
