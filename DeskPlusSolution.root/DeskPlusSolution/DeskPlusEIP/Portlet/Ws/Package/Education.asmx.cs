using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.Globalization;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// 현재 사용자의 교육 포틀릿 목록을 조회합니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Education : Base.BaseWSPortlet
    {

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            if (_totalCnt <= 0) return "0,0";

            int pageSize = NormalizePageSize(_pageSize);
            int pageCount = GetPageCount(_totalCnt, pageSize);
            int pageNo = Math.Min(Math.Max(1, _pageNo), pageCount);

            // 큰 요청값에서도 곱셈이 넘치지 않도록 시작 위치를 long으로 계산합니다.
            long pageOffset = (long)pageSize * (pageNo - 1);
            int outputCount = (int)Math.Min((long)pageSize, (long)_totalCnt - pageOffset);

            return outputCount.ToString(CultureInfo.InvariantCulture) + ","
                + pageOffset.ToString(CultureInfo.InvariantCulture);
        }

        private static int NormalizePageSize(int pageSize)
        {
            switch (pageSize)
            {
                case 5:
                case 10:
                case 20:
                case 30:
                    return pageSize;
                default:
                    return 5;
            }
        }

        private static int GetPageCount(int totalCount, int pageSize)
        {
            return totalCount <= 0 ? 1 : (int)(((long)totalCount + pageSize - 1) / pageSize);
        }

        private static string GetStatusName(string status)
        {
            switch (status)
            {
                case "1": return "대기";
                case "2": return "진행";
                case "3": return "완료";
                case "4": return "지연";
                default: return String.Empty;
            }
        }

        private static string GetPeriod(string startDate, string endDate)
        {
            DateTime start;
            DateTime end;
            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out start)
                || !DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
                return String.Empty;

            return start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "~"
                + end.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public EducationListResult GetList(string p)
        {
            try
            {
                SetDecryptString(p);

                int requestedPageSize;
                int requestedPageNo;
                Int32.TryParse(GetDecryptParam("pageSize", "5"), NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out requestedPageSize);
                Int32.TryParse(GetDecryptParam("pageSizeNo", "1"), NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out requestedPageNo);

                int pageSize = NormalizePageSize(requestedPageSize);
                int pageNo = Math.Max(1, requestedPageNo);

                int companyID = CurrentUserInfo.CompanyID;
                int userID = CurrentUserInfo.UserID;

                DAL.Education dal = new DAL.Education();
                // 화면 진입 때의 건수가 아닌 현재 건수로 새로고침과 페이지 이동을 계산합니다.
                int totalCount = Math.Max(0, dal.GetPageListCount(companyID, userID));
                pageNo = Math.Min(pageNo, GetPageCount(totalCount, pageSize));

                EducationListResult result = new EducationListResult();
                result.TotalCount = totalCount;
                result.PageNo = pageNo;
                result.PageSize = pageSize;
                if (totalCount == 0) return result;

                string rtnSetPaging = SetPaging(totalCount, pageSize, pageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                using (DataTableReader dtr = dal.GetList(companyID, setPageingStr[0], setPageingStr[1], userID))
                {
                    if (dtr != null)
                    {
                        while (dtr.Read())
                        {
                            EducationPageList info = new EducationPageList();

                            info.CompanyID = companyID;
                            info.ListID = Convert.ToInt32(dtr["ListID"], CultureInfo.InvariantCulture);
                            info.Title = dtr["Title"].ToString();
                            info.Status = dtr["Status"].ToString();
                            info.StatusStr = GetStatusName(info.Status);
                            info.Period = GetPeriod(dtr["StartDate"].ToString(), dtr["EndDate"].ToString());

                            result.List.Add(info);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }

    [Serializable]
    public class EducationListResult
    {
        public EducationListResult()
        {
            List = new List<EducationPageList>();
        }

        public int TotalCount { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public List<EducationPageList> List { get; set; }
    }

    [Serializable]
    public class EducationPageList
    {
        public EducationPageList() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string StatusStr { get; set; }
        public string Period { get; set; }
    }
}
