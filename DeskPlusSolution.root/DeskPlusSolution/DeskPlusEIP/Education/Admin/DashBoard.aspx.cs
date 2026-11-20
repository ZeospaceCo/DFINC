using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class DashBoard : Base.BasePageEducationAdmin
    {
        public string DefaultSearchMonth
        {
            get { return DashBoardSearch.GetCurrentMonth().ToString("yyyy-MM", CultureInfo.InvariantCulture); }
        }
        public string DefaultSearchStartMonth
        {
            get { return DefaultSearchMonth.Substring(0, 4) + "-01"; }
        }
        public string InitialSearchJson = "{}";
        public bool HasAccess { get; private set; }

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";			
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.DashBoard).Assembly;	
            #endregion

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
                try
                {
                    LoadData();
                }
                catch (ArgumentException ex)
                {
                    throw new HttpException(400, ex.Message);
                }
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            string startYM;
            string endYM;
            LoadMonthSearch(out startYM, out endYM);

            ddlGubun.Items.Clear();
            ddlGubun.Items.Add(new ListItem("전체", ""));
            DataView codes = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
            foreach (DataRowView code in codes)
            {
                if (code["ActiveCode"].ToString() != "1") continue;
                string name = GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                    code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                    code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                ddlGubun.Items.Add(new ListItem(name, code["CodeID"].ToString()));
            }

            DashBoardSearch search = DashBoardSearch.Parse(delegate(string key, string defaultValue)
            {
                if (key == "startYM") return startYM;
                if (key == "endYM") return endYM;
                return GetDecryptParam(key, defaultValue);
            }, CurrentUserInfo.CompanyID, false);
            ddlTarget.SelectedValue = search.Target;
            ddlGubun.SelectedValue = search.ItemID;
            ddlCourseStatus.SelectedValue = search.CourseStatus;
            SearchTarget = search.Target;
            SearchItem = search.ItemID;
            InitialSearchJson = search.ToJson();
            HasAccess = true;
        }

        private void LoadMonthSearch(out string startYM, out string endYM)
        {
            DateTime currentMonth = DashBoardSearch.GetCurrentMonth();
            string currentYM = currentMonth.ToString("yyyy-MM", CultureInfo.InvariantCulture);
            string startOfYearYM = currentMonth.ToString("yyyy", CultureInfo.InvariantCulture) + "-01";
            startYM = GetDecryptParam("startYM", null);
            endYM = GetDecryptParam("endYM", null);
            if (startYM == null && endYM == null)
            {
                string[] legacyMonths = GetDecryptParam("searchDateValue", "").Split(',');
                if (legacyMonths.Length == 2)
                {
                    startYM = legacyMonths[0];
                    endYM = legacyMonths[1];
                }
            }
            startYM = (startYM ?? "").Trim();
            endYM = (endYM ?? "").Trim();
            DateTime parsedMonth;
            if (!DashBoardSearch.TryParseMonth(startYM, out parsedMonth)) startYM = startOfYearYM;
            if (!DashBoardSearch.TryParseMonth(endYM, out parsedMonth)) endYM = currentYM;
            if (String.CompareOrdinal(startYM, endYM) > 0)
            {
                startYM = startOfYearYM;
                endYM = currentYM;
            }
            tbxStartYM.Text = startYM;
            tbxEndYM.Text = endYM;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        #endregion
    }

    /// <summary>
    /// 대시보드와 상태별 대상자 팝업에서 동일한 검색 범위를 사용합니다.
    /// </summary>
    internal sealed class DashBoardSearch
    {
        internal string StartYM { get; private set; }
        internal string EndYM { get; private set; }
        internal string StartDate { get; private set; }
        internal string EndDate { get; private set; }
        internal string Target { get; private set; }
        internal string TargetIDs { get; private set; }
        internal string TargetNames { get; private set; }
        internal string ItemID { get; private set; }
        internal string SearchTitle { get; private set; }
        internal string CourseStatus { get; private set; }
        internal string Status { get; private set; }
        internal int ListID { get; private set; }

        internal static DateTime GetCurrentMonth()
        {
            DateTime koreaNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"));
            return new DateTime(koreaNow.Year, koreaNow.Month, 1);
        }

        internal static bool TryParseMonth(string value, out DateTime month)
        {
            month = DateTime.MinValue;
            return value != null && value.Length == 7
                && DateTime.TryParseExact(value, "yyyy-MM", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out month) && month.Year >= 1900;
        }

        internal static DashBoardSearch Parse(Func<string, string, string> getParam, int companyID, bool detail)
        {
            DashBoardSearch search = new DashBoardSearch();
            string currentYM = GetCurrentMonth().ToString("yyyy-MM", CultureInfo.InvariantCulture);
            string startYM = getParam("startYM", null);
            string endYM = getParam("endYM", null);
            if (startYM == null && endYM == null)
            {
                // 신규등록 화면에서 되돌아올 때 유지하던 이전 월 검색값도 지원합니다.
                string legacyDates = getParam("searchDateValue", "") ?? "";
                if (legacyDates != "")
                {
                    string[] months = legacyDates.Split(',');
                    if (months.Length != 2)
                        throw new ArgumentException("조회 시작월과 종료월을 올바르게 입력해 주세요.");
                    startYM = months[0];
                    endYM = months[1];
                }
                else
                {
                    startYM = currentYM.Substring(0, 4) + "-01";
                    endYM = currentYM;
                }
            }
            search.StartYM = (startYM ?? "").Trim();
            search.EndYM = (endYM ?? "").Trim();
            DateTime startMonth;
            DateTime endMonth;
            if (!TryParseMonth(search.StartYM, out startMonth)
                || !TryParseMonth(search.EndYM, out endMonth) || startMonth > endMonth)
                throw new ArgumentException("조회 시작월과 종료월을 올바르게 입력해 주세요.");
            search.StartDate = new DateTime(startMonth.Year, startMonth.Month, 1)
                .ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            search.EndDate = new DateTime(endMonth.Year, endMonth.Month,
                DateTime.DaysInMonth(endMonth.Year, endMonth.Month)).ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            search.Target = (getParam("searchTarget", "A") ?? "").Trim();
            if (search.Target != "A" && search.Target != "G")
                throw new ArgumentException("대상 검색조건이 올바르지 않습니다.");
            search.TargetIDs = "";
            search.TargetNames = "";
            if (search.Target == "G")
            {
                string targetIDs = (getParam("targetIDs", "") ?? "").Trim();
                if (targetIDs.Length == 0 || targetIDs.Length > 10)
                    throw new ArgumentException("검색할 부서는 한 개만 선택해 주세요.");
                string[] tokens = targetIDs.Split(',');
                if (tokens.Length != 1)
                    throw new ArgumentException("부서는 한 개만 선택해 주세요.");
                HashSet<int> seen = new HashSet<int>();
                List<string> ids = new List<string>();
                foreach (string token in tokens)
                {
                    int id;
                    if (!Int32.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture, out id) || id <= 0)
                        throw new ArgumentException("선택한 부서 정보가 올바르지 않습니다.");
                    if (seen.Add(id)) ids.Add(id.ToString(CultureInfo.InvariantCulture));
                }
                search.TargetIDs = String.Join(",", ids.ToArray());
                search.TargetNames = getParam("targetNames", "") ?? "";
            }

            search.SearchTitle = (getParam("searchTitle", "") ?? "").Trim();
            if (search.SearchTitle.Length > 100)
                throw new ArgumentException("과정명 검색어는 100자 이내로 입력해 주세요.");

            search.ItemID = (getParam("searchItem", "") ?? "").Trim();
            if (search.ItemID.Length > 10)
                throw new ArgumentException("분류 검색조건이 올바르지 않습니다.");
            if (search.ItemID != "")
            {
                bool found = false;
                DataView codes = AppSettingHelper.GetSysCode(companyID, "EducationItem");
                foreach (DataRowView code in codes)
                {
                    if (code["ActiveCode"].ToString() == "1" && code["CodeID"].ToString() == search.ItemID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) throw new ArgumentException("선택한 분류가 올바르지 않습니다.");
            }

            search.CourseStatus = (getParam("courseStatus", "") ?? "").Trim();
            if (search.CourseStatus != "" && search.CourseStatus != "1" && search.CourseStatus != "2"
                && search.CourseStatus != "3" && search.CourseStatus != "4")
                throw new ArgumentException("진행상태 검색조건이 올바르지 않습니다.");

            search.Status = "";
            search.ListID = 0;
            if (detail)
            {
                search.Status = (getParam("searchStatus", "") ?? "").Trim();
                if (search.Status != "" && search.Status != "1" && search.Status != "2"
                    && search.Status != "3" && search.Status != "4")
                    throw new ArgumentException("진행상태 검색조건이 올바르지 않습니다.");
                int listID;
                if (!Int32.TryParse(getParam("listID", "0"), NumberStyles.None,
                    CultureInfo.InvariantCulture, out listID) || listID < 0)
                    throw new ArgumentException("조회할 정보가 올바르지 않습니다.");
                search.ListID = listID;
            }
            return search;
        }

        internal string ToJson()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("startYM", StartYM);
            values.Add("endYM", EndYM);
            values.Add("searchTarget", Target);
            values.Add("targetIDs", TargetIDs);
            values.Add("targetNames", TargetNames);
            values.Add("searchItem", ItemID);
            values.Add("searchTitle", SearchTitle);
            values.Add("courseStatus", CourseStatus);
            values.Add("searchStatus", Status);
            values.Add("listID", ListID);
            return new JavaScriptSerializer().Serialize(values)
                .Replace("<", "\\u003c").Replace(">", "\\u003e").Replace("&", "\\u0026")
                .Replace("\u2028", "\\u2028").Replace("\u2029", "\\u2029");
        }
    }
}
