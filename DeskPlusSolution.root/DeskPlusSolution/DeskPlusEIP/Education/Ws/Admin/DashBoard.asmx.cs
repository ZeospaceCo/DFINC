using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using SearchInfo = Zeospace.DeskPlus.Education.Web.Admin.DashBoardSearch;

namespace Zeospace.DeskPlus.Education.Web.Ws.Admin
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DashBoard : Base.BaseWSEducation
    {
        /// <summary>
        /// 검색 범위의 건수와 대상자 완료율을 조회합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.DashBoardInfo GetList(string p)
        {
            SearchInfo search = LoadSearch(p, false);
            BE.DashBoardInfo info = new BE.DashBoardInfo();
            DateTime today = BLL.List.GetRegistrationToday();
            using (DataSet data = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                search.StartDate, search.EndDate, search.ItemID, search.Target, search.TargetIDs))
            {
                if (data == null || data.Tables.Count < 2)
                    throw new InvalidOperationException("현황을 조회하지 못했습니다.");
                Dictionary<string, string> itemNames = GetItemNames();
                foreach (DataRow row in data.Tables[1].Rows)
                {
                    BE.DashBoardCourseInfo course = new BE.DashBoardCourseInfo();
                    course.ListID = GetCount(row["ListID"]);
                    string itemID = row["ItemID"].ToString();
                    string itemName;
                    course.ItemName = itemNames.TryGetValue(itemID, out itemName) ? itemName : itemID.ToReverseXSS();
                    course.Title = row["Title"].ToString().ToReverseXSS();
                    // 운영현황 건수와 진행률이 같은 과정명 검색 범위를 사용합니다.
                    if (!String.IsNullOrEmpty(search.SearchTitle)
                        && course.Title.IndexOf(search.SearchTitle, StringComparison.OrdinalIgnoreCase) < 0)
                        continue;
                    course.DateStr = GetDate(row["StartDate"]) + " ~ " + GetDate(row["EndDate"]);
                    course.TotalCount = GetCount(row["TotalCount"]);
                    course.WaitCount = GetCount(row["WaitCount"]);
                    course.ProgressCount = GetCount(row["ProgressCount"]);
                    course.CompleteCount = GetCount(row["CompleteCount"]);
                    course.DelayCount = GetCount(row["DelayCount"]);
                    course.CompleteRate = GetRate(course.CompleteCount, course.TotalCount);
                    course.Status = GetCourseStatus(row["StartDate"], row["EndDate"],
                        course.TotalCount, course.CompleteCount, today);
                    info.ListData.Add(course);
                    AddCourseSummary(info, course.Status);
                }
                info.CompleteRate = GetRate(info.CompleteCount, info.TotalCount);
            }
            return info;
        }

        private SearchInfo LoadSearch(string p, bool detail)
        {
            if (!String.Equals(Context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(405, "POST 요청만 사용할 수 있습니다.");
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "현황을 조회할 권한이 없습니다.");

            if (String.IsNullOrWhiteSpace(p) || p == "!NULL")
                throw new HttpException(400, "검색조건이 올바르지 않습니다.");
            string decrypted = Utility.DecryptString(HttpUtility.UrlDecode(p));
            if (String.IsNullOrWhiteSpace(decrypted) || decrypted == "!NULL")
                throw new HttpException(400, "검색조건이 올바르지 않습니다.");
            SetDecryptString(p);
            try
            {
                return SearchInfo.Parse(GetDecryptParam, CurrentUserInfo.CompanyID, detail);
            }
            catch (ArgumentException ex)
            {
                throw new HttpException(400, ex.Message);
            }
        }

        private Dictionary<string, string> GetItemNames()
        {
            Dictionary<string, string> names = new Dictionary<string, string>();
            DataView codes = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
            foreach (DataRowView code in codes)
            {
                names[code["CodeID"].ToString()] = GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                    code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                    code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), PreLang).ToReverseXSS();
            }
            return names;
        }

        private static int GetCount(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            int count;
            return Int32.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.None,
                CultureInfo.InvariantCulture, out count) && count >= 0 ? count : 0;
        }

        private static decimal GetRate(int completed, int total)
        {
            if (total <= 0) return 0m;
            decimal rate = Math.Round((decimal)completed * 100m / total, 1, MidpointRounding.AwayFromZero);
            return completed < total ? Math.Min(rate, 99.9m) : rate;
        }

        private static string GetCourseStatus(object startValue, object endValue,
            int totalCount, int completeCount, DateTime today)
        {
            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParseExact(GetDate(startValue), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate)
                || !DateTime.TryParseExact(GetDate(endValue), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate)
                || startDate > endDate)
                throw new InvalidOperationException("기간이 올바르지 않아 진행상태를 계산하지 못했습니다.");

            // 표시용으로 반올림한 완료율이 아니라 실제 완료 인원으로 100%를 판단합니다.
            if (totalCount > 0 && completeCount == totalCount) return "3";
            if (today.Date < startDate) return "1";
            if (today.Date > endDate) return "4";
            return "2";
        }

        private static void AddCourseSummary(BE.DashBoardInfo info, string status)
        {
            info.TotalCount++;
            switch (status)
            {
                case "1": info.WaitCount++; break;
                case "2": info.ProgressCount++; break;
                case "3": info.CompleteCount++; break;
                case "4": info.DelayCount++; break;
                default: throw new InvalidOperationException("진행상태가 올바르지 않습니다.");
            }
        }

        private static string GetDate(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime date;
            return DateTime.TryParseExact(Convert.ToString(value, CultureInfo.InvariantCulture).Trim(),
                new[] { "yyyyMMdd", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "";
        }

    }
}
